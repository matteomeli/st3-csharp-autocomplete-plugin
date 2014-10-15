using System;
using System.IO;
using CSharpRoslynAutoComplete;

namespace CSharpRoslynAutoCompleteClient
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				PrintHelp();
				return;
			}

			// Parse cursor position in code string
			int cursorPosition = 0;
			if (int.TryParse(args[1], out cursorPosition) == false)
			{
				PrintHelp();
				return;
			}

			// Read code string
			string code = args[0];

			var prompter = new CSharpPrompter();
			var suggestions = prompter.Prompt(code, cursorPosition);

			// Print any found suggestions
			foreach (var s in suggestions)
			{
				Console.WriteLine(s);
			}
		}

		static void PrintHelp()
		{
			Console.WriteLine("Usage: CSharpCodeCompleteClient <filename> <cursor>");
			Console.WriteLine("\t<filename> - Name of the code file to open.");
			Console.WriteLine("\t<cursor>   - Position of the cursor in the code file buffer.");
		}
	}
}
