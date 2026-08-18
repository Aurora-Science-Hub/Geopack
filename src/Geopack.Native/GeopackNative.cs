using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.PhysicalObjects;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;
using AuroraScienceHub.Geopack.Contracts.Spherical;
using Microsoft.Extensions.Logging.Abstractions;

namespace AuroraScienceHub.Geopack.Native;

/// <summary>
/// Flat C ABI over Geopack for consumption from Python (ctypes) and other languages.
/// Compiled with NativeAOT (NativeLib=Shared) into libgeopack.{dylib,so,dll}.
/// </summary>
/// <remarks>
/// Contract rules:
///  - every exported method is static, [UnmanagedCallersOnly], returns int (0 = success, non-zero = error);
///  - all outputs go through out-pointers (never exceptions, never out/ref across the boundary);
///  - a ComputationContext lives behind an opaque long handle returned by gp_context_create;
///  - the last error message (UTF-8) is available through gp_last_error (thread-local).
/// </remarks>
public static unsafe class GeopackNative
{
    private static readonly Geopack s_geopack = new(NullLogger<Geopack>.Instance);

    private static long s_nextHandle;
    private static readonly ConcurrentDictionary<long, ComputationContext> s_contexts = new();

    [ThreadStatic]
    private static string? s_lastError;

    // ------------------------------------------------------------------
    // Context lifecycle
    // ------------------------------------------------------------------

