using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Adapters.LogAccess;

public partial class ColorfulConsoleLog
{
    public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;

    public ConsoleColor WarningColor { get; set; } = ConsoleColor.DarkYellow;

    public ConsoleColor SuccessColor { get; set; } = ConsoleColor.Green;

    public ConsoleColor DataKeyColor { get; set; } = ConsoleColor.White;

    public ConsoleColor DataValueColor { get; set; } = ConsoleColor.DarkGray;

    public int? BinaryMaxLength { get; set; }

    public BinaryDisplayFormat BinaryFormat { get; set; }
}