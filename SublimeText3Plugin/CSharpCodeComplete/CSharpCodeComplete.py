import sublime, sublime_plugin
import subprocess

class CSharpCodeCompleteCommand(sublime_plugin.TextCommand):
	def run(self, edit):
		def done(index):
			if index != -1:
				print("Inserting " + results[index])
				self.view.insert(edit, sel.begin(), results[index])

		sel = self.view.sel()[0]

		p = subprocess.Popen(["mono", r"/Users/matteom/Library/Application Support/Sublime Text 3/Packages/CSharpCodeComplete/Service/CSharpCodeCompleteClient.exe", self.view.substr(sublime.Region(0, self.view.size())), str(sel.begin())], stdout=subprocess.PIPE, stderr=subprocess.PIPE)
		out, err = p.communicate()
		p.wait()

		if err:
			print("Error: " + err.decode('utf-8'))
			return

		results = out.decode("utf-8").rsplit('\n')
		results = [s for s in results if s]
		if not results:
			results.append("No results found.")

		self.view.show_popup_menu(results, done)
		
