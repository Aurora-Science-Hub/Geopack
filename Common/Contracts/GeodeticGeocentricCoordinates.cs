namespace Common.Contracts;

/// <summary>
/// Represents geodetic and geocentric coordinates of a point on Earth.
/// Geodetic coordinates (H, Xmu) account for the Earth's ellipsoidal shape (WGS84),
/// where height is measured from the ellipsoid surface, and latitude is the angle between
/// the ellipsoid normal and the equatorial plane.
/// Geocentric coordinates (R, Theta) are spherical coordinates relative to the Earth's center,
/// where distance is measured from the Earth's center, and colatitude is the angle from the rotation axis.
/// </summary>
/// <param name="H"> Height above the Earth's ellipsoid in kilometers (geodetic system) </param>
/// <param name="Xmu"> Geodetic latitude in radians (angle between the ellipsoid normal and equatorial plane) </param>
/// <param name="R"> Geocentric distance in kilometers (distance from Earth's center) </param>
/// <param name="Theta"> Geocentric colatitude in radians (angle from Earth's rotation axis, 0 at the North Pole) </param>
public record GeodeticGeocentricCoordinates(double H, double Xmu, double R, double Theta);
