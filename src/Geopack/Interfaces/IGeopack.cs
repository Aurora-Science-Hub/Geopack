using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack.Interfaces;

/// <summary>
/// Geopack-2008 Double Precision contracts
/// </summary>
public interface IGeopack
{
    /// <summary>
    /// Calculates components of the main (internal) geomagnetic field in the geocentric solar-wind
    /// (GSW) coordinate system, using IAGA international geomagnetic reference model coefficients
    /// (e.g., https://www.ngdc.noaa.gov/IAGA/vmod/coeffs/igrf13coeffs.txt, revised 01 January 2020)
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: IGRF_GSW_08
    /// The GSW system is essentially similar to the standard GSM (the two systems become identical
    /// to each other in the case of strictly anti-sunward solar wind flow). For a detailed
    /// definition, see introductory comments for the subroutine GswGse_08.
    /// Before the first call of this subroutine, or, if the date/time,
    /// or the solar wind velocity components (VGSEX, VGSEY, VGSEZ) have changed, the model coefficients
    /// and geo-gsw rotation matrix elements should be updated by calling the subroutine Recalc_08.
    /// </remarks>
    /// <param name="xgsw">Cartesian GSW X-coordinate (in units RE=6371.2 km)</param>
    /// <param name="ygsw">Cartesian GSW Y-coordinate (in units RE=6371.2 km)</param>
    /// <param name="zgsw">Cartesian GSW Z-coordinate (in units RE=6371.2 km)</param>
    CartesianFieldVector IgrfGsw_08(double xgsw, double ygsw, double zgsw);

    /// <summary>
    /// Calculates components of the main (internal) geomagnetic field in the spherical geographic
    /// (geocentric) coordinate system, using IAGA international geomagnetic reference model
    /// coefficients (e.g., http://www.ngdc.noaa.gov/IAGA/vmod/igrf.html, revised: 22 March 2005)
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: IGRF_GEO_08
    /// Before the first call of this subroutine, or if the date was changed,
    /// the model coefficients should be updated by calling the subroutine Recalc_08
    /// </remarks>
    /// <param name="r">Spherical geographic (geocentric) coordinates: radial distance R (in units RE=6371.2 km)</param>
    /// <param name="theta">Colatitude THETA in radians</param>
    /// <param name="phi">Longitude PHI in radians</param>
    SphericalFieldVector IgrfGeo_08(double r, double theta, double phi);

    /// <summary>
    /// Calculates GSW (geocentric solar-wind) components of geodipole field with the dipole moment
    /// corresponding to the epoch, specified by calling subroutine Recalc_08 (should be
    /// invoked before the first use of this one, or if the date/time, and/or the observed
    /// solar wind direction, have changed).
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: DIP_08
    /// The GSW coordinate system is essentially similar to the standard GSM (the two systems become
    /// identical to each other in the case of strictly radial anti-sunward solar wind flow). Its
    /// detailed definition is given in introductory comments for the subroutine GswGse_08.
    /// </remarks>
    /// <param name="xgsw">Cartesian GSW X-coordinate in RE (1 RE = 6371.2 km)</param>
    /// <param name="ygsw">Cartesian GSW Y-coordinate in RE (1 RE = 6371.2 km)</param>
    /// <param name="zgsw">Cartesian GSW Z-coordinate in RE (1 RE = 6371.2 km)</param>
    CartesianFieldVector Dip_08(double xgsw, double ygsw, double zgsw);

    /// <summary>
    /// Calculates four quantities necessary for coordinate transformations
    /// which depend on sun position (and, hence, on universal time and season)
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: SUN_08
    /// </remarks>
    /// <param name="dateTime">Year, day, and universal time in hours, minutes, and seconds</param>
    Sun Sun_08(DateTime dateTime);

    /// <summary>
    /// Converts spherical coordinates into Cartesian ones.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: SPHCAR_08
    /// THETA and PHI in radians, R in Earth radii.
    /// At the poles (X=0 and Y=0), PHI is assumed to be 0.
    /// </remarks>
    /// <param name="r">Radial distance in RE</param>
    /// <param name="theta">Co-latitude THETA in radians</param>
    /// <param name="phi">Longitude PHI in radians</param>
    CartesianLocation SphCar_08(double r, double theta, double phi);

