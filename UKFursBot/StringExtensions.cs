namespace UKFursBot;

public static class StringExtensions
{
    public static string ToLowerCaseWithUnderscores(this string sourceString)
    {
        for (var i = 1; i < sourceString.Length; i++)
        {
            var character = sourceString[i];

            if (Char.IsUpper(character))
            {
                sourceString = sourceString.Insert(i, "_");
                i++;
            }
        }
        return sourceString.ToLowerInvariant();
    }

    public static string WithoutUnderscores(this string sourceString)
    {
        return sourceString.Replace("_", string.Empty);
    }
}