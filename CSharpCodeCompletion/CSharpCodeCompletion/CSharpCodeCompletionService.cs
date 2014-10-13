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
		public List<string> Suggest(string code, int position)
		{
			code = code.Trim();

			if (string.IsNullOrEmpty(code))
			{
				return new List<string>();
			}

			var syntaxTree = CSharpSyntaxTree.ParseText(code);
			var compilation = CSharpCompilation.Create("MyCompilation")
				.AddSyntaxTrees(syntaxTree)
				.AddReferences(MetadataReference.CreateFromAssembly(typeof(object).Assembly));

			var semanticModel = compilation.GetSemanticModel(syntaxTree);
			var token = syntaxTree.GetRoot().FindToken(position);

			var parentNode = token.Parent;

			ExpressionSyntax identifierNode = parentNode as ExpressionSyntax;                    
			if (parentNode is MemberAccessExpressionSyntax)
			{
				identifierNode = parentNode.ChildNodes().FirstOrDefault() as ExpressionSyntax;
			}

			if (identifierNode == null)
				identifierNode = parentNode as LiteralExpressionSyntax;

			if (identifierNode == null)
				identifierNode = parentNode as ParenthesizedExpressionSyntax;

			if (identifierNode == null)
				identifierNode = parentNode.Parent as InvocationExpressionSyntax;

			if (identifierNode == null)
				identifierNode = parentNode.Parent as ObjectCreationExpressionSyntax;

			// TODO: Find keywords!

			if (identifierNode == null)
			{
				return new List<string>();
			}

			var semanticInfo = semanticModel.GetTypeInfo(identifierNode);

			var symbols = semanticModel.LookupSymbols(position, semanticInfo.Type);

			var suggestions = new List<string>();
			foreach (var symbol in symbols)
			{
				if (symbol.CanBeReferencedByName == false
				    || symbol.DeclaredAccessibility != Accessibility.Public
				    || symbol.IsStatic)
					continue;

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

				if (suggestions.Contains(result) == false)
					suggestions.Add(result);
			}
			suggestions.Sort();

			return suggestions;
		}

		public string SuggestToJSON(string code, int position)
		{
			var suggestions = Suggest(code, position);

			var serializer = new DataContractJsonSerializer(typeof(List<string>));
			var ms = new MemoryStream();
			serializer.WriteObject(ms, suggestions);
			ms.Position = 0;
			var sr = new StreamReader(ms);

			var result = sr.ReadToEnd();
			sr.Close();
			ms.Close();

			return result;
		}
	}
}

