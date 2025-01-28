namespace AuroraScienceHub.Geopack.Geopack08;

public sealed partial class Geopack08
{
    public void SUN_08(DateTime dateTime, out float gst, out float slong, out float srasn, out float sdec)
    {
        const float rad = 180.0f / MathF.PI;

        if (dateTime.Year is < 1901 or > 2099)
        {
            gst = slong = srasn = sdec = 0;
            return;
        }

        float FDAY = (dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second) / 86400.0f;
        float DJ = 365 * (dateTime.Year - 1900) + (dateTime.Year - 1901) / 4.0f + dateTime.DayOfYear - 0.5f + FDAY;
        float T = DJ / 36525.0f;
        float VL = (279.696678f + 0.9856473354f * DJ) % 360.0f;
        gst = (279.690983f + 0.9856473354f * DJ + 360.0f * FDAY + 180.0f) % 360.0f / rad;
        float G = (358.475845f + 0.985600267f * DJ) % 360.0f / rad;
        slong = (VL + (1.91946f - 0.004789f * T) * MathF.Sin(G) + 0.020094f * MathF.Sin(2.0f * G)) / rad;
        if (slong > 6.2831853f) slong -= 6.2831853f;
        if (slong < 0.0) slong += 6.2831853f;
        float OBLIQ = (23.45229f - 0.0130125f * T) / rad;
        float SOB = MathF.Sin(OBLIQ);
        float SLP = slong - 9.924E-5F;
        float SIND = SOB * MathF.Sin(SLP);
        float COSD = MathF.Sqrt(1.0f - SIND * SIND);
        float SC = SIND / COSD;
        sdec = MathF.Atan(SC);
        srasn = MathF.PI - MathF.Atan2(MathF.Cos(OBLIQ) / SOB * SC, -MathF.Cos(SLP) / COSD);
    }
}
