using DustInTheWind.ConsoleTools.Commando;

namespace DustInTheWind.Crypto.Presentation.Infrastructure;

public abstract class CustomView<T> : ControlBase, IView<T>
{
    public abstract void Display(T viewModel);
}