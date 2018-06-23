using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SongsCopier {
	class Program {
		static int count = 0;
		static void Main(string[] args) {
			DirectoryInfo sour = new DirectoryInfo(args[0]);
			DirectoryInfo dest = new DirectoryInfo(args[1]);
			dest.Create();
			StartCopy(sour,dest,args[2]);
		}
		private static void StartCopy(DirectoryInfo sour,DirectoryInfo dest,string pattern = "*.wav") {
			DirectoryInfo[] folders = sour.GetDirectories();
			foreach(DirectoryInfo folder in folders) {
				try {
					StartCopy(folder,dest,pattern);
				}catch(Exception ex) {
					Report(ex,folder.FullName);
				}
			}
			FileInfo[] files = sour.GetFiles(pattern);
			if(files.Length>0) {
				foreach(FileInfo file in files) {
					try {
						FileInfo copy = new FileInfo(Path.Combine(dest.FullName,file.Name));
						if(!copy.Exists) {
							try {
								file.CopyTo(copy.FullName);
								Report(file,copy);
							}catch(Exception ex) {
								Report(ex,file.FullName);
							}
						} else {
							ReportExists(copy,ref count);
						}
					}catch(Exception ex) {
						Report(ex,file.FullName);
					}
				}
			}
		}
		private static void ReportExists(FileInfo copy,ref int count) {
			string text = String.Format(@"{1,6} | Exists ""{0}""",copy.FullName,++count);
			Console.WriteLine(text);
		}
		private static void Report(Exception ex,string name) {
			Console.ForegroundColor=ConsoleColor.Red;
			Console.WriteLine("{1} | {0}",ex.Message,name);
			Console.ResetColor();
		}
		private static void Report(FileInfo file,FileInfo copy) => 
			Console.WriteLine("{2:ddMMMyyyy HH:mm:ss.fff}|{0}|{1}",file.FullName,copy.FullName,DateTime.Now);
	}
}