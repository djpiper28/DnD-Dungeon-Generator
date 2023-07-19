namespace DungeonGenerator.Configuration;

public class ConfigBuilder
{
    public int Width { get; set; } = 500;
    public int Height { get; set; } = 1000;
    public int PathWidth { get; set; } = 2;

    public Config Build()
    {
        return new Config(Width, Height, PathWidth);
    }
}