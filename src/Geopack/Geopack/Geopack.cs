using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Interfaces;

namespace AuroraScienceHub.Geopack.Geopack;

/// <summary>
/// Double precision original Geopack-2008
/// </summary>
public sealed partial class Geopack : IGeopack08
{
    private Common1 Common1 { get; set; } = new();

    private Common2 Common2 { get; set; } = new();

    public const double Pi = 3.141592654D;
    public const double TwoPi = 6.283185307D;
    public const double Rad = 57.295779513D;
}
