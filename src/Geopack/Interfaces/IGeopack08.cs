using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Interfaces;

/// <summary>
/// Geopack-2008 Double Precision contracts
/// </summary>
public interface IGeopack08
{
    /// <summary>
    /// Calculates components of the main (internal) geomagnetic field in the geocentric solar-wind
    /// (GSW) coordinate system, using IAGA international geomagnetic reference model coefficients
    /// (e.g., https://www.ngdc.noaa.gov/IAGA/vmod/coeffs/igrf13coeffs.txt, revised 01 January, 2020)
    /// </summary>
    /// <remarks>
    /// The GSW system is essentially similar to the standard GSM (the two systems become identical
    /// to each other in the case of strictly anti-sunward solar wind flow). For a detailed
    /// definition, see introductory comments for the subroutine GSWGSE_08.
    /// Before the first call of this subroutine, or, if the date/time,
    /// or the solar wind velocity components (vgsex, vgsey, vgsez) have changed, the model coefficients
    /// and geo-gsw rotation matrix elements should be updated by calling the subroutine RECALC_08.
    /// </remarks>
    /// <param name="xgsw">Cartesian geocentric solar-wind x-coordinate (in units re=6371.2 km)</param>
    /// <param name="ygsw">Cartesian geocentric solar-wind y-coordinate (in units re=6371.2 km)</param>
    /// <param name="zgsw">Cartesian geocentric solar-wind z-coordinate (in units re=6371.2 km)</param>
    /// <param name="hxgsw">Cartesian geocentric solar-wind hx-component of the main geomagnetic field [nT]</param>
    /// <param name="hygsw">Cartesian geocentric solar-wind hy-component of the main geomagnetic field [nT]</param>
    /// <param name="hzgsw">Cartesian geocentric solar-wind hz-component of the main geomagnetic field [nT]</param>
    void IGRF_GSW_08(
        float xgsw, float ygsw, float zgsw,
        out float hxgsw, out float hygsw, out float hzgsw);

    /// <summary>
    /// Calculates components of the main (internal) geomagnetic field in the spherical geographic
    /// (geocentric) coordinate system, using IAGA international geomagnetic reference model
    /// coefficients (e.g., http://www.ngdc.noaa.gov/IAGA/vmod/igrf.html, revised: 22 March, 2005)
    /// </summary>
    /// <remarks>
    /// Before the first call of this subroutine, or if the date was changed,
    /// the model coefficients should be updated by calling the subroutine RECALC_08
    /// </remarks>
    /// <param name="r">Spherical geographic (geocentric) coordinates: radial distance r in units re=6371.2 km</param>
    /// <param name="theta">Colatitude theta in radians</param>
    /// <param name="phi">Longitude phi in radians</param>
    /// <param name="br">Spherical components of the main geomagnetic field in nanotesla (positive br outward)</param>
    /// <param name="btheta">Spherical components of the main geomagnetic field in nanotesla (btheta southward)</param>
    /// <param name="bphi">Spherical components of the main geomagnetic field in nanotesla (bphi eastward)</param>
    void IGRF_GEO_08(
        float r, float theta, float phi,
        out float br, out float btheta, out float bphi);

    /// <summary>
    /// Calculates GSW (geocentric solar-wind) components of geodipole field with the dipole moment
    /// corresponding to the epoch, specified by calling subroutine RECALC_08 (should be
    /// invoked before the first use of this one, or if the date/time, and/or the observed
    /// solar wind direction, have changed.
    /// </summary>
    /// <remarks>
    /// The GSW coordinate system is essentially similar to the standard GSM (the two systems become
    /// identical to each other in the case of strictly radial anti-sunward solar wind flow). Its
    /// detailed definition is given in introductory comments for the subroutine GSWGSE_08.
    /// </remarks>
    /// <param name="xgsw">GSW coordinates in re (1 re = 6371.2 km)</param>
    /// <param name="ygsw">GSW coordinates in re (1 re = 6371.2 km)</param>
    /// <param name="zgsw">GSW coordinates in re (1 re = 6371.2 km)</param>
    /// <param name="bxgsw">Field components in GSW system, in nanotesla</param>
    /// <param name="bygsw">Field components in GSW system, in nanotesla</param>
    /// <param name="bzgsw">Field components in GSW system, in nanotesla</param>
    void DIP_08(
        float xgsw, float ygsw, float zgsw,
        out float bxgsw, out float bygsw, out float bzgsw);

    /// <summary>
    /// Calculates four quantities necessary for coordinate transformations
    /// which depend on sun position (and, hence, on universal time and season)
    /// </summary>
    /// <param name="dateTime">Year, day, and universal time in hours, minutes, and seconds</param>
    /// <remarks>
    /// Original: SUN_08
    /// </remarks>
    Sun Sun(DateTime dateTime);

