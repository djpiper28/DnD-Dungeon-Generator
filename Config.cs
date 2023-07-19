namespace DungeonGenerator.Configuration;

public class Config
{
    public Config(int width, int height, int pathWidth)
    {
        Width = width;
        Height = height;
        PathWidth = pathWidth;
        Validate();
    }

    public int Width { get; }
    public int Height { get; }
    public int PathWidth { get; }

    private void Validate()
    {
        if (Width <= 0) throw new ArgumentException("Width must be greater than 0");

        if (Height <= 0) throw new ArgumentException("Height must be greater than 0");

        if (PathWidth <= 0) throw new ArgumentException("Path width must be greater than 0");
    }
}