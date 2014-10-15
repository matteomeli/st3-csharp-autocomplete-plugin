using System;
using System.Collections.Generic;
using CSharpRoslynAutoComplete;

namespace CSharpCodeCompletionConsoleClient
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Roslyn C# Code Completion Console Client says: 'Hello World!'");

			const string code = @"
using System;
using UnityEngine;
public class Test
{
    public void TestMethod()
    {
        var now = DateTime.Now;
        now.
    }
}";
			// Simulate the cursor location at the last dot.
			int cursorPosition = code.LastIndexOf(".", StringComparison.InvariantCulture);

			Console.WriteLine(code);
			Console.WriteLine();

			var prompter = new CSharpPrompter();
			var suggestions = prompter.Prompt(code, cursorPosition, new List<string>());

			foreach (var s in suggestions)
			{
				Console.WriteLine(s);
			}

			// TODO: Allow moving cursor from command line + interface for it
		}
	}
}
