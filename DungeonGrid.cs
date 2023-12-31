using ImageMagick;

namespace DungeonGenerator.Configuration;

public class DungeonGrid
{
    private const int border = 10;
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

        GenerateGrid();
    }

    private void UpdateMin(int x, int y)
    {
        if (x < _xMin) _xMin = x;
        if (x > _xMax) _xMax = x;
        if (y < _yMin) _yMin = y;
        if (y > _yMax) _yMax = y;
    }

    private void GenerateGrid()
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
            ClearPath(x, y, currentAngle + Math.PI / 2.0);

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
            var newx = (int)(x + i * Math.Cos(rotation));
            var newy = (int)(y + i * Math.Sin(rotation));
            ClearPixel(newx, newy);
        }
    }

    private void ClearPixel(int x, int y)
    {
        if (x < 0 || y < 0) return;

        if (x >= _width || y >= _height) return;

        _grid[x, y] = false;
    }

    public void SaveGrid(MagickImage image)
    {
        byte[] white = { 0xFF, 0XFF, 0xFF };
        var pixels = image.GetPixels();

        for (var y = _yMin; y < _yMax; y++)
        for (var x = _xMin; x < _xMax; x++)
            if (!_grid[x, y])
                pixels.SetPixel(x + border / 2 - _xMin, y + border / 2 - _yMin, white);
    }

    public int RealWidth()
    {
        return border + _xMax - _xMin;
    }

    public int RealHeight()
    {
        return border + _yMax - _yMin;
    }
}