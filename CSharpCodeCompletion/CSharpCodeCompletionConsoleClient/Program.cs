using System;
using CSharpCodeCompletion;

namespace CSharpCodeCompletionConsoleClient
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Roslyn C# Code Completion Console Client says: 'Hello World!'");

			const string code = @"
using System;
public class Test
{
    public void TestMethod()
    {
        var now = DateTime.Now;
        now.
    }
}";

			// In the future the cursor position will come from the command line 
			// (from SublimeText for example) as the code string as well:
			// E.g.: public string Suggest(string code, int position, int line)
			// For now simulate the cursor location at the last dot.
			int cursorPosition = code.LastIndexOf(".", StringComparison.InvariantCulture);

			Console.WriteLine(code);
			Console.WriteLine();

			CSharpCodeCompletionService service = new CSharpCodeCompletionService();
			var suggestions = service.Suggest(code, cursorPosition);

			Console.WriteLine("Suggestions: ");
			foreach (var s in suggestions)
			{
				Console.WriteLine(s);
			}
			Console.WriteLine();

			//var suggestionJSON = service.SuggestToJSON(code, cursorPosition);
			//Console.WriteLine("Suggestions (JSON): ");
			//Console.WriteLine(suggestionJSON);
		}
	}
}