    /// <summary>
    /// Converts spherical coordinates into Cartesian ones.
    /// </summary>
    /// <param name="r">Radial distance (Earth radii, Re)</param>
    /// <param name="theta">Co-latitude theta in radians</param>
    /// <param name="phi">Longitude phi in radians</param>
    Point SphCar(double r, double theta, double phi);

    /// <summary>
    /// Converts Cartesian into spherical coordinates.
    /// </summary>
    /// <param name="x">Cartesian x-coordinate</param>
    /// <param name="y">Cartesian y-coordinate</param>
    /// <param name="z">Cartesian z-coordinate</param>
    /// <remarks>
    /// Theta and Phi in radians, R in Earth radii.
    /// At the poles (x=0 and y=0), phi is assumed to be 0.
    /// </remarks>
    Point CarSph(double x, double y, double z);

    /// <summary>
    /// Calculates Cartesian field components from local spherical ones.
    /// </summary>
    /// <param name="theta">Spherical angle theta of the point in radians</param>
    /// <param name="phi">Spherical angle phi of the point in radians</param>
    /// <param name="br">Local spherical component of the field (radial)</param>
    /// <param name="btheta">Local spherical component of the field (co-latitude)</param>
    /// <param name="bphi">Local spherical component of the field (longitude)</param>
    /// <param name="bx">Cartesian component of the field (x)</param>
    /// <param name="by">Cartesian component of the field (y)</param>
    /// <param name="bz">Cartesian component of the field (z)</param>
    void BSPCAR_08(
        double theta, double phi, double br, double btheta, double bphi,
        out double bx, out double by, out double bz);

    /// <summary>
    /// Calculates local spherical field components from those in Cartesian system.
    /// </summary>
    /// <param name="x">Cartesian component of the position vector (x)</param>
    /// <param name="y">Cartesian component of the position vector (y)</param>
    /// <param name="z">Cartesian component of the position vector (z)</param>
    /// <param name="bx">Cartesian component of the field vector (bx)</param>
    /// <param name="by">Cartesian component of the field vector (by)</param>
    /// <param name="bz">Cartesian component of the field vector (bz)</param>
    /// <param name="br">Local spherical component of the field vector (radial)</param>
    /// <param name="btheta">Local spherical component of the field vector (co-latitude)</param>
    /// <param name="bphi">Local spherical component of the field vector (longitude)</param>
    void BCARSP_08(
        float x, float y, float z,
        float bx, float by, float bz,
        out float br, out float btheta, out float bphi);

    /// <summary>
    /// Prepares elements of rotation matrices for transformations of vectors between several coordinate systems,
    /// most frequently used in space physics. Also prepares coefficients used in the calculation of the main geomagnetic field (IGRF model).
    /// </summary>
    /// <remarks>
    /// This subroutine should be invoked before using the following subroutines:
    /// IGRF_GEO_08, IGRF_GSW_08, DIP_08, GEOMAG_08, GEOGSW_08, MAGSW_08, SMGSW_08, GSWGSE_08,
    /// GEIGEO_08, TRACE_08, STEP_08, RHAND_08.
    /// There is no need to repeatedly invoke RECALC_08 if multiple calculations are made for the same date/time and solar wind flow direction.
    /// </remarks>
    /// <param name="dateTime">Date and time in UTC</param>
    /// <param name="vgsex">GSE (geocentric solar-ecliptic) component of the observed solar wind flow velocity (in km/s)</param>
    /// <param name="vgsey">GSE (geocentric solar-ecliptic) component of the observed solar wind flow velocity (in km/s)</param>
    /// <param name="vgsez">GSE (geocentric solar-ecliptic) component of the observed solar wind flow velocity (in km/s)</param>
    (Common1, Common2) RECALC_08(DateTime dateTime, double vgsex, double vgsey, double vgsez);

    /// <summary>
    /// Transforms components of geocentric solar-wind (GSW) system to GSE coordinate.
    /// </summary>
    /// <remarks>
    /// In the GSW system, the X axis is antiparallel to the observed direction of the solar wind flow.
    /// The Y and Z axes are defined similarly to the standard GSM system.
    /// The GSW system becomes identical to the standard GSM in the case of a strictly radial solar wind flow.
    /// Before calling GSWGSE_08, be sure to invoke the subroutine RECALC_08 to define all necessary elements of transformation matrices.
    /// </remarks>
    /// <param name="xgsw">GSW x-coordinate</param>
    /// <param name="ygsw">GSW y-coordinate</param>
    /// <param name="zgsw">GSW z-coordinate</param>
    /// <param name="xgse">GSE x-coordinate</param>
    /// <param name="ygse">GSE y-coordinate</param>
    /// <param name="zgse">GSE z-coordinate</param>
    void GSWGSE_08(
        float xgsw, float ygsw, float zgsw,
        out float xgse, out float ygse, out float zgse);

