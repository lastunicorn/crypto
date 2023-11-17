namespace DustInTheWind.Crypto.Ports.LogAccess;

public interface ILog
{
    void WriteTitle(string title);

    void WriteInfo(string message);

    void WriteSuccess(string message);

    void WriteWarning(string message);

    void WriteError(string message);

    void WriteError(Exception exception);

    void IncreaseIndentation();

    void DecreaseIndentation();

    void WithIndentation(string title, Action action);

    void WithIndentation(Action action);

    T WithIndentation<T>(string title, Func<T> action);

    T WithIndentation<T>(Func<T> action);

    void WriteValue(string name, object value);

    void WriteValue(string name, byte[] bytes, BinaryDisplayFormat? format = null);

    void WriteType(string name, object obj);

    void WriteValueBelowName(string name, object value);

    void WriteLine();

    void WriteHorizontalLine(int margin = 0);

    void WriteHorizontalLine(int topMargin, int bottomMargin);
}