    /// <summary>
    /// Converts Cartesian into spherical coordinates.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: SPHCAR_08
    /// THETA and PHI in radians, R in Earth radii.
    /// At the poles (X=0 and Y=0), PHI is assumed to be 0.
    /// </remarks>
    /// <param name="x">Cartesian X-coordinate</param>
    /// <param name="y">Cartesian Y-coordinate</param>
    /// <param name="z">Cartesian Z-coordinate</param>
    SphericalLocation CarSph_08(double x, double y, double z);

    /// <summary>
    /// Calculates Cartesian field components from local spherical ones.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: BSPCAR_08
    /// </remarks>
    /// <param name="theta">Spherical angle THETA of the point in radians</param>
    /// <param name="phi">Spherical angle PHI of the point in radians</param>
    /// <param name="br">Local spherical component of the field (radial)</param>
    /// <param name="btheta">Local spherical component of the field (co-latitude)</param>
    /// <param name="bphi">Local spherical component of the field (longitude)</param>
    CartesianFieldVector BSphCar_08(double theta, double phi, double br, double btheta, double bphi);

    /// <summary>
    /// Calculates local spherical field components from those in Cartesian system.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: BCARSP_08
    /// </remarks>
    /// <param name="x">Cartesian component of the position vector</param>
    /// <param name="y">Cartesian component of the position vector</param>
    /// <param name="z">Cartesian component of the position vector</param>
    /// <param name="bx">Cartesian component of the field vector</param>
    /// <param name="by">Cartesian component of the field vector</param>
    /// <param name="bz">Cartesian component of the field vector</param>
    SphericalFieldVector BCarSph_08(double x, double y, double z, double bx, double by, double bz);

    /// <summary>
    /// Prepares elements of rotation matrices for transformations of vectors between several coordinate systems,
    /// most frequently used in space physics. Also prepares coefficients used in the calculation of the main geomagnetic field (IGRF model).
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: RECALC_08.
    /// This subroutine should be invoked before using the following subroutines:
    /// IgrfGeo_08, IgrfGsw_08, Dip_08, GeoMag_08 / MagGeo, GeoGsw_08 / GswGeo_08,
    /// MagSm_08 / SmMag_08, SmGsw_08 / GswSm_08, GswGse_08 / GseGsw_08, GeiGeo_08 / GeoGei_08, Trace_08.
    /// There is no need to repeatedly invoke Recalc_08 if multiple calculations are made for the same date/time and solar wind flow direction.
    /// </remarks>
    /// <param name="dateTime">Date and time in UTC</param>
    /// <param name="vgsex">Cartesian GSE X-component of the observed solar wind flow velocity (in km/s)</param>
    /// <param name="vgsey">Cartesian GSE Y-component of the observed solar wind flow velocity (in km/s)</param>
    /// <param name="vgsez">Cartesian GSE Z-component of the observed solar wind flow velocity (in km/s)</param>
    (Common1, Common2) Recalc_08(DateTime dateTime, double vgsex = -400.0, double vgsey = 0.0, double vgsez = 0.0);

    /// <summary>
    /// Transforms components of geocentric solar-wind (GSW) system to GSE coordinate.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GSWGSE_08.
    /// In the GSW system, the X axis is antiparallel to the observed direction of the solar wind flow.
    /// The Y and Z axes are defined similarly to the standard GSM system.
    /// The GSW system becomes identical to the standard GSM in the case of a strictly radial solar wind flow.
    /// Before calling GSWGSE_08, be sure to invoke the subroutine Recalc_08 to define all necessary elements of transformation matrices.
    /// </remarks>
    /// <param name="xgsw">Cartesian GSW X-coordinate</param>
    /// <param name="ygsw">Cartesian GSW Y-coordinate</param>
    /// <param name="zgsw">Cartesian GSW Z-coordinate</param>
    CartesianLocation GswGse_08(double xgsw, double ygsw, double zgsw);

