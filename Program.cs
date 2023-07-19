using System.Text;
using DungeonGenerator.Configuration;

public class Program
{
    private readonly ConfigBuilder _configBuilder = new();

    public Program(string[] args)
    {
        foreach (var arg in args)
            if (!HandleArg(arg))
                throw new ArgumentException($"Invalid argument {arg}");

        new Generator(_configBuilder.Build());
    }

    private bool HandleArg(string arg)
    {
        var argParts = arg.Split("=");
        var stringBuilder = new StringBuilder();
        var prop = argParts[0];
        for (var i = 1; i < argParts.Length; i++)
        {
            stringBuilder.Append(argParts[i]);
            if (i != argParts.Length - 1) stringBuilder.Append("=");
        }

        var value = stringBuilder.ToString();
        switch (prop)
        {
            case "width":
                _configBuilder.Width = int.Parse(value);
                return true;
            case "height":
                _configBuilder.Height = int.Parse(value);
                return true;
            case "path-width":
                _configBuilder.PathWidth = int.Parse(value);
                return true;
        }

        return false;
    }

    public static void Main(string[] args)
    {
        new Program(args);
    }
}