using System.Drawing;
using ImageMagick;

namespace DungeonGenerator.Configuration;

public class OutputImageColouriser
{
    private readonly MagickImage _image;
    private readonly int[,] _wallDistanceMap; // Each pixel has the distance to the closest black pixel
    private readonly int _width, _height;
    private readonly IPixelCollection<byte> _pixels;

    public OutputImageColouriser(MagickImage _image)
    {
        this._image = _image;
        _width = _image.Width;
        _height = _image.Height;
        _pixels = _image.GetPixels();
        
        _wallDistanceMap = new int[_image.Width, _image.Height];
        for (var y = 0; y < _height; y++)
        for (var x = 0; x < _width; x++)
            if (!IsBlack(_pixels.GetPixel(x, y).ToArray()))
                _wallDistanceMap[x, y] = int.MaxValue;

        for (var y = 0; y < _height; y++)
        for (var x = 0; x < _width; x++)
            GenerateWallDistanceMap(x, y);
    }

    private void GenerateWallDistanceMap(int xIn, int yIn)
    {
        if (IsBlack(_pixels.GetPixel(xIn, yIn).ToArray())) return;

        var minDistance = int.MaxValue;
        for (var yy = yIn - 1; yy <= yIn + 1; yy++)
        for (var xx = xIn - 1; xx <= xIn + 1; xx++)
            if (xx >= 0 && yy >= 0 && xx < _width && yy < _height && !(xx == xIn && yy == yIn))
                minDistance = int.Min(minDistance, _wallDistanceMap[xx, yy]);

        var fillVal = _wallDistanceMap[xIn, yIn] = minDistance + 1;

        for (var yy = yIn - 1; yy <= yIn + 1; yy++)
        for (var xx = xIn - 1; xx <= xIn + 1; xx++)
            if (xx >= 0 && yy >= 0 && xx < _width && yy < _height && !(xx == xIn && yy == yIn))
                if (_wallDistanceMap[xx, yy] > fillVal + 1 && _wallDistanceMap[xx, yy] != int.MaxValue)
                    GenerateWallDistanceMap(xx, yy);
    }

    public void ColouriseImage()
    {
        for (var x = 0; x < _width; x++)
        for (var y = 0; y < _height; y++)
            if (!IsBlack(_pixels.GetPixel(x, y).ToArray()))
                FillPath(x, y);
    }

    private void FillPath(int x, int y)
    {
        var luminance = Math.Log(Math.E, 5 + _wallDistanceMap[x, y]) * 3;
        byte[] c = {10 * 255 / 100, 60 * 255 / 100, (byte)(255 * double.Min(1, 55 * luminance / 100.0))};
        _pixels.SetPixel(x, y, c);
    }

    private static bool IsBlack(byte[] pixel)
    {
        for (var i = 0; i < pixel.Length && i < 3; i++)
            if (pixel[i] != 0)
                return false;
        return true;
    }
}