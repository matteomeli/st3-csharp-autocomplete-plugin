import sublime, sublime_plugin

import os
import threading
import subprocess

class CSharpRoslynAutoCompleteCommand(sublime_plugin.TextCommand):

	def run(self, edit):
		sel = self.view.sel()[0]
		stringData = self.view.substr( sublime.Region( 0, self.view.size()) ), str(sel.begin() )

		thread = threading.Thread( target=RunCodeComplete, args=( stringData, self ) )
		thread.start()

def RunCodeComplete( stringData, codeCompleteCommand ):

	def done(index):
			if index != -1:
				print("Inserting " + results[index])
				codeCompleteCommand.view.insert(edit, sel.begin(), results[index])

	command = "mono"
	executable = r"~/Library/Application Support/Sublime Text 3/Packages/CSharpRoslynAutoComplete/CSharpRoslynAutoCompleteClient.exe"
	executable = os.path.expanduser( executable )
	code = stringData[0]
	cursor = stringData[1]
	
	p = subprocess.Popen([command, executable, code, cursor], stdout=subprocess.PIPE, stderr=subprocess.PIPE)
	out, err = p.communicate()
	p.wait()

	if err:
		print("Error: " + err.decode('utf-8'))
		return

	results = out.decode("utf-8").rsplit('\n')
	results = [s for s in results if s]
	if not results:
		results.append("No results found.")

	codeCompleteCommand.view.show_popup_menu(results, done)