    /// <summary>
    /// Transforms GSE coordinate components to geocentric solar-wind (GSW) ones.
    /// </summary>
    /// <remarks>
    /// In the GSW system, the X axis is antiparallel to the observed direction of the solar wind flow.
    /// The Y and Z axes are defined similarly to the standard GSM system.
    /// The GSW system becomes identical to the standard GSM in the case of a strictly radial solar wind flow.
    /// Before calling GSEGSW_08, be sure to invoke the subroutine RECALC_08 to define all necessary elements of transformation matrices.
    /// </remarks>
    /// <param name="xgse">GSE x-coordinate</param>
    /// <param name="ygse">GSE y-coordinate</param>
    /// <param name="zgse">GSE z-coordinate</param>
    /// <param name="xgsw">GSW x-coordinate</param>
    /// <param name="ygsw">GSW y-coordinate</param>
    /// <param name="zgsw">GSW z-coordinate</param>
    void GSEGSW_08(
         float xgse, float ygse, float zgse,
         out float xgsw, out float ygsw, out float zgsw);

    /// <summary>
    /// Converts geographic (GEO) to dipole (MAG) coordinates.
    /// </summary>
    /// <remarks>
    /// Before calling GEOMAG_08, be sure to invoke the subroutine RECALC_08 in two cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have been changed.
    /// No information is required here on the solar wind velocity, so one can set VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0 in RECALC_08.
    /// </remarks>
    /// <param name="xgeo">GEO x-coordinate</param>
    /// <param name="ygeo">GEO y-coordinate</param>
    /// <param name="zgeo">GEO z-coordinate</param>
    /// <param name="xmag">MAG x-coordinate</param>
    /// <param name="ymag">MAG y-coordinate</param>
    /// <param name="zmag">MAG z-coordinate</param>
    void GEOMAG_08(
        float xgeo, float ygeo, float zgeo,
        out float xmag, out float ymag, out float zmag);

    /// <summary>
    /// Converts dipole (MAG) coordinates to geographic (GEO).
    /// </summary>
    /// <remarks>
    /// Before calling MAGGEO_08, be sure to invoke the subroutine RECALC_08 in two cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have been changed.
    /// No information is required here on the solar wind velocity, so one can set VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0 in RECALC_08.
    /// </remarks>
    /// <param name="xmag">MAG x-coordinate</param>
    /// <param name="ymag">MAG y-coordinate</param>
    /// <param name="zmag">MAG z-coordinate</param>
    /// <param name="xgeo">GEO x-coordinate</param>
    /// <param name="ygeo">GEO y-coordinate</param>
    /// <param name="zgeo">GEO z-coordinate</param>
    void MAGGEO_08(
        float xmag, float ymag, float zmag,
        out float xgeo, out float ygeo, out float zgeo);

    /// <summary>
    /// Converts equatorial inertial (GEI) to geographical (GEO) coordinates.
    /// </summary>
    /// <remarks>
    /// Before calling GEIGEO_08, be sure to invoke the subroutine RECALC_08 in two cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the current values date/time have been changed.
    /// No information is required here on the solar wind velocity, so one can set VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0 in RECALC_08.
    /// </remarks>
    /// <param name="xgei">GEI x-coordinate</param>
    /// <param name="ygei">GEI y-coordinate</param>
    /// <param name="zgei">GEI z-coordinate</param>
    /// <param name="xgeo">GEO x-coordinate</param>
    /// <param name="ygeo">GEO y-coordinate</param>
    /// <param name="zgeo">GEO z-coordinate</param>
    void GEIGEO_08(
        float xgei, float ygei, float zgei,
        out float xgeo, out float ygeo, out float zgeo);

    /// <summary>
    /// Converts geographical (GEO) coordinates to equatorial inertial (GEI).
    /// </summary>
    /// <remarks>
    /// Before calling GEOGEI_08, be sure to invoke the subroutine RECALC_08 in two cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the current values date/time have been changed.
    /// No information is required here on the solar wind velocity, so one can set VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0 in RECALC_08.
    /// </remarks>
    /// <param name="xgeo">GEO x-coordinate</param>
    /// <param name="ygeo">GEO y-coordinate</param>
    /// <param name="zgeo">GEO z-coordinate</param>
    /// <param name="xgei">GEI x-coordinate</param>
    /// <param name="ygei">GEI y-coordinate</param>
    /// <param name="zgei">GEI z-coordinate</param>
    void GEOGEI_08(
        float xgeo, float ygeo, float zgeo,
        out float xgei, out float ygei, out float zgei);

