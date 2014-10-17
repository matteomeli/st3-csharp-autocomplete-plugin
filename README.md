Sublime Text CSharp Roslyn AutoComplete
==================

The starting point for a Roslyn-based Mono-powered C# code completion plugin for Sublime Text 3.

Installation
==================

1. Copy the content of the "Plugin" folder into your ST3 "Packages" folder.
2. Add into your ST3 keymap file a key shortcut to invoke the plugin. An example is `{ "keys": ["ctrl+shift+space"], "command": "c_sharp_code_complete"}`.
3. Open the CSharpRoslynAutoComplete.sublime-settings file under the "User" folder and add custom C# assemblies you want to load, e.g.:
```json
{
	"assemblyPaths":
	[
		"/Applications/AnApp/AnApp.app/Contents/Bin/an_app.dll"
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
3. Add unit tests.
4. Add syntax checking.
5. Make auto-complete popup handle better filtering and colours (more like the Sublime built-in auto-complete plugin).

License
==================
All of Sublime Text CSharp Roslyn AutoComplete plugin is licensed under the MIT License (MIT).

Copyright (c) <2014> Matteo Meli <matteo.meli@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
