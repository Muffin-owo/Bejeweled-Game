public class Gem
{
    public GemColor Color { get; set; }

    public PowerType PowerType { get; set; }

    public Gem(GemColor color, PowerType powerType = PowerType.None)
    {
        Color = color;
        PowerType = powerType;
    }

    public bool IsPower()
    {
        return PowerType != PowerType.None;
    }

    public bool IsNormal()
    {
        return PowerType == PowerType.None;
    }
}