    /// <summary>
    /// Converts dipole (MAG) to solar magnetic (SM) coordinates.
    /// </summary>
    /// <remarks>
    /// Before calling MAGSM_08, be sure to invoke the subroutine RECALC_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// A non-standard definition is implied here for the solar magnetic coordinate system:
    /// it is assumed that the XSM axis lies in the plane defined by the geodipole axis and the observed vector of the solar wind flow
    /// (rather than the Earth-Sun line). In order to convert MAG coordinates to and from the standard SM coordinates,
    /// invoke RECALC_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xmag">MAG x-coordinate</param>
    /// <param name="ymag">MAG y-coordinate</param>
    /// <param name="zmag">MAG z-coordinate</param>
    /// <param name="xsm">SM x-coordinate</param>
    /// <param name="ysm">SM y-coordinate</param>
    /// <param name="zsm">SM z-coordinate</param>
    void MAGSM_08(
        float xmag, float ymag, float zmag,
        out float xsm, out float ysm, out float zsm);

    /// <summary>
    /// Converts solar magnetic (SM) to dipole (MAG) coordinates.
    /// </summary>
    /// <remarks>
    /// Before calling SMMAG_08, be sure to invoke the subroutine RECALC_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// A non-standard definition is implied here for the solar magnetic coordinate system:
    /// it is assumed that the XSM axis lies in the plane defined by the geodipole axis and the observed vector of the solar wind flow
    /// (rather than the Earth-Sun line). In order to convert MAG coordinates to and from the standard SM coordinates,
    /// invoke RECALC_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xsm">SM x-coordinate</param>
    /// <param name="ysm">SM y-coordinate</param>
    /// <param name="zsm">SM z-coordinate</param>
    /// <param name="xmag">MAG x-coordinate</param>
    /// <param name="ymag">MAG y-coordinate</param>
    /// <param name="zmag">MAG z-coordinate</param>
    void SMMAG_08(
        float xsm, float ysm, float zsm,
        out float xmag, out float ymag, out float zmag);

    /// <summary>
    /// Converts solar magnetic (SM) to geocentric solar-wind (GSW) coordinates.
    /// </summary>
    /// <remarks>
    /// Before calling SMGSW_08, be sure to invoke the subroutine RECALC_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have been changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// A non-standard definition is implied here for the solar magnetic (SM) coordinate system:
    /// it is assumed that the XSM axis lies in the plane defined by the geodipole axis and the observed vector of the solar wind flow
    /// (rather than the Earth-Sun line). In order to convert MAG coordinates to and from the standard SM coordinates,
    /// invoke RECALC_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xsm">SM x-coordinate</param>
    /// <param name="ysm">SM y-coordinate</param>
    /// <param name="zsm">SM z-coordinate</param>
    /// <param name="xgsw">GSW x-coordinate</param>
    /// <param name="ygsw">GSW y-coordinate</param>
    /// <param name="zgsw">GSW z-coordinate</param>
    void SMGSW_08(
        float xsm, float ysm, float zsm,
        out float xgsw, out float ygsw, out float zgsw);

    /// <summary>
    /// Converts geocentric solar-wind (GSW) to solar magnetic (SM) coordinates.
    /// </summary>
    /// <remarks>
    /// Before calling GSWSM_08, be sure to invoke the subroutine RECALC_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have been changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// A non-standard definition is implied here for the solar magnetic (SM) coordinate system:
    /// it is assumed that the XSM axis lies in the plane defined by the geodipole axis and the observed vector of the solar wind flow
    /// (rather than the Earth-Sun line). In order to convert MAG coordinates to and from the standard SM coordinates,
    /// invoke RECALC_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xgsw">GSW x-coordinate</param>
    /// <param name="ygsw">GSW y-coordinate</param>
    /// <param name="zgsw">GSW z-coordinate</param>
    /// <param name="xsm">SM x-coordinate</param>
    /// <param name="ysm">SM y-coordinate</param>
    /// <param name="zsm">SM z-coordinate</param>
    void GSWSM_08(
        float xgsw, float ygsw, float zgsw,
        out float xsm, out float ysm, out float zsm);