    /// <summary>
    /// Transforms GSE coordinate components to geocentric solar-wind (GSW) ones.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GSWGSE_08.
    /// In the GSW system, the X axis is antiparallel to the observed direction of the solar wind flow.
    /// The Y and Z axes are defined similarly to the standard GSM system.
    /// The GSW system becomes identical to the standard GSM in the case of a strictly radial solar wind flow.
    /// Before calling GseGsw_08, be sure to invoke the subroutine Recalc_08 to define all necessary elements of transformation matrices.
    /// </remarks>
    /// <param name="xgse">Cartesian GSE X-coordinate</param>
    /// <param name="ygse">Cartesian GSE Y-coordinate</param>
    /// <param name="zgse">Cartesian GSE Z-coordinate</param>
    CartesianLocation GseGsw_08(double xgse, double ygse, double zgse);

    /// <summary>
    /// Converts geographic (GEO) to dipole (MAG) coordinates.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GEOMAG_08.
    /// Before calling GeoMag_08, be sure to invoke the subroutine Recalc_08 in two cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have been changed.
    /// No information is required here on the solar wind velocity, so one can set VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0 in Recalc_08.
    /// </remarks>
    /// <param name="xgeo">Cartesian GEO X-coordinate</param>
    /// <param name="ygeo">Cartesian GEO Y-coordinate</param>
    /// <param name="zgeo">Cartesian GEO Z-coordinate</param>
    CartesianLocation GeoMag_08(double xgeo, double ygeo, double zgeo);

    /// <summary>
    /// Converts dipole (MAG) coordinates to geographic (GEO).
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GEOMAG_08.
    /// Before calling MAG_GEO_08, be sure to invoke the subroutine Recalc_08 in two cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have been changed.
    /// No information is required here on the solar wind velocity, so one can set VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0 in Recalc_08.
    /// </remarks>
    /// <param name="xmag">Cartesian MAG X-coordinate</param>
    /// <param name="ymag">Cartesian MAG Y-coordinate</param>
    /// <param name="zmag">Cartesian MAG Z-coordinate</param>
    CartesianLocation MagGeo_08(double xmag, double ymag, double zmag);

    /// <summary>
    /// Converts equatorial inertial (GEI) to geographical (GEO) coordinates.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GEIGEO_08.
    /// Before calling GEI_GEO_08, be sure to invoke the subroutine Recalc_08 in two cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the current values date/time have been changed.
    /// No information is required here on the solar wind velocity, so one can set VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0 in Recalc_08.
    /// </remarks>
    /// <param name="xgei">Cartesian GEI X-coordinate</param>
    /// <param name="ygei">Cartesian GEI Y-coordinate</param>
    /// <param name="zgei">Cartesian GEI Z-coordinate</param>
    CartesianLocation GeiGeo_08(double xgei, double ygei, double zgei);

    /// <summary>
    /// Converts geographical (GEO) coordinates to equatorial inertial (GEI).
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GEIGEO_08.
    /// Before calling GEO_GEI_08, be sure to invoke the subroutine Recalc_08 in two cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the current values date/time have been changed.
    /// No information is required here on the solar wind velocity, so one can set VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0 in Recalc_08.
    /// </remarks>
    /// <param name="xgeo">Cartesian GEO X-coordinate</param>
    /// <param name="ygeo">Cartesian GEO Y-coordinate</param>
    /// <param name="zgeo">Cartesian GEO Z-coordinate</param>
    CartesianLocation GeoGei_08(double xgeo, double ygeo, double zgeo);

    /// <summary>
    /// Converts dipole (MAG) to solar magnetic (SM) coordinates.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: MAGSM_08.
    /// Before calling MAG_SM_08, be sure to invoke the subroutine Recalc_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// A non-standard definition is implied here for the solar magnetic coordinate system:
    /// it is assumed that the XSM axis lies in the plane defined by the geodipole axis and the observed vector of the solar wind flow
    /// (rather than the Earth-Sun line). In order to convert MAG coordinates to and from the standard SM coordinates,
    /// invoke Recalc_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xmag">Cartesian MAG X-coordinate</param>
    /// <param name="ymag">Cartesian MAG Y-coordinate</param>
    /// <param name="zmag">Cartesian MAG Z-coordinate</param>
    CartesianLocation MagSm_08(double xmag, double ymag, double zmag);

