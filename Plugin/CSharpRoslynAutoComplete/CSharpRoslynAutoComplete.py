import sublime, sublime_plugin

import os
import subprocess
import threading

class CSharpRoslynAutoCompleteCommand(sublime_plugin.TextCommand):

	def run(self, edit):
		assemblyPaths = sublime.load_settings('CSharpRoslynAutoComplete.sublime-settings').get('assemblyPaths')
		code = self.view.substr(sublime.Region(0, self.view.size()))
		cursor = self.view.sel()[0].begin()

		thread = CSharpRoslynAutoCompleteCall(code, cursor, assemblyPaths)
		thread.start()

		self.handle_thread(edit, thread)

	def handle_thread(self, edit, thread, i = 0, dir = 1):
		if thread.is_alive():
			# This animates a little activity indicator in the status area
			before = i % 8
			after = (7) - before
			if not after:
				dir = -1
			if not before:
				dir = 1
			i += dir
			self.view.set_status('csharp-roslyn-auto-complete', 'CSharpRoslynAutoComplete [%s=%s]' % \
				(' ' * before, ' ' * after))

			sublime.set_timeout(lambda: self.handle_thread(edit, thread, i, dir), 100)
			return

		self.view.erase_status('csharp-roslyn-auto-complete')	

		if thread.result:
			self.suggest(edit, thread)
		else:
			sublime.status_message('CSharpRoslynAutoComplete could not find any syggestion.')

	def suggest(self, edit, thread):
		def done(index):
			if index >= 0:
				sublime.status_message('CSharpRoslynAutoComplete successfully suggested completion %s%s.' % 
					(self.view.substr(self.view.line(thread.cursor)).strip(), thread.result[index]))
				self.view.run_command("insert", {"characters": thread.result[index]})
		
		self.view.show_popup_menu(thread.result, done)

class CSharpRoslynAutoCompleteCall(threading.Thread):

	def __init__(self, code, cursor, assemblyPaths):
		self.code = code
		self.cursor = cursor
		self.result = None
		self.pp = subprocess.PIPE
		self.cwd = os.path.dirname(__file__)
		self.command = 'mono'
		self.executable = 'CSharpRoslynAutoCompleteClient.exe'
		self.assemblyPaths = assemblyPaths
		threading.Thread.__init__(self)

	def run(self):
		commandLineArgs = [self.command, self.executable, "-p", self.code, "-c", str(self.cursor), "-d"]
		for ap in self.assemblyPaths:
			commandLineArgs.append(ap)

		p = subprocess.Popen(commandLineArgs, cwd = self.cwd, stdout = self.pp, stderr = self.pp)
		o, e = p.communicate()
		p.wait()

		if e:
			err = 'Error: %s' % e.decode('utf-8')
			sublime.error_message(err)
			self.result = False
			return

		suggestions = o.decode("utf-8").rsplit('\n')
		self.result = [s for s in suggestions if s]
