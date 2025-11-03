namespace AuroraScienceHub.Geopack.Contracts.Models;

/// <summary>
/// Sun position parameters
/// </summary>
/// <param name="DateTime">Input UTC date and time</param>
/// <param name="Gst">Greenwich mean sidereal time</param>
/// <param name="Slong">Longitude along ecliptic</param>
/// <param name="Srasn">Right ascension></param>
/// <param name="Sdec">Declination of the Sun (radians)</param>
public readonly record struct Sun(DateTime DateTime, double Gst = 0, double Slong = 0, double Srasn = 0, double Sdec = 0);