    /// <summary>
    /// Creates an immutable ComputationContext for the given UTC date/time and solar wind velocity (GSE, km/s).
    /// Defaults for the velocity (-400,0,0 km/s) are applied on the Python side.
    /// </summary>
    [UnmanagedCallersOnly(EntryPoint = "gp_context_create", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int ContextCreate(
        int year, int month, int day, int hour, int minute, int second,
        double vx, double vy, double vz, long* handle)
    {
        try
        {
            var dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
            var velocity = CartesianVector<Velocity>.New(vx, vy, vz, CoordinateSystem.GSE);
            ComputationContext context = s_geopack.Recalc(dateTime, velocity);

            long id = Interlocked.Increment(ref s_nextHandle);
            s_contexts[id] = context;

            *handle = id;
            return 0;
        }
        catch (Exception e)
        {
            return Fail(e);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "gp_context_release", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void ContextRelease(long handle)
    {
        if (handle != 0)
        {
            s_contexts.TryRemove(handle, out _);
        }
    }

    /// <summary>
    /// Copies the thread-local last error message (UTF-8, NUL-terminated) into <paramref name="buffer"/>.
    /// Returns the number of bytes written (0 if there was no pending error).
    /// </summary>
    [UnmanagedCallersOnly(EntryPoint = "gp_last_error", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int LastError(byte* buffer, int capacity)
    {
        string? error = s_lastError;
        if (error is null)
        {
            return 0;
        }

        // A call with a null buffer / zero capacity is a probe that asks for the message length.
        // The error is only cleared when the caller actually reads it into a buffer.
        if (buffer is null || capacity <= 0)
        {
            return Encoding.UTF8.GetByteCount(error);
        }

        s_lastError = null;
        int written = Encoding.UTF8.GetBytes(error, new Span<byte>(buffer, capacity - 1));
        buffer[written] = 0;
        return written;
    }

    // ------------------------------------------------------------------
    // Field models
    // ------------------------------------------------------------------

    [UnmanagedCallersOnly(EntryPoint = "gp_igrf_gsw", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int IgrfGsw(long handle, double x, double y, double z, double* bx, double* by, double* bz)
        => Field(handle, x, y, z, (ctx, loc) => s_geopack.IgrfGsw(ctx, loc), bx, by, bz);

    [UnmanagedCallersOnly(EntryPoint = "gp_dip", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int Dip(long handle, double x, double y, double z, double* bx, double* by, double* bz)
        => Field(handle, x, y, z, (ctx, loc) => s_geopack.Dip(ctx, loc), bx, by, bz);

    [UnmanagedCallersOnly(EntryPoint = "gp_igrf_geo", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int IgrfGeo(long handle, double r, double theta, double phi, double* br, double* bt, double* bp)
    {
        try
        {
            if (!s_contexts.TryGetValue(handle, out ComputationContext? context))
            {
                return Fail("Invalid context handle.");
            }

            var location = SphericalLocation.New(r, theta, phi, CoordinateSystem.GEO);
            SphericalVector<MagneticField> field = s_geopack.IgrfGeo(context, location);

            *br = field.R;
            *bt = field.Theta;
            *bp = field.Phi;
            return 0;
        }
        catch (Exception e)
        {
            return Fail(e);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "gp_sun", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int Sun(
        int year, int month, int day, int hour, int minute, int second,
        double* gst, double* slong, double* srasn, double* sdec)
    {
        try
        {
            var dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
            Sun sun = s_geopack.Sun(dateTime);

            *gst = sun.Gst;
            *slong = sun.Slong;
            *srasn = sun.Srasn;
            *sdec = sun.Sdec;
            return 0;
        }
        catch (Exception e)
        {
            return Fail(e);
        }
    }

    // ------------------------------------------------------------------
    // Coordinate transformations (CartesianLocation in -> CartesianLocation out)
    // ------------------------------------------------------------------

    [UnmanagedCallersOnly(EntryPoint = "gp_gsw_to_gse", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int GswToGse(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.GSW, (ctx, loc) => s_geopack.GswToGse(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_gse_to_gsw", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int GseToGsw(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.GSE, (ctx, loc) => s_geopack.GseToGsw(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_geo_to_mag", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int GeoToMag(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.GEO, (ctx, loc) => s_geopack.GeoToMag(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_mag_to_geo", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int MagToGeo(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.MAG, (ctx, loc) => s_geopack.MagToGeo(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_gei_to_geo", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int GeiToGeo(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.GEI, (ctx, loc) => s_geopack.GeiToGeo(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_geo_to_gei", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int GeoToGei(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.GEO, (ctx, loc) => s_geopack.GeoToGei(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_mag_to_sm", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int MagToSm(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.MAG, (ctx, loc) => s_geopack.MagToSm(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_sm_to_mag", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int SmToMag(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.SM, (ctx, loc) => s_geopack.SmToMag(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_sm_to_gsw", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int SmToGsw(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.SM, (ctx, loc) => s_geopack.SmToGsw(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_gsw_to_sm", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int GswToSm(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.GSW, (ctx, loc) => s_geopack.GswToSm(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_geo_to_gsw", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int GeoToGsw(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.GEO, (ctx, loc) => s_geopack.GeoToGsw(ctx, loc), ox, oy, oz);

    [UnmanagedCallersOnly(EntryPoint = "gp_gsw_to_geo", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int GswToGeo(long handle, double x, double y, double z, double* ox, double* oy, double* oz)
        => Transform(handle, x, y, z, CoordinateSystem.GSW, (ctx, loc) => s_geopack.GswToGeo(ctx, loc), ox, oy, oz);

    // ------------------------------------------------------------------
    // Magnetopause models (GSW)
    // ------------------------------------------------------------------

    [UnmanagedCallersOnly(EntryPoint = "gp_shu_mgnp", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int ShuMgnp(
        double xnPd, double vel, double bzImf,
        double x, double y, double z,
        double* mx, double* my, double* mz, double* dist, int* position)
        => Magnetopause(xnPd, vel, bzImf, x, y, z,
            (ctx, loc) => s_geopack.ShuMgnp(xnPd, vel, bzImf, loc),
            mx, my, mz, dist, position);

    [UnmanagedCallersOnly(EntryPoint = "gp_t96_mgnp", CallConvs = new[] { typeof(CallConvCdecl) })]
    public static int T96Mgnp(
        double xnPd, double vel,
        double x, double y, double z,
        double* mx, double* my, double* mz, double* dist, int* position)
        => Magnetopause(xnPd, vel, 0, x, y, z,
            (_, loc) => s_geopack.T96Mgnp(xnPd, vel, loc),
            mx, my, mz, dist, position);

    // ------------------------------------------------------------------
    // Shared plumbing
    // ------------------------------------------------------------------

    private static int Field(
        long handle, double x, double y, double z,
        Func<ComputationContext, CartesianLocation, CartesianVector<MagneticField>> field,
        double* bx, double* by, double* bz)
    {
        try
        {
            if (!s_contexts.TryGetValue(handle, out ComputationContext? context))
            {
                return Fail("Invalid context handle.");
            }

            var location = CartesianLocation.New(x, y, z, CoordinateSystem.GSW);
            CartesianVector<MagneticField> result = field(context, location);

            *bx = result.X;
            *by = result.Y;
            *bz = result.Z;
            return 0;
        }
        catch (Exception e)
        {
            return Fail(e);
        }
    }

    private static int Transform(
        long handle, double x, double y, double z, CoordinateSystem source,
        Func<ComputationContext, CartesianLocation, CartesianLocation> transform,
        double* ox, double* oy, double* oz)
    {
        try
        {
            if (!s_contexts.TryGetValue(handle, out ComputationContext? context))
            {
                return Fail("Invalid context handle.");
            }

            var location = CartesianLocation.New(x, y, z, source);
            CartesianLocation result = transform(context, location);

            *ox = result.X;
            *oy = result.Y;
            *oz = result.Z;
            return 0;
        }
        catch (Exception e)
        {
            return Fail(e);
        }
    }

    private static int Magnetopause(
        double xnPd, double vel, double bzImf,
        double x, double y, double z,
        Func<ComputationContext, CartesianLocation, Magnetopause> model,
        double* mx, double* my, double* mz, double* dist, int* position)
    {
        try
        {
            var location = CartesianLocation.New(x, y, z, CoordinateSystem.GSW);
            Magnetopause result = model(null!, location);

            *mx = result.BoundaryLocation.X;
            *my = result.BoundaryLocation.Y;
            *mz = result.BoundaryLocation.Z;
            *dist = result.Dist;
            *position = (int)result.Position;
            return 0;
        }
        catch (Exception e)
        {
            return Fail(e);
        }
    }

    // The exception is converted into the last-error message; the concrete exception type
    // is irrelevant to the C ABI consumer, which only sees the error code + message.
#pragma warning disable CA1031
    private static int Fail(Exception exception)
        => Fail(exception.Message);

    private static int Fail(string message)
    {
        s_lastError = message;
        return 1;
    }
#pragma warning restore CA1031
}
