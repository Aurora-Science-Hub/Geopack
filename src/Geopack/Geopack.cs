using Microsoft.Extensions.Logging;

namespace AuroraScienceHub.Geopack;

/// <summary>
/// Double precision original Geopack-2008
/// </summary>
internal sealed partial class Geopack : IGeopack
{
    private readonly ILogger<IGeopack> _logger;

    public Geopack(ILogger<IGeopack> logger)
    {
        _logger = logger;
    }
}
