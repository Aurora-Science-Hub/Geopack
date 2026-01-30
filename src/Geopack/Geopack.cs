using Microsoft.Extensions.Logging;

namespace AuroraScienceHub.Geopack;

/// <summary>
/// Double precision original Geopack-2008
/// </summary>
internal sealed partial class Geopack : IGeopack
{
    private readonly ILogger<Geopack> _logger;

    public Geopack(ILogger<Geopack> logger)
    {
        _logger = logger;
    }
}
