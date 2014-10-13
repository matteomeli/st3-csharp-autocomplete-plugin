st3-csharp-codecompletion
==================

The starting point for a Roslyn-based Mono-powered C# code completion plugin for Sublime Text 3.

Installation
==================

1. Copy folder CSharpCodeComplete under SublimeText3Plugin folder into your ST3 Packages folder.
2. Open the file CSharpCodeComplete.py and change the line `executable = r"/Users/matteom/Library/Application Support/Sublime Text 3/Packages/CSharpCodeComplete/Service/CSharpCodeCompleteClient.exe"` to your local path. Sorry. ;)
3. Add into your ST3 keymap file the shortcut `{ "keys": ["ctrl+shift+space"], "command": "c_sharp_code_complete"}`

Uasage
==================

Open a .cs file and use the shortcut. Currently it works only for member access syntax (i.e. after a dot token).