    /// <summary>
    /// Converts geographic (GEO) to geocentric solar-wind (GSW) coordinates.
    /// </summary>
    /// <remarks>
    /// Before calling GEOGSW_08, be sure to invoke the subroutine RECALC_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// This subroutine converts GEO vectors to and from the solar-wind GSW coordinate system,
    /// taking into account possible deflections of the solar wind direction from strictly radial.
    /// Before converting to/from standard GSM coordinates, invoke RECALC_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xgeo">GEO x-coordinate</param>
    /// <param name="ygeo">GEO y-coordinate</param>
    /// <param name="zgeo">GEO z-coordinate</param>
    /// <param name="xgsw">GSW x-coordinate</param>
    /// <param name="ygsw">GSW y-coordinate</param>
    /// <param name="zgsw">GSW z-coordinate</param>
    void GEOGSW_08(
        float xgeo, float ygeo, float zgeo,
        out float xgsw, out float ygsw, out float zgsw);

    /// <summary>
    /// Converts geocentric solar-wind (GSW) to geographic (GEO) coordinates.
    /// </summary>
    /// <remarks>
    /// Before calling GSWGEO_08, be sure to invoke the subroutine RECALC_08 in three cases:
    /// 1. Before the first transformation of coordinates.
    /// 2. If the values of date/time have changed.
    /// 3. If the values of components of the solar wind flow velocity have changed.
    /// This subroutine converts GEO vectors to and from the solar-wind GSW coordinate system,
    /// taking into account possible deflections of the solar wind direction from strictly radial.
    /// Before converting to/from standard GSM coordinates, invoke RECALC_08 with VGSEX=-400.0, VGSEY=0.0, VGSEZ=0.0.
    /// </remarks>
    /// <param name="xgsw">GSW x-coordinate</param>
    /// <param name="ygsw">GSW y-coordinate</param>
    /// <param name="zgsw">GSW z-coordinate</param>
    /// <param name="xgeo">GEO x-coordinate</param>
    /// <param name="ygeo">GEO y-coordinate</param>
    /// <param name="zgeo">GEO z-coordinate</param>
    void GSWGEO_08(
        float xgsw, float ygsw, float zgsw,
        out float xgeo, out float ygeo, out float zgeo);

    /// <summary>
    /// Converts vertical local height (altitude) H and geodetic latitude XMU into geocentric coordinates R and THETA.
    /// </summary>
    /// <remarks>
    /// The subroutine uses World Geodetic System WGS84 parameters for the Earth's ellipsoid. The angular quantities
    /// (geo co-latitude THETA and geodetic latitude XMU) are in radians, and the distances (geocentric radius R and
    /// altitude H above the Earth's ellipsoid) are in kilometers.
    /// </remarks>
    /// <param name="h">Altitude (km) for geodetic input or geocentric radius (km) for geocentric input</param>
    /// <param name="xmu">Geodetic latitude (radians) for geodetic input or co-latitude (radians) for geocentric input</param>
    /// <param name="r">Geocentric radius (km) for geocentric output or altitude (km) for geodetic output</param>
    /// <param name="theta">Co-latitude (radians) for geocentric output or geodetic latitude (radians) for geodetic output</param>
    void GEODGEO_08(
        float h, float xmu,
        out float r, out float theta);

    /// <summary>
    /// Converts geocentric coordinates R and THETA into vertical local height (altitude) H and geodetic latitude XMU.
    /// </summary>
    /// <remarks>
    /// The subroutine uses World Geodetic System WGS84 parameters for the Earth's ellipsoid. The angular quantities
    /// (geo co-latitude THETA and geodetic latitude XMU) are in radians, and the distances (geocentric radius R and
    /// altitude H above the Earth's ellipsoid) are in kilometers.
    /// </remarks>
    /// <param name="h">Altitude (km) for geodetic input or geocentric radius (km) for geocentric input</param>
    /// <param name="xmu">Geodetic latitude (radians) for geodetic input or co-latitude (radians) for geocentric input</param>
    /// <param name="r">Geocentric radius (km) for geocentric output or altitude (km) for geodetic output</param>
    /// <param name="theta">Co-latitude (radians) for geocentric output or geodetic latitude (radians) for geodetic output</param>
    void GEOGEOD_08(
        float r, float theta,
        out float h, out float xmu);

