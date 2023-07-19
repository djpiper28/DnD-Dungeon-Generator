using ImageMagick;

namespace DungeonGenerator.Configuration;

public class DungeonGrid
{
    private readonly bool[,] _grid;
    private readonly int _height;
    private readonly int _pathWidth;
    private readonly int _width;

    private int _xMin, _yMin, _xMax, _yMax;

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

    private void UpdateMin(int x, int y)
    {
        if (x < _xMin) _xMin = x;
        if (x > _xMax) _xMax = x;
        if (y < _yMin) _yMin = y;
        if (y > _yMax) _yMax = y;
    }

    private void generateGrid()
    {
        var rand = new Random();
        var currentAngle = 2 * Math.PI * rand.NextDouble();
        var x = _width / 2.0;
        var y = _height / 2.0;

        _xMax = _xMin = (int)x;
        _yMax = _yMin = (int)y;

        const double maxAngleChange = Math.PI / 4.0;
        const double step = 0.1;

        while (x > 0 && y > 0 && x < _width && y < _height)
        {
            // Clear the tangent to the current angle of travel
            ClearPath(x, y, 1 / currentAngle);

            currentAngle += maxAngleChange * rand.NextDouble() - 0.5;
            x += step * Math.Cos(currentAngle);
            y += step * Math.Sin(currentAngle);
            UpdateMin((int)x, (int)y);
        }
    }

    private void ClearPath(double x, double y, double rotation)
    {
        for (var i = -_pathWidth / 2.0; i < _pathWidth / 2.0; i++)
        {
            var newx = (int)Math.Round(x + i * Math.Cos(rotation));
            var newy = (int)Math.Round(y + i * Math.Sin(rotation));
            ClearPixel(newx, newy);
        }
    }

    private void ClearPixel(int x, int y)
    {
        if (x < 0 || y < 0) return;

        if (x >= _width || y >= _height) return;

        _grid[x, y] = false;
    }

    public void SaveGrid()
    {
        const int border = 10;
        byte[] white = { 0xFF, 0XFF, 0xFF };
        var realWidth = border + _xMax - _xMin;
        var realHeight = border + _yMax - _yMin;

        using var image = new MagickImage(new MagickColor("#000000"), realWidth, realHeight);
        for (var y = _yMin; y < _yMax; y++)
        for (var x = _xMin; x < _xMax; x++)
            if (!_grid[x, y])
                image.GetPixels().SetPixel(x + border / 2 - _xMin, y + border / 2 -_yMin, white);
        image.Write("output.png");
    }
}