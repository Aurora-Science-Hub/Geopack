using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.PhysicalObjects;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    public Sun Sun(DateTime dateTime)
    {
        if (dateTime.Year is < 1901 or > 2099)
        {
            return new Sun(dateTime);
        }

        double fday = (dateTime.Hour * GeopackConstants.SecondsPerHour + dateTime.Minute * 60 + dateTime.Second) / GeopackConstants.SecondsPerDay;
        //TODO Ask Tsyganenko if there 4 is really not 4.0D. Here we lose fraction
        double dj = 365 * (dateTime.Year - 1900) + (dateTime.Year - 1901) / 4 + dateTime.DayOfYear - 0.5D + fday;
        double t = dj / GeopackConstants.DaysPerJulianCentury;
        double vl = (279.696678D + 0.9856473354D * dj) % GeopackConstants.DegreesPerCircle;
        double gst = (279.690983D + 0.9856473354D * dj + GeopackConstants.DegreesPerCircle * fday + GeopackConstants.DegreesPerSemicircle) % GeopackConstants.DegreesPerCircle / GeopackConstants.Rad;
        double g = (358.475845D + 0.985600267D * dj) % GeopackConstants.DegreesPerCircle / GeopackConstants.Rad;
        double slong = (vl + (1.91946D - 0.004789D * t) * Math.Sin(g) + 0.020094D * Math.Sin(2.0D * g)) / GeopackConstants.Rad;

        if (slong > GeopackConstants.TwoPi)
        {
            slong -= GeopackConstants.TwoPi;
        }

        if (slong < -double.Epsilon)
        {
            slong += GeopackConstants.TwoPi;
        }

        double obliq = (GeopackConstants.EarthObliquityJ2000 - GeopackConstants.EarthObliquityRatePerCentury * t) / GeopackConstants.Rad;
        (double sob, double cob) = Math.SinCos(obliq);
        double slp = slong - 9.924e-5D;
        (double sinSlp, double cosSlp) = Math.SinCos(slp);
        double sind = sob * sinSlp;
        double cosd = Math.Sqrt(1.0D - sind * sind);
        double sc = sind / cosd;
        double sdec = Math.Atan2(sind, cosd);
        double srasn = GeopackConstants.Pi - Math.Atan2(cob / sob * sc, -cosSlp / cosd);

        return new Sun(dateTime, gst, slong, srasn, sdec);
    }
}