    /// <summary>
    /// Calculates the components of the right-hand side vector in the geomagnetic field line equation.
    /// </summary>
    /// <remarks>
    /// This is a subsidiary subroutine for the subroutine STEP_08.
    /// </remarks>
    /// <param name="x">X-coordinate</param>
    /// <param name="y">Y-coordinate</param>
    /// <param name="z">Z-coordinate</param>
    /// <param name="r1">Output component R1</param>
    /// <param name="r2">Output component R2</param>
    /// <param name="r3">Output component R3</param>
    /// <param name="iopt">Option flag</param>
    /// <param name="parmod">Array of parameters (dimension 10)</param>
    /// <param name="exname">Name of the subroutine for the external part of the total field</param>
    /// <param name="inname">Name of the subroutine for the internal part of the total field</param>
    void RHAND_08(
        float x, float y, float z,
        int iopt,
        float[] parmod,
        string exname,
        string inname,
        out float r1, out float r2, out float r3);

    /// <summary>
    /// Re-calculates the input values {X, Y, Z} (in GSW coordinates) for any point on a field line,
    /// by making a step along that line using the Runge-Kutta-Merson algorithm.
    /// </summary>
    /// <remarks>
    /// DS is a prescribed value of the current step size, DSMAX is its upper limit.
    /// ERRIN is a permissible error (its optimal value specified in the subroutine TRACE_08).
    /// If the actual error (ERRCUR) at the current step is larger than ERRIN, the step is rejected,
    /// and the calculation is repeated anew with halved step size DS.
    /// If ERRCUR is smaller than ERRIN, the step is accepted, and the current value of DS is retained
    /// for the next step.
    /// If ERRCUR is smaller than 0.04 * ERRIN, the step is accepted, and the value of DS for the next step
    /// is increased by the factor 1.5, but not larger than DSMAX.
    /// IOPT is a flag, reserved for specifying a version of the external field model EXNAME.
    /// Array PARMOD(10) contains input parameters for the model EXNAME.
    /// EXNAME is the name of the subroutine for the external field model.
    /// INNAME is the name of the subroutine for the internal field model (either DIP_08 or IGRF_GSW_08).
    /// All the above parameters are input ones; output is the recalculated values of X, Y, Z.
    /// </remarks>
    /// <param name="x">X-coordinate</param>
    /// <param name="y">Y-coordinate</param>
    /// <param name="z">Z-coordinate</param>
    /// <param name="ds">Current step size</param>
    /// <param name="dsmax">Upper limit of step size</param>
    /// <param name="errin">Permissible error</param>
    /// <param name="iopt">Flag for specifying a version of the external field model</param>
    /// <param name="parmod">Array of parameters (dimension 10)</param>
    /// <param name="exname">Name of the subroutine for the external field model</param>
    /// <param name="inname">Name of the subroutine for the internal field model</param>
    void STEP_08(
        float ds, float dsmax,
        float errin,
        int iopt,
        float[] parmod,
        string exname,
        string inname,
        out float x, out float y, out float z);

    /// <summary>
    /// Traces a field line from an arbitrary point in space to the Earth's surface or to a model limiting boundary.
    /// </summary>
    /// <remarks>
    /// This subroutine allows two options:
    /// 1. If INNAME=IGRF_GSW_08, then the IGRF model will be used for calculating contribution from Earth's internal sources.
    ///    In this case, subroutine RECALC_08 must be called before using TRACE_08, with properly specified date, universal time,
    ///    and solar wind velocity components, to calculate in advance all quantities needed for the main field model and for
    ///    transformations between involved coordinate systems.
    /// 2. If INNAME=DIP_08, then a pure dipole field will be used instead of the IGRF model. In this case, the subroutine RECALC_08
    ///    must also be called before TRACE_08. Here one can choose either to (a) calculate dipole tilt angle based on date, time,
    ///    and solar wind direction, or (b) explicitly specify that angle, without any reference to date/UT/solar wind. In the last
    ///    case (b), the sine (SPS) and cosine (CPS) of the dipole tilt angle must be specified in advance (but after having called
    ///    RECALC_08) and forwarded in the common block /GEOPACK1/ (in its 11th and 12th elements, respectively). In this case the
    ///    role of the subroutine RECALC_08 is reduced to only calculating the components of the Earth's dipole moment.
    /// </remarks>
    /// <param name="xi">GSW x-coordinate of the field line starting point (in Earth radii, 1 RE = 6371.2 km)</param>
    /// <param name="yi">GSW y-coordinate of the field line starting point (in Earth radii, 1 RE = 6371.2 km)</param>
    /// <param name="zi">GSW z-coordinate of the field line starting point (in Earth radii, 1 RE = 6371.2 km)</param>
    /// <param name="dsmax">Upper limit on the step size (sets a desired maximal spacing between the field line points)</param>
    /// <param name="err">Permissible step error</param>
    /// <param name="rlim">Radius of a sphere (in RE), defining the outer boundary of the tracing region</param>
    /// <param name="r0">Radius of a sphere (in RE), defining the inner boundary of the tracing region</param>
    /// <param name="iopt">A model index; can be used for specifying a version of the external field model</param>
    /// <param name="parmod">A 10-element array containing input parameters needed for a unique specification of the external field model</param>
    /// <param name="exname">Name of a subroutine providing components of the external magnetic field</param>
    /// <param name="inname">Name of a subroutine providing components of the internal magnetic field</param>
    /// <param name="xf">GSW x-coordinate of the endpoint of the traced field line</param>
    /// <param name="yf">GSW y-coordinate of the endpoint of the traced field line</param>
    /// <param name="zf">GSW z-coordinate of the endpoint of the traced field line</param>
    /// <param name="xx">Array of length lmax, containing x-coordinates of the field line points</param>
    /// <param name="yy">Array of length lmax, containing y-coordinates of the field line points</param>
    /// <param name="zz">Array of length lmax, containing z-coordinates of the field line points</param>
    /// <param name="l">Actual number of field line points, generated by this subroutine</param>
    /// <param name="lmax">Maximal length of the arrays xx, yy, zz</param>
    /// <remarks> Trace from Norther to Southern Hemisphere (anti-parallel to Earth's magnetic field vector).</remarks>
    void TRACE_NS_08(
        float xi, float yi, float zi,
        float dsmax,
        float err,
        float rlim,
        float r0,
        int iopt,
        float[] parmod,
        string exname,
        string inname,
        int lmax,
        out float[] xx, out float[] yy, out float[] zz,
        out float xf, out float yf, out float zf,
        out int l);

