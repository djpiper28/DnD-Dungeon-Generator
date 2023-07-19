namespace DungeonGenerator.Configuration;

public class Generator
{
    private Config _config;
    private readonly DungeonGrid _grid;

    public Generator(Config config)
    {
        _config = config;
        _grid = new DungeonGrid(config.Width, config.Height, config.PathWidth);
        _grid.PrintGrid();
    }
}