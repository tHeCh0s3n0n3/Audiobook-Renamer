namespace Audiobook_Renamer;

public static class ExtensionMethods
{
    /// <summary>
    /// Invokes a method on a Control object if Invoking is required (eg. if called
    /// from a different thread).
    /// </summary>
    /// <param name="control">The control on which to Invoke.</param>
    /// <param name="action">The action to perform.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void Invoke(this Control control, Action action)
    {
        ArgumentNullException.ThrowIfNull(control, nameof(control));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        if (control.InvokeRequired)
        {
            control.Invoke(action);
        }
        else
        {
            action();
        }
    }
}
