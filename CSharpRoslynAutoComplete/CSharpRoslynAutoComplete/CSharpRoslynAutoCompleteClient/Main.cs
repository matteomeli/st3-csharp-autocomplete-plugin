using System;
using System.IO;
using System.Collections.Generic;
using CSharpRoslynAutoComplete;
using NDesk.Options;

namespace CSharpRoslynAutoCompleteClient
{
	class MainClass
	{
		static int verbosity;

		public static void Main(string[] args)
		{
			bool show_help = false;
			string currentParameter = string.Empty;

			string code = string.Empty;
			int cursor = 0;
			List<string> assemblyPaths = new List<string>();

			var p = new OptionSet()
			{
				{ "p|program=", "the {PROGRAM} string to parse.", 
					v => {
						currentParameter = "p";
						code = v;
					}},
				{ "c|cursor=", 
					"the current position of the {CURSOR} in the {PROGRAM}\n" +
						"this must be an integer.",
					(int v) => {
						currentParameter = "c";
						cursor = v;
					}},
				{ "d|dlls=", "the {ASSEMBLY} filename values to load with the {PROGRAM}.",
					v => currentParameter = "d" },
				{ "v", "increase debug message verbosity",
					v => { if (v != null) ++verbosity; } },
				{ "<>", v => {
						switch (currentParameter)
						{
							case "d":
								assemblyPaths.Add(v);
								break;
						}
					}},
				{ "h|help",  "show this message and exit", 
					v => show_help = v != null },
			};

			try {
				p.Parse(args);
			}
			catch (OptionException e)
			{
				Console.WriteLine("CSharpRoslynAutoCompleteClient.exe: ");
				Console.WriteLine(e.Message);
				Console.WriteLine("Try `CSharpRoslynAutoCompleteClient.exe --help' for more information.");
				return;
			}

			if (show_help)
			{
				ShowHelp(p);
				return;
			}

			var prompter = new CSharpPrompter();
			var suggestions = prompter.Prompt(code, cursor, assemblyPaths);

			// Print any found suggestions
			foreach (var s in suggestions)
			{
				Console.WriteLine(s);
			}
		}

		static void ShowHelp (OptionSet p)
		{
			Console.WriteLine("Usage: CSharpRoslynAutoCompleteClient.exe [OPTIONS]+");
			Console.WriteLine("Prompt code suggestions at current cursor position inside a C# program.");
			Console.WriteLine();
			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Error);
		}
	}
}
