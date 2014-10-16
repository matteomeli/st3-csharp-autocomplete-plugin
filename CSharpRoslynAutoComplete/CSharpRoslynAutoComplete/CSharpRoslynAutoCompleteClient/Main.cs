using System;
using System.IO;
using System.Collections.Generic;
using CSharpRoslynAutoComplete;
using NDesk.Options;

namespace CSharpRoslynAutoCompleteClient
{
	class MainClass
	{
		static CSharpPrompter prompter = new CSharpPrompter();
		static int verbosity = 0;

		public static void Main(string[] args)
		{
			bool show_help = false;
			string currentParameter = string.Empty;
			bool useInteractiveMode = false;

			string code = string.Empty;
			int cursor = -1;
			List<string> assemblyPaths = new List<string>();

			// Build the arguments list
			var p = new OptionSet()
			{
				{ "p|program", 
					"the {PROGRAM} string to parse.", 
					v => {
						currentParameter = "p";
					}
				},
				{ "c|cursor", 
					"the current position of the {CURSOR} in the {PROGRAM} - " +
						"this must be an integer.",
					v => {
						currentParameter = "c";
					}
				},
				{ "d|dlls", 
					"the {ASSEMBLY} filepath lists to load with the {PROGRAM} - " +
						"this must be space separated paths.",
					v => currentParameter = "d"
				},
				{ "v|verbose", 
					"increase debug message verbosity",
					v => {
						if (v != null) ++verbosity; 
					} 
				},
				{
					"i|interactive",
					"run the program in interactive mode.",
					v => useInteractiveMode = v != null
				},
				{ "h|help",  
					"show this message and exit", 
					v => show_help = v != null
				},
				{ "<>", v => {
						switch (currentParameter)
						{
							case "d":
								assemblyPaths.Add(v);
								break;
							case "p":
								// Only parse the first string passed
								if (string.IsNullOrEmpty(code))
									code = v;
								break;
							case "c":
								// Only try to parse the first integer number passed
								if (cursor < 0)
								{
									if (int.TryParse(v, out cursor) == false)
									{
										cursor = -1;
									}
								}
								break;
						}
					}
				}
			};

			// Parse the arguments
			try
			{
				p.Parse(args);
			}
			catch (OptionException e)
			{
				Console.Write("CSharpRoslynAutoCompleteClient.exe: ");
				Console.WriteLine(e.Message);
				Console.WriteLine("Try `CSharpRoslynAutoCompleteClient.exe --help' for more information.");
				return;
			}

			if (show_help)
			{
				ShowHelp(p);
				return;
			}

			// Check for required arguments
			if (string.IsNullOrEmpty(code))
			{
				Console.WriteLine("CSharpRoslynAutoCompleteClient.exe: Missing required option -p|--program <PROGRAM>");
				Console.WriteLine("Try `CSharpRoslynAutoCompleteClient.exe --help' for more information.");
				return;
			}
			else if (cursor < 0)
			{
				Console.WriteLine("CSharpRoslynAutoCompleteClient.exe: Missing required option -c|--cursor <CURSOR> or error in parsing the argument.");
				Console.WriteLine("Try `CSharpRoslynAutoCompleteClient.exe --help' for more information.");
				return;
			}

			// Verbose output
			if (verbosity > 0 || useInteractiveMode)
			{
				PrintVerboseAdditionalAssembliesInfo(assemblyPaths);
				PrintVerboseProgramInfo(code, cursor);
				Console.WriteLine();
			}

			var suggestions = prompter.Prompt(code, cursor, assemblyPaths, verbosity);

			// Print any found suggestions
			foreach (var s in suggestions)
			{
				Console.WriteLine(s);
			}

			// Check for interactive mode
			if (useInteractiveMode)
			{
				while (true)
				{
					Console.WriteLine();
					Console.WriteLine("Press the arrow keys LEFT and RIGHT to move the cursor, then ENTER to set it. ESC to quit.");

					ConsoleKeyInfo cki;
					do
					{
						cki = Console.ReadKey(true);
						if (cki.Key == ConsoleKey.Escape)
						{
							Console.WriteLine("Bye!");
							return;
						}
						// Cache arrow presses
						if (cki.Key == ConsoleKey.LeftArrow)
						{
							cursor--;
						}
						else if (cki.Key == ConsoleKey.RightArrow)
						{
							cursor++;
						}

						if (cursor < 0 || cursor > code.Length)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("The required cursor position is not in range within program string. Try again.");
							Console.ResetColor();
							continue;
						}

						PrintVerboseProgramInfo(code, cursor);
					}
					while (cki.Key != ConsoleKey.Enter);

					Console.WriteLine();
					suggestions = prompter.Prompt(code, cursor, assemblyPaths, verbosity);

					// Print any found suggestions
					foreach (var s in suggestions)
					{
						Console.WriteLine(s);
					}
				}
			}
		}

		static void ShowHelp(OptionSet p)
		{
			Console.WriteLine("Usage: CSharpRoslynAutoCompleteClient.exe");
			Console.WriteLine("Prompt code suggestions at current cursor position inside a C# program.");
			Console.WriteLine();
			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Error);
		}

		static void PrintVerboseProgramInfo(string code, int cursor)
		{
			Console.WriteLine();
			Console.Write("PROGRAM (cursor is at "); 
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(cursor);
			Console.ResetColor();
			Console.WriteLine("):");

			int currentChar = 0;
			foreach (Char c in code.ToCharArray())
			{
				if (currentChar == cursor)
				{
					Console.ForegroundColor = ConsoleColor.Green;
				}

				if (currentChar == cursor && Char.IsWhiteSpace(c))
				{
					Console.Write("_");
				}
				else
				{
					Console.Write(c);
				}

				if (currentChar == cursor)
				{
					Console.ResetColor();
				}

				currentChar++;
			}
			Console.WriteLine();
		}

		static void PrintVerboseAdditionalAssembliesInfo(List<string> assemblyPaths)
		{
			Console.WriteLine();
			Console.WriteLine("INCLUDED ASSEMBLIES:");
			assemblyPaths.ForEach(t => Console.WriteLine(t));
		}
	}
}
