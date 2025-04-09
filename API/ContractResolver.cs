using Newtonsoft.Json.Serialization;
using System.Globalization;

public class UpperCaseWordsNamingStrategy : NamingStrategy
{
    protected override string ResolvePropertyName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        // Split the name into words based on camelCase convention
        var words = System.Text.RegularExpressions.Regex.Split(name, @"(?<!^)(?=[A-Z])");

        // Capitalize the first letter of each word and join them back together
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(words[i]);
        }

        return string.Join(string.Empty, words);
    }
}