    /// <summary>
    /// Traces a field line from an arbitrary point in space to the Earth's surface or to a model limiting boundary.
    /// </summary>
    /// <remarks>
    /// This subroutine allows two options:
    /// 1. If INNAME=IGRF_GSW_08, then the IGRF model will be used for calculating contribution from Earth's internal sources.
    ///    In this case, subroutine RECALC_08 must be called before using TRACE_08, with properly specified date, universal time,
    ///    and solar wind velocity components, to calculate in advance all quantities needed for the main field model and for
    ///    transformations between involved coordinate systems.
    /// 2. If INNAME=DIP_08, then a pure dipole field will be used instead of the IGRF model. In this case, the subroutine RECALC_08
    ///    must also be called before TRACE_08. Here one can choose either to (a) calculate dipole tilt angle based on date, time,
    ///    and solar wind direction, or (b) explicitly specify that angle, without any reference to date/UT/solar wind. In the last
    ///    case (b), the sine (SPS) and cosine (CPS) of the dipole tilt angle must be specified in advance (but after having called
    ///    RECALC_08) and forwarded in the common block /GEOPACK1/ (in its 11th and 12th elements, respectively). In this case the
    ///    role of the subroutine RECALC_08 is reduced to only calculating the components of the Earth's dipole moment.
    /// </remarks>
    /// <param name="xi">GSW x-coordinate of the field line starting point (in Earth radii, 1 RE = 6371.2 km)</param>
    /// <param name="yi">GSW y-coordinate of the field line starting point (in Earth radii, 1 RE = 6371.2 km)</param>
    /// <param name="zi">GSW z-coordinate of the field line starting point (in Earth radii, 1 RE = 6371.2 km)</param>
    /// <param name="dsmax">Upper limit on the step size (sets a desired maximal spacing between the field line points)</param>
    /// <param name="err">Permissible step error</param>
    /// <param name="rlim">Radius of a sphere (in RE), defining the outer boundary of the tracing region</param>
    /// <param name="r0">Radius of a sphere (in RE), defining the inner boundary of the tracing region</param>
    /// <param name="iopt">A model index; can be used for specifying a version of the external field model</param>
    /// <param name="parmod">A 10-element array containing input parameters needed for a unique specification of the external field model</param>
    /// <param name="exname">Name of a subroutine providing components of the external magnetic field</param>
    /// <param name="inname">Name of a subroutine providing components of the internal magnetic field</param>
    /// <param name="xf">GSW x-coordinate of the endpoint of the traced field line</param>
    /// <param name="yf">GSW y-coordinate of the endpoint of the traced field line</param>
    /// <param name="zf">GSW z-coordinate of the endpoint of the traced field line</param>
    /// <param name="xx">Array of length lmax, containing x-coordinates of the field line points</param>
    /// <param name="yy">Array of length lmax, containing y-coordinates of the field line points</param>
    /// <param name="zz">Array of length lmax, containing z-coordinates of the field line points</param>
    /// <param name="l">Actual number of field line points, generated by this subroutine</param>
    /// <param name="lmax">Maximal length of the arrays xx, yy, zz</param>
    /// <remarks> Trace from Southern to Northern Hemisphere (parallel to Earth's magnetic field vector).</remarks>
    void TRACE_SN_08(
        float xi, float yi, float zi,
        float dsmax,
        float err,
        float rlim,
        float r0,
        int iopt,
        float[] parmod,
        string exname,
        string inname,
        int lmax,
        out float[] xx, out float[] yy, out float[] zz,
        out float xf, out float yf, out float zf,
        out int l);

