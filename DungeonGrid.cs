namespace DungeonGenerator.Configuration;

public class DungeonGrid
{
    private readonly bool[,] _grid;
    private readonly int _width;
    private readonly int _height;
    private readonly int _pathWidth;

    public DungeonGrid(int width, int height, int pathWidth)
    {
        _width = width;
        _height = height;
        _pathWidth = pathWidth;

        _grid = new bool[width, height];
        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
            _grid[x, y] = true;

        generateGrid();
    }

    private void generateGrid()
    {
        var rand = new Random();
        var currentAngle = 2 * Math.PI * rand.NextDouble();
        double x = _width / 2;
        double y = _height / 2;

        const double MAX_ANGLE_CHANGE = Math.PI / 4;
        const double STEP = 0.1;

        while (x > 0 && y > 0 && x < _width && y < _height)
        {
            // Clear the tangent to the current angle of travel
            clearPath(x, y, 1 / currentAngle);

            currentAngle += MAX_ANGLE_CHANGE * rand.NextDouble() - 0.5;
            x = x + STEP * Math.Cos(currentAngle);
            y = y + STEP * Math.Sin(currentAngle);
        }
    }

    private void clearPath(double x, double y, double rotation)
    {
        for (var i = -_pathWidth / 2.0; i < _pathWidth / 2.0; i++)
        {
            var newx = (int)Math.Round(x + i * Math.Cos(rotation));
            var newy = (int)Math.Round(y + i * Math.Sin(rotation));
            clearPixel(newx, newy);
        }
    }

    private void clearPixel(int x, int y)
    {
        if (x < 0 || y < 0) return;

        if (x >= _width || y >= _height) return;

        _grid[x, y] = false;
    }

    public void PrintGrid()
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
                if (_grid[x, y])
                    Console.Write("██");
                else
                    Console.Write("  ");
            Console.WriteLine("");
        }
    }
}