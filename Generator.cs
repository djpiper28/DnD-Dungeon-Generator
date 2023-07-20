using ImageMagick;

namespace DungeonGenerator.Configuration;

public class Generator
{
    private readonly DungeonGrid _grid;
    private Config _config;

    public Generator(Config config)
    {
        _config = config;
        _grid = new DungeonGrid(config.Width, config.Height, config.PathWidth);

        using var image = new MagickImage(new MagickColor("#000000"), _grid.RealWidth(), _grid.RealHeight());
        _grid.SaveGrid(image);
        image.Write("output.png");
    }
}