    /// <summary>
    /// For any point in space with coordinates (XGSW, YGSW, ZGSW) and specified conditions
    /// in the incoming solar wind, this subroutine:
    /// 1. Determines if the point (XGSW, YGSW, ZGSW) lies inside or outside the
    ///    model magnetopause of Shue et al. (JGR-A, V.103, P. 17691, 1998).
    /// 2. Calculates the GSW position of a point {XMGNP, YMGNP, ZMGNP}, lying at the model
    ///    magnetopause and asymptotically tending to the nearest boundary point with
    ///    respect to the observation point {XGSW, YGSW, ZGSW}, as it approaches the magnetopause.
    /// </summary>
    /// <param name="xn_pd">
    /// Either solar wind proton number density (per c.c.) (if VEL > 0)
    /// or the solar wind ram pressure in nanopascals (if VEL < 0)
    /// </param>
    /// <param name="vel">
    /// Either solar wind velocity (km/sec)
    /// or any negative number, which indicates that XN_PD stands
    /// for the solar wind pressure, rather than for the density
    /// </param>
    /// <param name="bzimf">IMF BZ in nanoteslas</param>
    /// <param name="xgsw">GSW x-coordinate of the observation point in Earth radii</param>
    /// <param name="ygsw">GSW y-coordinate of the observation point in Earth radii</param>
    /// <param name="zgsw">GSW z-coordinate of the observation point in Earth radii</param>
    /// <param name="xmsnp">GSW x-coordinate of the boundary point</param>
    /// <param name="ymsnp">GSW y-coordinate of the boundary point</param>
    /// <param name="zmsnp">GSW z-coordinate of the boundary point</param>
    /// <param name="dist">Distance (in RE) between the observation point (XGSW, YGSW, ZGSW) and the model magnetopause</param>
    /// <param name="id">
    /// Position flag: ID=+1 (-1) means that the observation point
    /// lies inside (outside) of the model magnetopause, respectively.
    /// </param>
    void SHUETAL_MGNP_08(
        float xn_pd, float vel, float bzimf,
        float xgsw, float ygsw, float zgsw,
        out float xmsnp, out float ymsnp, out float zmsnp,
        out float dist, out int id);

    /// <summary>
    /// For any point in space with given coordinates (XGSW, YGSW, ZGSW), this subroutine defines
    /// the position of a point (XMGNP, YMGNP, ZMGNP) at the T96 model magnetopause with the
    /// same value of the ellipsoidal tau-coordinate, and the distance between them.
    /// </summary>
    /// <remarks>
    /// This is not the shortest distance D_MIN to the boundary, but DIST asymptotically tends to D_MIN,
    /// as the observation point gets closer to the magnetopause.
    /// </remarks>
    /// <param name="xn_pd">
    /// Either solar wind proton number density (per c.c.) (if VEL > 0)
    /// or the solar wind ram pressure in nanopascals (if VEL < 0)
    /// </param>
    /// <param name="vel">
    /// Either solar wind velocity (km/sec)
    /// or any negative number, which indicates that XN_PD stands
    /// for the solar wind pressure, rather than for the density
    /// </param>
    /// <param name="xgsw">Coordinates of the observation point in Earth radii</param>
    /// <param name="ygsw">Coordinates of the observation point in Earth radii</param>
    /// <param name="zgsw">Coordinates of the observation point in Earth radii</param>
    /// <param name="xmsnp">GSW position of the boundary point, having the same value of tau-coordinate as the observation point</param>
    /// <param name="ymsnp">GSW position of the boundary point, having the same value of tau-coordinate as the observation point</param>
    /// <param name="zmsnp">GSW position of the boundary point, having the same value of tau-coordinate as the observation point</param>
    /// <param name="dist">The distance between the two points, in RE</param>
    /// <param name="id">
    /// Position flag: ID=+1 (-1) means that the point (XGSW, YGSW, ZGSW)
    /// lies inside (outside) the model magnetopause, respectively.
    /// </param>
    void T96_MGNP_08(
        float xn_pd, float vel,
        float xgsw, float ygsw, float zgsw,
        out float xmsnp, out float ymsnp, out float zmsnp,
        out float dist, out int id);
}
