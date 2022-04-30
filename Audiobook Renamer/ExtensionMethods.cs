namespace Audiobook_Renamer;

public static class ExtensionMethods
{
    public static void Invoke(this Control @this, Action action)
    {
        ArgumentNullException.ThrowIfNull(@this, nameof(@this));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        if (@this.InvokeRequired)
        {
            @this.Invoke(action);
        }
        else
        {
            action();
        }
    }
}