    /// <summary>
    /// Converts solar magnetic (SM) to dipole (MAG) coordinates.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: MAGSM_08.
    /// Before calling SM_MAG_08, be sure to invoke the subroutine Recalc_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// A non-standard definition is implied here for the solar magnetic coordinate system:
    /// it is assumed that the XSM axis lies in the plane defined by the geodipole axis and the observed vector of the solar wind flow
    /// (rather than the Earth-Sun line). In order to convert MAG coordinates to and from the standard SM coordinates,
    /// invoke Recalc_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xsm">Cartesian SM X-coordinate</param>
    /// <param name="ysm">Cartesian SM Y-coordinate</param>
    /// <param name="zsm">Cartesian SM Z-coordinate</param>
    CartesianLocation SmMag_08(double xsm, double ysm, double zsm);

    /// <summary>
    /// Converts solar magnetic (SM) to geocentric solar-wind (GSW) coordinates.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: SMGSW_08.
    /// Before calling SM_GSW_08, be sure to invoke the subroutine Recalc_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have been changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// A non-standard definition is implied here for the solar magnetic (SM) coordinate system:
    /// it is assumed that the XSM axis lies in the plane defined by the geodipole axis and the observed vector of the solar wind flow
    /// (rather than the Earth-Sun line). In order to convert MAG coordinates to and from the standard SM coordinates,
    /// invoke Recalc_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xsm">Cartesian SM X-coordinate</param>
    /// <param name="ysm">Cartesian SM Y-coordinate</param>
    /// <param name="zsm">Cartesian SM Z-coordinate</param>
    CartesianLocation SmGsw_08(double xsm, double ysm, double zsm);

    /// <summary>
    /// Converts geocentric solar-wind (GSW) to solar magnetic (SM) coordinates.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: SMGSW_08.
    /// Before calling GSW_SM_08, be sure to invoke the subroutine Recalc_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have been changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// A non-standard definition is implied here for the solar magnetic (SM) coordinate system:
    /// it is assumed that the XSM axis lies in the plane defined by the geodipole axis and the observed vector of the solar wind flow
    /// (rather than the Earth-Sun line). In order to convert MAG coordinates to and from the standard SM coordinates,
    /// invoke Recalc_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xgsw">Cartesian GSW X-coordinate</param>
    /// <param name="ygsw">Cartesian GSW Y-coordinate</param>
    /// <param name="zgsw">Cartesian GSW Z-coordinate</param>
    CartesianLocation GswSm_08(double xgsw, double ygsw, double zgsw);

    /// <summary>
    /// Converts geographic (GEO) to geocentric solar-wind (GSW) coordinates.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GEOGSW_08.
    /// Before calling GEO_GSW_08, be sure to invoke the subroutine Recalc_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// This subroutine converts GEO vectors to and from the solar-wind GSW coordinate system,
    /// taking into account possible deflections of the solar wind direction from strictly radial.
    /// Before converting to/from standard GSM coordinates, invoke Recalc_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xgeo">Cartesian GEO X-coordinate</param>
    /// <param name="ygeo">Cartesian GEO Y-coordinate</param>
    /// <param name="zgeo">Cartesian GEO Z-coordinate</param>
    CartesianLocation GeoGsw_08(double xgeo, double ygeo, double zgeo);

    /// <summary>
    /// Converts geocentric solar-wind (GSW) to geographic (GEO) coordinates.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GEOGSW_08.
    /// Before calling GSW_GEO_08, be sure to invoke the subroutine Recalc_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// This subroutine converts GEO vectors to and from the solar-wind GSW coordinate system,
    /// taking into account possible deflections of the solar wind direction from strictly radial.
    /// Before converting to/from standard GSM coordinates, invoke Recalc_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xgsw">Cartesian GSW X-coordinate</param>
    /// <param name="ygsw">Cartesian GSW Y-coordinate</param>
    /// <param name="zgsw">Cartesian GSW Z-coordinate</param>
    CartesianLocation GswGeo_08(double xgsw, double ygsw, double zgsw);

