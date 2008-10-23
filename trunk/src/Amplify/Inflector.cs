//-----------------------------------------------------------------------
// <copyright file="http://andrewpeters.net/inflectornet/" author="Andrew Peters">
//     Copyright (c) Andrew Peters.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify // changed the name space to avoid conflictions of namespaces of other libraries
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text.RegularExpressions;

    [SuppressMessage("Microsoft.Performance", "CA1810", Justification = "It needs the static constructor")]
	public static class Inflector
	{
		#region Default Rules

		static Inflector()
		{
			AddPlural("$", "s");
			AddPlural("s$", "s");
			AddPlural("(ax|test)is$", "$1es");
			AddPlural("(octop|vir)us$", "$1i");
			AddPlural("(alias|status)$", "$1es");
			AddPlural("(bu)s$", "$1ses");
			AddPlural("(buffal|tomat)o$", "$1oes");
			AddPlural("([ti])um$", "$1a");
			AddPlural("sis$", "ses");
			AddPlural("(?:([^f])fe|([lr])f)$", "$1$2ves");
			AddPlural("(hive)$", "$1s");
			AddPlural("([^aeiouy]|qu)y$", "$1ies");
			AddPlural("(x|ch|ss|sh)$", "$1es");
			AddPlural("(matr|vert|ind)ix|ex$", "$1ices");
			AddPlural("([m|l])ouse$", "$1ice");
			AddPlural("^(ox)$", "$1en");
			AddPlural("(quiz)$", "$1zes");

			AddSingular("s$", string.Empty);
			AddSingular("(n)ews$", "$1ews");
			AddSingular("([ti])a$", "$1um");
			AddSingular("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
			AddSingular("(^analy)ses$", "$1sis");
			AddSingular("([^f])ves$", "$1fe");
			AddSingular("(hive)s$", "$1");
			AddSingular("(tive)s$", "$1");
			AddSingular("([lr])ves$", "$1f");
			AddSingular("([^aeiouy]|qu)ies$", "$1y");
			AddSingular("(s)eries$", "$1eries");
			AddSingular("(m)ovies$", "$1ovie");
			AddSingular("(x|ch|ss|sh)es$", "$1");
			AddSingular("([m|l])ice$", "$1ouse");
			AddSingular("(bus)es$", "$1");
			AddSingular("(o)es$", "$1");
			AddSingular("(shoe)s$", "$1");
			AddSingular("(cris|ax|test)es$", "$1is");
			AddSingular("(octop|vir)i$", "$1us");
			AddSingular("(alias|status)es$", "$1");
			AddSingular("^(ox)en", "$1");
			AddSingular("(vert|ind)ices$", "$1ex");
			AddSingular("(matr)ices$", "$1ix");
			AddSingular("(quiz)zes$", "$1");

			AddIrregular("person", "people");
			AddIrregular("man", "men");
			AddIrregular("child", "children");
			AddIrregular("sex", "sexes");
			AddIrregular("move", "moves");

			AddUncountable("equipment");
			AddUncountable("information");
			AddUncountable("rice");
			AddUncountable("money");
			AddUncountable("species");
			AddUncountable("series");
			AddUncountable("fish");
			AddUncountable("sheep");
		}

		#endregion

		private class Rule
		{
			private readonly Regex _regex;
			private readonly string _replacement;

			public Rule(string pattern, string replacement)
			{
				_regex = new Regex(pattern, RegexOptions.IgnoreCase);
				_replacement = replacement;
			}

			public string Apply(string word)
			{
				if (!_regex.IsMatch(word))
				{
					return null;
				}

				return _regex.Replace(word, _replacement);
			}
		}

		internal static void AddIrregular(string singular, string plural)
		{
			AddPlural("(" + singular[0] + ")" + singular.Substring(1) + "$", "$1" + plural.Substring(1));
			AddSingular("(" + plural[0] + ")" + plural.Substring(1) + "$", "$1" + singular.Substring(1));
		}

        [SuppressMessageAttribute("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "pointless")]
		internal static void AddUncountable(string word)
		{
            Uncountables.Add(word.ToLowerInvariant());
		}

		internal static void AddPlural(string rule, string replacement)
		{
			Plurals.Add(new Rule(rule, replacement));
		}

		internal static void AddSingular(string rule, string replacement)
		{
			Singulars.Add(new Rule(rule, replacement));
		}

        private static List<Rule> s_plurals;
		private static List<Rule> s_singulars;
        private static List<string> s_uncountables;

        private static List<Rule> Plurals
        {
            get
            {
                if (s_plurals == null)
                    s_plurals = new List<Rule>();
                return s_plurals;
            }
        }

        private static List<Rule> Singulars
        {
            get
            {
                if (s_singulars == null)
                    s_singulars = new List<Rule>();
                return s_singulars;
            }
        }

        private static List<string> Uncountables
        {
            get
            {
                if (s_uncountables == null)
                    s_uncountables = new List<string>();
                return s_uncountables;
            }
        }

		public static string Pluralize(string word)
		{
			return ApplyRules(Plurals, word);
		}

		public static string Singularize(string word)
		{
			return ApplyRules(Singulars, word);
		}

		[SuppressMessageAttribute("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "pointless")]
		private static string ApplyRules(List<Rule> rules, string word)
		{
			string result = word;

			if (!s_uncountables.Contains(word.ToLowerInvariant()))
			{
				for (int i = rules.Count - 1; i >= 0; i--)
				{
					if ((result = rules[i].Apply(word)) != null)
					{
						break;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Capitalizes each word and turns underscores into spaces.
		/// </summary>
		/// <param name="word">The string value to be parsed.</param>
		/// <returns>The new value.</returns>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1704", Justification = "Its correctly named")]
		public static string Titleize(string word)
		{
			return Regex.Replace(Humanize(Underscore(word)), @"\b([a-z])",
								 delegate(Match match)
								 {
									 return match.Captures[0].Value.ToUpperInvariant();
								 });
		}

		/// <summary>
		/// Capitalizes the first word and replaces the underscores with spaces. 
		/// </summary>
		/// <param name="lowercaseAndUnderscoredWord">The string value to be parsed.</param>
		/// <returns>The new value.</returns>
		public static string Humanize(string lowercaseAndUnderscoredWord)
		{
			return Capitalize(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
		}

		/// <summary>
		/// Converts a lowercased and underscored word into pascal case.
		/// </summary>
		/// <param name="lowercaseAndUnderscoredWord">The string value to be parsed.</param>
		/// <returns>The new value.</returns>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1704", Justification = "Its correctly named")]
		public static string Pascalize(string lowercaseAndUnderscoredWord)
		{
			return Regex.Replace(lowercaseAndUnderscoredWord, "(?:^|_)(.)",
								 delegate(Match match)
								 {
									 return match.Groups[1].Value.ToUpperInvariant();
								 });
		}

		/// <summary>
		/// Converts an underscored and lowercased word into Camelcases.
		/// </summary>
		/// <param name="lowercaseAndUnderscoredWord">The string value to be parsed.</param>
		/// <returns>The new value.</returns>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1704", Justification = "Its correctly named")]
		public static string Camelize(string lowercaseAndUnderscoredWord)
		{
			return Uncapitalize(Pascalize(lowercaseAndUnderscoredWord));
		}

		/// <summary>
		/// Converts a pascal cased word into a word with underscores.
		/// </summary>
		/// <param name="pascalCasedWord">The string value to be parsed.</param>
		/// <returns>The new value.</returns>
        [SuppressMessageAttribute("Microsoft.Globalization", "CA1308", Justification = "pointless")]
		public static string Underscore(string pascalCasedWord)
		{
			return Regex.Replace(
			  Regex.Replace(
				Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
				"$1_$2"), @"[-\s]", "_").ToLowerInvariant();
		}

		/// <summary>
		/// Capitalizes the first letter of the string/word
		/// </summary>
		/// <param name="word">The string value to be parsed.</param>
		/// <returns>The new string value.</returns>
        [SuppressMessageAttribute("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
		public static string Capitalize(string word)
		{
            return word.Substring(0, 1).ToUpperInvariant() + word.Substring(1).ToLowerInvariant();
		}

		/// <summary>
		/// Lowercases the first letter of the string/word
		/// </summary>
		/// <param name="word">The string value to be parsed.</param>
		/// <returns>The new string value.</returns>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1704")]
        [SuppressMessageAttribute("Microsoft.Globalization", "CA1308")]
		public static string Uncapitalize(string word)
		{
			return word.Substring(0, 1).ToLowerInvariant() + word.Substring(1);
		}

		/// <summary>
		/// Adds numeric suffixes to numbers, like 1st, 2nd, 3rd, 4th, etc.
		/// </summary>
		/// <param name="number">The string value to be parsed.</param>
		/// <returns></returns>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1704")]
		public static string Ordinalize(string number)
		{
			int n = int.Parse(number, CultureInfo.InvariantCulture);
			int nMod100 = n % 100;

			if (nMod100 >= 11 && nMod100 <= 13)
			{
				return number + "th";
			}

			switch (n % 10)
			{
				case 1:
					return number + "st";
				case 2:
					return number + "nd";
				case 3:
					return number + "rd";
				default:
					return number + "th";
			}
		}

		/// <summary>
		/// Changes underscores to hyphens in a string.
		/// </summary>
		/// <param name="underscoredWord">The string value to be parsed.</param>
		/// <returns>The new string value.</returns>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1704")]
		public static string Dasherize(string underscoredWord)
		{
			return underscoredWord.Replace('_', '-');
		}
	}
}