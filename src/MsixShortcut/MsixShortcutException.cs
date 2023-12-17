namespace MsixShortcut;

public sealed class MsixShortcutException : Exception
{
    public MsixShortcutException(string message)
        : base(message)
    { }

    public MsixShortcutException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
