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

        double fday = (dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second) / 86400.0;
        double dj = 365 * (dateTime.Year - 1900) + (dateTime.Year - 1901) / 4 + dateTime.DayOfYear - 0.5 + fday;
        double t = dj / 36525.0;
        double vl = (279.696678 + 0.9856473354 * dj) % 360.0;
        double gst = (279.690983 + 0.9856473354 * dj + 360.0 * fday + 180.0) % 360.0 / GeopackConstants.Rad;
        double g = (358.475845 + 0.985600267 * dj) % 360.0 / GeopackConstants.Rad;

        (double sinG, double cosG) = Math.SinCos(g);
        double g2 = 2.0 * g;
        double slong = (vl + (1.91946 - 0.004789 * t) * sinG + 0.020094 * Math.Sin(g2)) / GeopackConstants.Rad;

        if (slong > 6.2831853)
        {
            slong -= 6.283185307;
        }

        if (slong < 0.0)
        {
            slong += 6.283185307;
        }

        double obliq = (23.45229 - 0.0130125 * t) / GeopackConstants.Rad;
        (double sinObliq, double cosObliq) = Math.SinCos(obliq);
        double slp = slong - 9.924e-5;
        (double sinSlp, double cosSlp) = Math.SinCos(slp);
        double sind = sinObliq * sinSlp;
        double cosd = Math.Sqrt(1.0 - sind * sind);
        double cosdInv = 1.0 / cosd;
        double sc = sind * cosdInv;
        double sdec = Math.Atan2(sind, cosd);
        double srasn = GeopackConstants.Pi - Math.Atan2(cosObliq / sinObliq * sc, -cosSlp * cosdInv);

        return new Sun(dateTime, gst, slong, srasn, sdec);
    }
}
