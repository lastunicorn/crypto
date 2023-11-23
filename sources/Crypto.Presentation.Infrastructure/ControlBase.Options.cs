using DustInTheWind.ConsoleTools.Commando;

namespace DustInTheWind.Crypto.Presentation.Infrastructure;

public partial class ControlBase
{
    public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;

    public ConsoleColor WarningColor { get; set; } = ConsoleColor.DarkYellow;

    public ConsoleColor SuccessColor { get; set; } = ConsoleColor.Green;

    public ConsoleColor DataKeyColor { get; set; } = ConsoleColor.White;

    public ConsoleColor DataValueColor { get; set; } = ConsoleColor.DarkGray;

    public int? BinaryMaxLength { get; set; }

    public BinaryDisplayFormat BinaryFormat { get; set; }
}