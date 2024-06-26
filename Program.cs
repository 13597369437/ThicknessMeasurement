using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThicknessMeasurement
{
	internal static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			if (IsRunning())
			{
				MessageBox.Show("应用程序已经在运行中,无法重复启动。", "错误",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Main());
		}
		static bool IsRunning()
		{
			Process currentProcess = Process.GetCurrentProcess();
			Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
			return (processes.Length > 1);
		}
	}
}
