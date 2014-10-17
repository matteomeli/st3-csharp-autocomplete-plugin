st3-csharp-codecompletion
==================

The starting point for a Roslyn-based Mono-powered C# code completion plugin for Sublime Text 3.

Installation
==================

1. Copy the content of the "Plugin" folder into your ST3 "Packages" folder.
2. Add into your ST3 keymap file a key shortcut to invoke the plugin. An example is `{ "keys": ["ctrl+shift+space"], "command": "c_sharp_code_complete"}`.
3. Open the CSharpRoslynAutoComplete.sublime-settings file under the "User" folder and add custom C# assemblies you want to load, e.g.
```json
{
	"assemblyPaths":
	[
		"/Applications/AwesomeApp/AwesomeApp.app/Contents/Frameworks/Managed/awesome_app.dll"
	]
}
```

Usage
==================

Open a .cs file and use the key shortcut of your choice. Currently it works only for member access syntax (i.e. after a dot token).

Todo
==================

1. Support other C# syntax kinds in the parser.
2. Add delay real-time suggestions popup (not only on invocation) to the plugin.