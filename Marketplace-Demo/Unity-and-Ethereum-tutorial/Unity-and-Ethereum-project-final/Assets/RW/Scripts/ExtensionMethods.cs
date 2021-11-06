static class ExtensionMethods
{
    /// <summary>
    /// Nudge the value passed up or down by a random percentage between 0 and the % passed
    /// e.g. 5.Nudge(20) will return 5 +/- 20% which is anything from 4 to 6
    /// </summary>   
    public static float Nudge(this float nudgeMe, int nudgeAmountMaxPercentage)
    {
        float adjustment = nudgeMe * (UnityEngine.Random.value * (nudgeAmountMaxPercentage / 100f));
        if (UnityEngine.Random.value < 0.5f)
        {
            adjustment *= -1;
        }
        return (nudgeMe + adjustment);
    }
}

