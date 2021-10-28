namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Helper for common regular expressions.
    /// </summary>
    public static class RegExHelper
    {
        /// <summary>
        /// Finds the word instances.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="word">The word.</param>
        /// <returns>The matches.</returns>
        public static MatchCollection FindWordInstances(string input, string word)
        {
            var regEx = new Regex($"\\b{word}\\b", RegexOptions.IgnoreCase);
            return regEx.Matches(input);
        }

        /// <summary>
        /// Determines wether the given word is the last word in a statement.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="word">The word.</param>
        /// <returns>The matches.</returns>
        public static bool IsLastWord(string input, string word)
        {
            var matches = RegExHelper.FindWordInstances(input, word);

            if (matches.Count > 0)
            {
                Match lastMatch = matches.Last();
                return lastMatch.Index == (input.Length - word.Length);
            }

            return false;
        }

        /// <summary>
        /// Finds the variable replacements.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The matches.</returns>
        public static MatchCollection FindVariableReplacements(string input)
        {
            var regEx = new Regex("{(.*?)}");
            return regEx.Matches(input);
        }
    }
}
