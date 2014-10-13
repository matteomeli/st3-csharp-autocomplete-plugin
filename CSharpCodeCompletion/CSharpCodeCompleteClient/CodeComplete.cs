using System;
using System.IO;
using CSharpCodeCompletion;

namespace CSharpCodeCompleteClient
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

			// Parse code file
			string code = args[0];

			int cursorPosition = 0;
			if (int.TryParse(args[1], out cursorPosition) == false)
			{
				PrintHelp();
				return;
			}

			var service = new CSharpCodeCompletionService();
			var suggestions = service.Suggest(code, cursorPosition);

			if (suggestions.Count > 0)
			{
				foreach (var s in suggestions)
				{
					Console.WriteLine(s);
				}
			}

			//var suggestionJSON = service.SuggestToJSON(code, cursorPosition);
			//Console.WriteLine("Suggestions (JSON): ");
			//Console.WriteLine(suggestionJSON);
		}

		static void PrintHelp()
		{
			Console.WriteLine("Usage: CSharpCodeCompleteClient <filename> <cursor>");
			Console.WriteLine("\t<filename> - Name of the code file to open.");
			Console.WriteLine("\t<cursor>   - Position of the cursor in the code file buffer.");
		}
	}
}
