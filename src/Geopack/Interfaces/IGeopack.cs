namespace AuroraScienceHub.Geopack.Interfaces;

public interface IGeopack<T>
    where T : struct
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="xgsw"></param>
    /// <param name="ygsw"></param>
    /// <param name="zgsw"></param>
    /// <param name="hzgsw"></param>
    /// <param name="hxgsw"></param>
    /// <param name="hygsw"></param>
    void IGRF_GSW_08(T xgsw, T ygsw, T zgsw, out T hxgsw, out T hygsw, out T hzgsw);

    /// <summary>
    ///
    /// </summary>
    /// <param name="r"></param>
    /// <param name="theta"></param>
    /// <param name="phi"></param>
    /// <param name="br"></param>
    /// <param name="btheta"></param>
    /// <param name="bphi"></param>
    void IGRF_GEO_08(T r, T theta, T phi, out T br, out T btheta, out T bphi);

    /// <summary>
    ///
    /// </summary>
    /// <param name="xgsw"></param>
    /// <param name="ygsw"></param>
    /// <param name="zgsw"></param>
    /// <param name="bxgsw"></param>
    /// <param name="bygsw"></param>
    /// <param name="bzgsw"></param>
    void DIP_08(T xgsw, T ygsw, T zgsw, out T bxgsw, out T bygsw, out T bzgsw);

    /// <summary>
    ///
    /// </summary>
    /// <param name="year"></param>
    /// <param name="day"></param>
    /// <param name="hour"></param>
    /// <param name="min"></param>
    /// <param name="sec"></param>
    /// <param name="gst"></param>
    /// <param name="slong"></param>
    /// <param name="srasn"></param>
    /// <param name="sdec"></param>
    void SUN_08(int year, int day, int hour, int min, int sec, T gst, T slong, T srasn, T sdec);

    /// <summary>
    ///
    /// </summary>
    /// <param name="year"></param>
    /// <param name="day"></param>
    /// <param name="hour"></param>
    /// <param name="minute"></param>
    /// <param name="second"></param>
    /// <param name="vgsex"></param>
    /// <param name="vgsey"></param>
    /// <param name="vgsez"></param>
    void RECALC_08(int year, int day, int hour, int minute, int second, T vgsex, T vgsey, T vgsez);

    /// <summary>
    ///
    /// </summary>
    /// <param name="xgsw"></param>
    /// <param name="ygsw"></param>
    /// <param name="zgsw"></param>
    /// <param name="xgse"></param>
    /// <param name="ygse"></param>
    /// <param name="zgse"></param>
    /// <param name="j"></param>
    void GSWGSE_08(T xgsw, T ygsw, T zgsw, out T xgse, out T ygse, out T zgse, int j);

    /// <summary>
    ///
    /// </summary>
    /// <param name="xgeo"></param>
    /// <param name="ygeo"></param>
    /// <param name="zgeo"></param>
    /// <param name="xmag"></param>
    /// <param name="ymag"></param>
    /// <param name="zmag"></param>
    /// <param name="j"></param>
    void GEOMAG_08(T xgeo, T ygeo, T zgeo, out T xmag, out T ymag, out T zmag, int j);

    /// <summary>
    ///
    /// </summary>
    /// <param name="xgei"></param>
    /// <param name="ygei"></param>
    /// <param name="zgei"></param>
    /// <param name="xgeo"></param>
    /// <param name="ygeo"></param>
    /// <param name="zgeo"></param>
    /// <param name="j"></param>
    void GEIGEO_08(T xgei, T ygei, T zgei, out T xgeo, out T ygeo, out T zgeo, int j);

    /// <summary>
    ///
    /// </summary>
    /// <param name="xmag"></param>
    /// <param name="ymag"></param>
    /// <param name="zmag"></param>
    /// <param name="xsm"></param>
    /// <param name="ysm"></param>
    /// <param name="zsm"></param>
    /// <param name="j"></param>
    void MAGSM_08(T xmag, T ymag, T zmag, out T xsm, out T ysm, out T zsm, int j);

    /// <summary>
    ///
    /// </summary>
    /// <param name="xsm"></param>
    /// <param name="ysm"></param>
    /// <param name="zsm"></param>
    /// <param name="xgsw"></param>
    /// <param name="ygsw"></param>
    /// <param name="zgsw"></param>
    /// <param name="j"></param>
    void SMGSW_08(T xsm, T ysm, T zsm, out T xgsw, out T ygsw, out T zgsw, int j);

    /// <summary>
    ///
    /// </summary>
    /// <param name="xgeo"></param>
    /// <param name="ygeo"></param>
    /// <param name="zgeo"></param>
    /// <param name="xgsw"></param>
    /// <param name="ygsw"></param>
    /// <param name="zgsw"></param>
    /// <param name="j"></param>
    void GEOGSW_08(T xgeo, T ygeo, T zgeo, out T xgsw, out T ygsw, out T zgsw, int j);

    /// <summary>
    ///
    /// </summary>
    /// <param name="h"></param>
    /// <param name="xmu"></param>
    /// <param name="r"></param>
    /// <param name="theta"></param>
    /// <param name="j"></param>
    void GEODGEO_08(T h, T xmu, out T r, out T theta, int j);

    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="r1"></param>
    /// <param name="r2"></param>
    /// <param name="r3"></param>
    /// <param name="iopt"></param>
    /// <param name="parmod"></param>
    /// <param name="exname"></param>
    /// <param name="inname"></param>
    void RHAND_08(T x, T y, T z, out T r1, out T r2, out T r3, int iopt, T[] parmod, string exname, string inname);

    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="ds"></param>
    /// <param name="dsmax"></param>
    /// <param name="errin"></param>
    /// <param name="iopt"></param>
    /// <param name="parmod"></param>
    /// <param name="exname"></param>
    /// <param name="inname"></param>
    void STEP_08(ref T x, ref T y, ref T z, ref T ds, T dsmax, T errin, int iopt, T[] parmod, string exname, string inname);

    /// <summary>
    ///
    /// </summary>
    /// <param name="xi"></param>
    /// <param name="yi"></param>
    /// <param name="zi"></param>
    /// <param name="dir"></param>
    /// <param name="dsmax"></param>
    /// <param name="err"></param>
    /// <param name="rlim"></param>
    /// <param name="r0"></param>
    /// <param name="iopt"></param>
    /// <param name="parmod"></param>
    /// <param name="exname"></param>
    /// <param name="inname"></param>
    /// <param name="xf"></param>
    /// <param name="yf"></param>
    /// <param name="zf"></param>
    /// <param name="xx"></param>
    /// <param name="yy"></param>
    /// <param name="zz"></param>
    /// <param name="l"></param>
    /// <param name="lmax"></param>
    void TRACE_08(T xi, T yi, T zi, T dir, T dsmax, T err, T rlim, T r0, int iopt, T[] parmod, string exname, string inname, out T xf, out T yf, out T zf, T[] xx, T[] yy, T[] zz, out int l, int lmax);

    /// <summary>
    ///
    /// </summary>
    /// <param name="xn_pd"></param>
    /// <param name="vel"></param>
    /// <param name="bzimf"></param>
    /// <param name="xgsw"></param>
    /// <param name="ygsw"></param>
    /// <param name="zgsw"></param>
    /// <param name="xmsnp"></param>
    /// <param name="ymsnp"></param>
    /// <param name="zmsnp"></param>
    /// <param name="dist"></param>
    /// <param name="id"></param>
    void SHUETAL_MGNP_08(T xn_pd, T vel, T bzimf, T xgsw, T ygsw, T zgsw, out T xmsnp, out T ymsnp, out T zmsnp, out T dist, out int id);

    /// <summary>
    ///
    /// </summary>
    /// <param name="xn_pd"></param>
    /// <param name="vel"></param>
    /// <param name="xgsw"></param>
    /// <param name="ygsw"></param>
    /// <param name="zgsw"></param>
    /// <param name="xmsnp"></param>
    /// <param name="ymsnp"></param>
    /// <param name="zmsnp"></param>
    /// <param name="dist"></param>
    /// <param name="id"></param>
    void T96Mgnp_08(T xn_pd, T vel, T xgsw, T ygsw, T zgsw, out T xmsnp, out T ymsnp, out T zmsnp, out T dist, out int id);
}
