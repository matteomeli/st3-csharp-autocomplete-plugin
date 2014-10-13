import sublime, sublime_plugin
import os
import subprocess

class CSharpCodeCompleteCommand(sublime_plugin.TextCommand):
	def run(self, edit):
		def done(index):
			if index != -1:
				print(self.results[index])

		sel = self.view.sel()[0]
		p = subprocess.Popen(["mono", r"/Users/matteomeli/Library/Application Support/Sublime Text 3/Packages/CSharpCodeComplete/Service/CSharpCodeCompleteClient.exe", self.view.substr(sublime.Region(0, self.view.size())), str(sel.begin())], stdout=subprocess.PIPE, stderr=subprocess.PIPE)
		out, err = p.communicate()
		self.results = out.decode("utf-8").split('\n')
		self.view.show_popup_menu(self.results, done)
		
