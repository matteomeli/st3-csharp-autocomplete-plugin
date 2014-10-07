using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpCodeCompletion
{
	public class CSharpCodeCompletionService
	{
		public List<string> Suggest(string code, int position, int line)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(code);
			var compilation = CSharpCompilation.Create("MyCompilation")
				.AddSyntaxTrees(syntaxTree)
				.AddReferences(MetadataReference.CreateFromAssembly(typeof(object).Assembly));

			var semanticModel = compilation.GetSemanticModel(syntaxTree);

			// Find suggestions
			var p = syntaxTree.GetRoot().FindToken(position).Parent;
			ExpressionSyntax identifier = p as ExpressionSyntax;
			if (p is MemberAccessExpressionSyntax)
			{
				identifier = p.ChildNodes().FirstOrDefault() as ExpressionSyntax;
			}

			if (identifier == null)
				identifier = p as LiteralExpressionSyntax;

			if (identifier == null)
				identifier = p as ParenthesizedExpressionSyntax;

			if (identifier == null)
				identifier = p.Parent as InvocationExpressionSyntax;

			if (identifier == null)
				identifier = p.Parent as ObjectCreationExpressionSyntax;

			var semanticInfo = semanticModel.GetTypeInfo(identifier);
			var type = semanticInfo.Type;

			var symbols = semanticModel
				.LookupSymbols(
				              position, 
				              type
			              );

			List<string> suggestions = new List<string>();
			foreach (var symbol in symbols)
			{
				var result = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
				if (symbol.Kind == SymbolKind.Method)
				{
					result = result.Substring(result.IndexOf(" ", StringComparison.InvariantCulture));	// Cut return type
				}
				result = result.Substring(result.IndexOf(".", StringComparison.InvariantCulture) + 1);	// Cut containing type
				if (symbol.Kind == SymbolKind.Method)
				{
					var prefix = result.Substring(0, result.IndexOf("(", StringComparison.InvariantCulture));	// Cut redundant generic info
					prefix = prefix.Substring(0, prefix.IndexOf("<", StringComparison.InvariantCulture) == -1 ? prefix.Length : prefix.IndexOf("<"));
					result = prefix + result.Substring((result.IndexOf("(", StringComparison.InvariantCulture)));
				}
				if (result.Length > 100)
				{
					result = result.Substring(0, 100) + "...";
					if (symbol.Kind == SymbolKind.Method)
						result += ")";
				}
				if (suggestions.Contains(result) == false)
					suggestions.Add(result);
			}
			suggestions.Sort();

			return suggestions;
		}

		public string SuggestToJSON(string code, int position, int line)
		{
			var suggestions = Suggest(code, position, line);

			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<string>));
			MemoryStream ms = new MemoryStream();
			serializer.WriteObject(ms, suggestions);
			ms.Position = 0;
			StreamReader sr = new StreamReader(ms);

			var result = sr.ReadToEnd();
			sr.Close();
			ms.Close();

			return result;
		}
	}
}

