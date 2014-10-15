using System;
using System.IO;
using System.Text;

namespace CSharpRoslynAutoCompleteClient
{
	public class Logger
	{
		static TextWriter standardOutput;
		static TextWriter standardError;

		public Logger()
		{
			Initiliaze();
		}

		public static TextWriter Out
		{
			get
			{
				return standardOutput;
			}
		}

		public static TextWriter Error
		{
			get
			{
				return standardError;
			}
		}

		private static void Initiliaze()
		{
			if (standardOutput == null)
				standardOutput = new StreamWriter(Console.OpenStandardOutput());

			if (standardError == null)
				standardError = new StreamWriter(Console.OpenStandardError());

			Console.SetOut(Out);
			Console.SetError(Error);
		}

		public void Log(string value = "")
		{
			standardOutput.WriteLine(value);
		}

		public void LogError(string value = "")
		{
			standardError.WriteLine(value);
		}
	}
}
