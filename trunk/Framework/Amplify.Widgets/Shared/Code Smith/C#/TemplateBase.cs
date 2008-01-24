#region 
//==============================================================================================
// CSLA 2.x CodeSmith Templates for C#
// Author: Ricky A. Supit (rsupit@hotmail.com)
//
// This software is provided free of charge. Feel free to use and modify anyway you want it.
//==============================================================================================
#endregion
using System;
using System.Collections;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Text.RegularExpressions;

using CodeSmith.Engine;
using SchemaExplorer;
using CodeSmith.CustomProperties;
using System.Xml;


namespace CodeSmith.Csla
{
	using CodeSmith.Csla;
	
	
	
	
	public class Table 
	{
		private TableSchema table;
		private string name;
		
		public string Name { get { return this.name; } }
		
		public Table(TableSchema table, StringCollection prefixes) 
		{

			this.table = table;
			this.name = table.Name;
			foreach(string prefix in prefixes) {
				if(this.name.StartsWith(prefix)) {
					this.name = this.name.Replace(prefix, "");
					break;
				}	
			}
		}
	}
	
    public class TemplateBase : CodeSmith.Engine.CodeTemplate 
    {
		
		
		
		public string Pluralize(string word) {
			return Inflector.Pluralize(word);	
		}
		
		
		
		public string GetObjectName(string tableName, StringCollection prefixes) 
		{
			if(prefixes != null) 
			{
				foreach(string prefix in prefixes) 
				{
					if(tableName.StartsWith(prefix)) {
						tableName = tableName.Replace(prefix, "");
						
						return Inflector.Singularize(tableName);
					}
				}
			}
			return Inflector.Singularize(tableName);
		}
		
		public string GetListName(string tableName) 
		{
			
			return (Inflector.UnCountable(tableName)) ? tableName + "List" : Inflector.Pluralize(tableName);	
		}
		
		public string[] GetTableNames(TableSchemaCollection tables, StringCollection prefixes)
		{
			System.Collections.ArrayList list = new System.Collections.ArrayList();
			foreach(TableSchema table in tables) {
				if(table.Name != null && table.Name.Length > 0)
					list.Add(this.GetObjectName(table.Name, prefixes));	
			}
			return (string[])list.ToArray(typeof(string));
		}
			
		
		public string Indent(int indent) {
			string value = "";
			for(int i = 0; i < indent; i++) {
				value += "\t";
			}
			return value;
		}
		
		#region Inflector
		
		public class Inflector
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

			AddSingular("s$", "");
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
			AddUncountable("membership");
			AddUncountable("news");
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

		private static void AddIrregular(string singular, string plural)
		{
			AddPlural("(" + singular[0] + ")" + singular.Substring(1) + "$", "$1" + plural.Substring(1));
			AddSingular("(" + plural[0] + ")" + plural.Substring(1) + "$", "$1" + singular.Substring(1));
		}

		private static void AddUncountable(string word)
		{
			_uncountables.Add(word.ToLower());
		}

		private static void AddPlural(string rule, string replacement)
		{
			_plurals.Add(new Rule(rule, replacement));
		}

		private static void AddSingular(string rule, string replacement)
		{
			_singulars.Add(new Rule(rule, replacement));
		}

		private static readonly System.Collections.ArrayList _plurals = new System.Collections.ArrayList();
		private static readonly System.Collections.ArrayList _singulars = new System.Collections.ArrayList();
		private static readonly System.Collections.ArrayList _uncountables = new System.Collections.ArrayList();

		public static bool UnCountable(string word) 
		{
			return _uncountables.Contains(word.ToLower());
		}

		public static string Pluralize(string word)
		{
			return ApplyRules(_plurals, word);
		}
		
		

		public static string Singularize(string word)
		{
			return ApplyRules(_singulars, word);
		}

		private static string ApplyRules(System.Collections.ArrayList rules, string word)
		{
			string result = word;
			
			if(result == null)
				return "";
			
			if (!_uncountables.Contains(word.ToLower()))
			{
				for (int i = rules.Count - 1; i >= 0; i--)
				{
					string test = ((Rule)rules[i]).Apply(word);
					if (test != null && test.Length > 0)
					{
						result = test;
						return result;
					}
				}
			}

			return result;
		}

		public static string Titleize(string word)
		{
			return Regex.Replace(Humanize(Underscore(word)), @"\b([a-z])", MatchTitleize);
		}

		private static string MatchTitleize(Match match)
		{
			return match.Captures[0].Value.ToUpper();
		}

		public static string Humanize(string lowercaseAndUnderscoredWord)
		{
			return Capitalize(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
		}

		public static string Pascalize(string lowercaseAndUnderscoredWord)
		{
			return Regex.Replace(lowercaseAndUnderscoredWord, "(?:^|_)(.)", MatchPascalize);
		}

		private static string MatchPascalize(Match match)
		{
			return match.Groups[1].Value.ToUpper();
		}

		public static string Camelize(string lowercaseAndUnderscoredWord)
		{
			return Uncapitalize(Pascalize(lowercaseAndUnderscoredWord));
		}

		public static string Underscore(string pascalCasedWord)
		{
			return Regex.Replace(
			  Regex.Replace(
				Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
				"$1_$2"), @"[-\s]", "_").ToLower();
		}

		public static string Capitalize(string word)
		{
			return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
		}

		public static string Uncapitalize(string word)
		{
			return word.Substring(0, 1).ToLower() + word.Substring(1);
		}

		public static string Ordinalize(string number)
		{
			int n = int.Parse(number);
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

		public static string Dasherize(string underscoredWord)
		{
			return underscoredWord.Replace('_', '-');
		}
	}
		
		#endregion
		
		
		#region Assocation
		
		
		public class Association 
		{
			private bool belongsTo = false;
			private bool hasOne = false;
			private bool hasMany = false;
			private string owner = "";
			private string child = "";
			
			public string Owner { get { return this.owner; } }
			
			public string Child { get { return this.child; } }
			
			public bool SingleOwner {
				get { 
					string single = Inflector.Singularize(this.owner);
					return (single == this.owner);
				}
			}
			
			public bool SingleChild
			{
				get {
					string single = Inflector.Singularize(this.child);
					return (single == this.child);
				}
			}
			
			public Association(string association) 
			{
				
				string[] pair = association.Split("_".ToCharArray());
				this.owner = pair[0];
				this.child = pair[1];
			}
			
		}
		
		#endregion 
	}
}