    /// <summary>
    /// Converts vertical local height (altitude) H and geodetic latitude XMU into geocentric coordinates R and THETA.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GEODGEO_08.
    /// The subroutine uses World Geodetic System WGS84 parameters for the Earth's ellipsoid. The angular quantities
    /// (geo co-latitude THETA and geodetic latitude XMU) are in radians, and the distances (geocentric radius R and
    /// altitude H above the Earth's ellipsoid) are in kilometers.
    /// </remarks>
    /// <param name="h">Altitude (km) for geodetic input or geocentric radius (km) for geocentric input</param>
    /// <param name="xmu">Geodetic latitude (radians) for geodetic input or co-latitude (radians) for geocentric input</param>
    GeodeticGeocentricCoordinates GeodGeo_08(double h, double xmu);

    /// <summary>
    /// Converts geocentric coordinates R and THETA into vertical local height (altitude) H and geodetic latitude XMU.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: GEODGEO_08.
    /// The subroutine uses World Geodetic System WGS84 parameters for the Earth's ellipsoid. The angular quantities
    /// (geo co-latitude THETA and geodetic latitude XMU) are in radians, and the distances (geocentric radius R and
    /// altitude H above the Earth's ellipsoid) are in kilometers.
    /// </remarks>
    /// <param name="r">Geocentric radius (km) for geocentric output or altitude (km) for geodetic output</param>
    /// <param name="theta">Co-latitude (radians) for geocentric output or geodetic latitude (radians) for geodetic output</param>
    GeodeticGeocentricCoordinates GeoGeod_08(double r, double theta);

    /// <summary>
    /// Traces a field line from an arbitrary point in space to the Earth's surface or to a model limiting boundary.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: TRACE_08.
    /// This subroutine allows two options:
    /// 1. If inName=IGRF_GSW_08, then the IGRF model will be used for calculating contribution from Earth's internal sources.
    ///    In this case, subroutine Recalc_08 must be called before using TRACE_08, with properly specified date, universal time,
    ///    and solar wind velocity components, to calculate in advance all quantities needed for the main field model and for
    ///    transformations between involved coordinate systems.
    /// 2. If inName=DIP_08, then a pure dipole field will be used instead of the IGRF model. In this case, the subroutine Recalc_08
    ///    must also be called before TRACE_08. Here one can choose either to (a) calculate dipole tilt angle based on date, time,
    ///    and solar wind direction, or (b) explicitly specify that angle, without any reference to date/UT/solar wind. In the last
    ///    case (b), the sine (SPS) and cosine (CPS) of the dipole tilt angle must be specified in advance (but after having called
    ///    Recalc_08) and forwarded in the common block /GEOPACK1/ (in its 11th and 12th elements, respectively). In this case the
    ///    role of the subroutine Recalc_08 is reduced to only calculating the components of the Earth's dipole moment.
    /// </remarks>
    /// <param name="xi">GSW X-coordinate of the field line starting point (in Earth radii, 1 RE = 6371.2 km)</param>
    /// <param name="yi">GSW Y-coordinate of the field line starting point (in Earth radii, 1 RE = 6371.2 km)</param>
    /// <param name="zi">GSW Z-coordinate of the field line starting point (in Earth radii, 1 RE = 6371.2 km)</param>
    /// <param name="dir">Sign of the tracing direction: if dir=1.0 then the tracing is made antiparallel
    /// to the total field vector (e.g., from northern to southern conjugate point);
    /// if dir=-1.0 then the tracing proceeds in the opposite direction, that is, parallel to
    /// the total field vector.</param>
    /// <param name="dsMax">Upper limit on the stepsize (sets a desired maximal spacing between the field line points)</param>
    /// <param name="err">Permissible step error. A reasonable estimate providing a sufficient accuracy for most
    /// applications is err=0.0001. Smaller/larger values will result in larger/smaller number
    /// of steps and, hence, of output field line points. Note that using much smaller values
    /// of err may require using a double precision version of the entire package.</param>
    /// <param name="rLim">Radius of a sphere (in RE), defining the outer boundary of the tracing region;
    /// if the field line reaches that boundary from inside, its outbound tracing is
    /// terminated and the crossing point coordinates XF,YF,ZF are calculated.</param>
    /// <param name="r0">Radius of a sphere (in RE), defining the inner boundary of the tracing region
    /// (usually, Earth's surface or the ionosphere, where R0~1.0)
    /// if the field line reaches that sphere from outside, its inbound tracing is
    /// terminated and the crossing point coordinates XF,YF,ZF are calculated.</param>
    /// <param name="iopt">A model index; can be used for specifying a version of the external field
    /// model (e.g., a number of the Kp-index interval). Alternatively, one can use the array
    /// PARMOD for that purpose (see below); in that case IOPT is just a dummy parameter.</param>
    /// <param name="parmod">A 10-element array containing input parameters needed for a unique
    /// specification of the external field model. The concrete meaning of the components
    /// of PARMOD depends on a specific version of that model.</param>
    /// <param name="exName">Name of a subroutine providing components of the external magnetic field
    /// (e.g., T89, or T96_01, etc.)</param>
    /// <param name="inName">Name of a subroutine providing components of the internal magnetic field
    /// (either DIP_08 or IGRF_GSW_08).</param>
    /// <param name="lMax">Maximal length of the arrays XX,YY,ZZ, in which coordinates of the field
    /// line points are stored. LMAX should be set equal to the actual length of
    /// the arrays, defined in the main program as actual arguments of this subroutine.</param>
    FieldLine Trace_08(
        double xi, double yi, double zi,
        TraceDirection dir,
        double dsMax,
        double err,
        double rLim, double r0,
        int iopt, double[] parmod,
        IExternalFieldModel exName, InternalFieldModel inName,
        int lMax);

