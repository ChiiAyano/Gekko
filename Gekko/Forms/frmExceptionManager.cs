using System;
using System.Net;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace Gekko.Forms
{
	public partial class frmExceptionManager : Form
	{
		Exception exep;
		public frmExceptionManager(Exception ex)
		{
			InitializeComponent();
			this.exep = ex;
			this.Text = exep.GetType().ToString();
		}

		private void frmExceptionManager_Load(object sender, EventArgs e)
		{
			string expName = exep.GetType().ToString();
			string message = string.Empty;
			ComputerInfo ci = new ComputerInfo();

			// 例外の種類によって表示を変更
			switch (expName)
			{
				case "System.Web.WebException":
					WebException we = ((WebException)exep);
					hintLabel.Text =Properties.Resources.ExceptionManagerWebExceptionHint;
					message = string.Format(Properties.Resources.ExceptionManagerGeneralStacktrace, expName, we.Message, we.StackTrace);
					break;
				case "System.ApplicationException":
					hintLabel.Text = Properties.Resources.ExceptionManagerApplicationExceptionHint;
					message = string.Format(Properties.Resources.ExceptionManagerGeneralStacktrace, expName, exep.Message, exep.StackTrace);
					break;
				default:
					hintLabel.Text = Properties.Resources.ExceptionManagerDefaultHint;
					message = string.Format(Properties.Resources.ExceptionManagerGeneralStacktrace, expName, exep.Message, exep.StackTrace);
					break;
			}

			message = message.Insert(message.Length, string.Format("\r\n" + Properties.Resources.ExceptionManagerStacktraceEnd,
				Application.ProductVersion, ci.OSFullName, Environment.OSVersion.ServicePack));

			// テキストボックスに投入
			stackTraceBox.Text = message;

			// 説明を記述
			exceptionLabel.Text =
				expName + Properties.Resources.ExceptionManagerMessage;
		}

		private void clipBoardButton_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(stackTraceBox.Text);
			MessageBox.Show(Properties.Resources.ExceptionManagerClipboard, Properties.Resources.SoftwareName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void exitButton_Click(object sender, EventArgs e)
		{
			if (resumeCheck.Checked)
			{
				this.Close();
				Program.SystemShutdown = true;
				Application.Restart();
			}
			else
			{
				this.Close();
				Program.SystemShutdown = true;
				Application.Exit();
			}
		}
	}
}
