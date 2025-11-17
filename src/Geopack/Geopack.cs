using AuroraScienceHub.Geopack.Interfaces;

namespace AuroraScienceHub.Geopack;

/// <summary>
/// Double precision original Geopack-2008
/// </summary>
public sealed partial class Geopack : IGeopack
{
    private Common1 Common1 { get; set; } = new();

    private Common2 Common2 { get; set; } = new();

    private const double Pi = 3.141592654D;
    private const double TwoPi = 6.283185307D;
    private const double Rad = 57.295779513D;
}
