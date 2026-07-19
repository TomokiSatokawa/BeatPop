using System.Threading;

/// <summary>
/// UnitaskŠÖ˜A‚Ì‹¤’Êˆ—
/// </summary>
public static class UniTaskUtility
{
    public static void SafeDispose(ref CancellationTokenSource cts)
    {
        if (cts == null)
            return;

        cts.Cancel();
        cts.Dispose();
        cts = null;
    }
}