    /// <summary>
    /// For any point in space with coordinates (XGSW, YGSW, ZGSW) and specified conditions
    /// in the incoming solar wind, this subroutine:
    /// 1. Determines if the point (XGSW, YGSW, ZGSW) lies inside or outside the
    ///    model magnetopause of Shue et al. (JGR-A, V.103, P. 17691, 1998).
    /// 2. Calculates the GSW position of a point {XMGNP, YMGNP, ZMGNP}, lying at the model
    ///    magnetopause and asymptotically tending to the nearest boundary point with
    ///    respect to the observation point {XGSW, YGSW, ZGSW}, as it approaches the magnetopause.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: SHUETAL_MGNP_08.
    /// </remarks>
    /// <param name="xnPd">Either solar wind proton number density (per c.c.) (if VEL greater than 0)
    /// or the solar wind ram pressure in nanopascals (if VEL less than 0)</param>
    /// <param name="vel">Either solar wind velocity (km/sec)
    /// or any negative number, which indicates that XN_PD stands
    /// for the solar wind pressure, rather than for the density</param>
    /// <param name="bzImf">IMF BZ in nanoteslas</param>
    /// <param name="xgsw">Cartesian GSW X-coordinate of the observation point in Earth radii</param>
    /// <param name="ygsw">Cartesian GSW Y-coordinate of the observation point in Earth radii</param>
    /// <param name="zgsw">Cartesian GSW Z-coordinate of the observation point in Earth radii</param>
    Magnetopause ShuMgnp_08(
        double xnPd, double vel, double bzImf,
        double xgsw, double ygsw, double zgsw);

    /// <summary>
    /// For any point in space with given coordinates (XGSW, YGSW, ZGSW), this subroutine defines
    /// the position of a point (XMGNP, YMGNP, ZMGNP) at the T96 model magnetopause with the
    /// same value of the ellipsoidal tau-coordinate, and the distance between them.
    /// </summary>
    /// <remarks>
    /// Original Geopack-2008 method: T96_MGNP_08.
    /// This is not the shortest distance D_MIN to the boundary, but DIST asymptotically tends to D_MIN,
    /// as the observation point gets closer to the magnetopause.
    /// </remarks>
    /// <param name="xnPd">Either solar wind proton number density (per c.c.) (if VEL greater than zero)
    /// or the solar wind ram pressure in nanopascals (if VEL lower than zero)</param>
    /// <param name="vel">Either solar wind velocity (km/sec) or any negative number, which indicates that XN_PD stands
    /// for the solar wind pressure, rather than for the density</param>
    /// <param name="xgsw">Cartesian GSW X-coordinate of the observation point in Earth radii</param>
    /// <param name="ygsw">Cartesian GSW Y-coordinate of the observation point in Earth radii</param>
    /// <param name="zgsw">Cartesian GSW Z-coordinate of the observation point in Earth radii</param>
    Magnetopause T96Mgnp_08(
        double xnPd, double vel,
        double xgsw, double ygsw, double zgsw);
}
