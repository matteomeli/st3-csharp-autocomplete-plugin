st3-csharp-codecompletion
==================

The starting point for a Roslyn-based Mono-powered C# code completion plugin for Sublime Text 3.

Installation
==================

1. Copy folder CSharpCodeComplete under SublimeText3Plugin folder into your ST3 Packages folder.
2. Add into your ST3 keymap file a key shortcut to invoke the plugin. An example is `{ "keys": ["ctrl+shift+space"], "command": "c_sharp_code_complete"}`. 

Usage
==================

Open a .cs file and use the key shortcut of your choice. Currently it works only for member access syntax (i.e. after a dot token).