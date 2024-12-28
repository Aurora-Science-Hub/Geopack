namespace UnitTests.Geopack2008.TestData;

public sealed class TestData
{
    /// <summary>
    /// Дата и время
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// X-component of the solar wind velocity
    /// </summary>
    public float VGSEX { get; set; }

    /// <summary>
    /// Y-component of the solar wind velocity
    /// </summary>
    public float VGSEY { get; set; }

    /// <summary>
    /// Z-component of the solar wind velocity
    /// </summary>
    public float VGSEZ { get; set; }

    /// <summary>
    /// Давление плазмы солнечного ветра
    /// </summary>
    public float SolarWindPressure { get; set; }

    /// <summary>
    /// Индекс Dst
    /// </summary>
    public float DstIndex { get; set; }

    /// <summary>
    /// Y-компонента межпланетного магнитного поля
    /// </summary>
    public float ByIMF { get; set; }

    /// <summary>
    /// Z-компонента межпланетного магнитного поля
    /// </summary>
    public float BzIMF { get; set; }

    /// <summary>
    /// Координаты точек линии магнитного поля
    /// </summary>
    public List<(float X, float Y, float Z)> FieldLineCoordinates { get; set; } = [];
}
