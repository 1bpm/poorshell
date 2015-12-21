using System;
using System.IO;
using System.Management.Automation;
using System.Collections.ObjectModel;

namespace powerShellExecutor {
	public class PS {
		string script;
		string file;
		string[] args;
		PowerShell instance;
		
		public PS(string[] args) {
			this.args=args;
			Run();
		}
		
		void Run() {
			if (args.Length<1) {
				Fatal("No script name supplied.");
			}
			file=args[0];
			if (!File.Exists(file)) {
				Fatal("Script file does not exist");
			}
			script=File.ReadAllText(args[0]);
			RunScript();
		}
		
		void Fatal(string message) {
			Console.WriteLine(message);
			Console.WriteLine("Usage: psExec <script name>");
			Environment.Exit(0);
		}
		
		void ProcessOutput(Collection<PSObject> output) {
			if (instance.Streams.Error.Count>0) {
				Console.WriteLine("Execution errors: ");
				foreach (ErrorRecord error in instance.Streams.Error) {
					Console.WriteLine(error.ToString());
				}
			}
			if (instance.Streams.Warning.Count>0) {
				Console.WriteLine("Execution warnings: ");
				foreach (WarningRecord warn in instance.Streams.Warning) {
					Console.WriteLine(warn.ToString());
				}
			}
			foreach (PSObject outs in output) {
				Console.WriteLine(outs.ToString());
			}
		}
		
		void RunScript() {
			using (instance = PowerShell.Create()) {
				instance.AddScript(script);
				try {
					Collection<PSObject> output=instance.Invoke();
					ProcessOutput(output);
				} catch (Exception ex) {
					Console.WriteLine("Error with script:");
					Console.Write(ex.ToString());
				}
			}
		}
	}
}
