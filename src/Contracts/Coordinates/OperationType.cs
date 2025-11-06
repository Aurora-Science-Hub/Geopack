namespace AuroraScienceHub.Geopack.Contracts.Coordinates;

/// <summary>
/// Direct or reversed operations in Geopack methods.
/// </summary>
public enum OperationType
{
    /// <summary> Direct operation </summary>
    /// <remarks>
    /// If you use a Geopack method to convert from coordinate system A to B,
    /// the direct operation corresponds to the conversion from A to B.
    /// </remarks>
    Direct,

    /// <summary> Reversed operation </summary>
    /// <remarks>
    /// If you use a Geopack method to convert from coordinate system A to B,
    /// the reversed operation corresponds to the conversion from B to A.
    /// </remarks>
    Reversed
}
