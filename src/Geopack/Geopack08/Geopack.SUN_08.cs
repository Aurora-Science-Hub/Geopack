namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    public void SUN_08(DateTime dateTime, out float gst, out float slong, out float srasn, out float sdec)
    {
        const double rad = 57.295779513;

        if (dateTime.Year < 1901 || dateTime.Year > 2099)
        {
            gst = slong = srasn = sdec = 0;
            return;
        }

        var FDAY = (dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second) / 86400.0;
        var DJ = 365 * (dateTime.Year - 1900) + (dateTime.Year - 1901) / 4 + dateTime.DayOfYear - 0.5 + FDAY;
        var T = DJ / 36525.0;
        var VL = (279.696678 + 0.9856473354 * DJ) % 360.0;
        gst = (float)((279.690983 + 0.9856473354 * DJ + 360.0 * FDAY + 180.0) % 360.0 / rad);
        var G = (358.475845 + 0.985600267 * DJ) % 360.0 / rad;
        slong = (float)((VL + (1.91946 - 0.004789 * T) * Math.Sin(G) + 0.020094 * Math.Sin(2.0 * G)) / rad);
        if (slong > 6.2831853) slong -= 6.2831853f;
        if (slong < 0.0) slong += 6.2831853f;
        var OBLIQ = (23.45229 - 0.0130125 * T) / rad;
        var SOB = Math.Sin(OBLIQ);
        var SLP = slong - 9.924E-5;
        var SIND = SOB * Math.Sin(SLP);
        var COSD = Math.Sqrt(1.0 - SIND * SIND);
        var SC = SIND / COSD;
        sdec = (float)Math.Atan(SC);
        srasn = (float)(3.141592654 - Math.Atan2(Math.Cos(OBLIQ) / SOB * SC, -Math.Cos(SLP) / COSD));
    }
}
