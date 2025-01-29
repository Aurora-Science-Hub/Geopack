namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    public void SUN_08(DateTime dateTime, out double gst, out double slong, out double srasn, out double sdec)
    {


        if (dateTime.Year < 1901 || dateTime.Year > 2099)
        {
            gst = slong = srasn = sdec = 0;
            return;
        }

        double fday = (dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second) / 86400.0D;
        double dj = 365 * (dateTime.Year - 1900) + (dateTime.Year - 1901) / 4.0D + dateTime.DayOfYear - 0.5D + fday;
        double t = dj / 36525.0D;
        double vl = (279.696678D + 0.9856473354D * dj) % 360.0D;
        gst = (279.690983D + 0.9856473354D * dj + 360.0D * fday + 180.0D) % 360.0D / Rad;
        double g = (358.475845D + 0.985600267D * dj) % 360.0D / Rad;
        slong = (vl + (1.91946D - 0.004789D * t) * Math.Sin(g) + 0.020094D * Math.Sin(2.0D * g)) / Rad;

        if (slong > 6.2831853D)
        {
            slong -= 6.283185307D;
        }

        if (slong < 0.0D)
        {
            slong += 6.283185307D;
        }

        double obliq = (23.45229D - 0.0130125D * t) / Rad;
        double sob = Math.Sin(obliq);
        double slp = slong - 9.924e-5D;
        double sind = sob * Math.Sin(slp);
        double cosd = Math.Sqrt(1.0D - sind * sind);
        double sc = sind / cosd;
        sdec = Math.Atan2(sind, cosd);
        srasn = Pi - Math.Atan2(Math.Cos(obliq) / sob * sc, -Math.Cos(slp) / cosd);
    }
}
