// author: Jan Horesovsky

using System;
using System.Windows.Forms;
using System.Threading;

namespace meshDiff
{
	public partial class ProgressDialog : Form
	{
		int tasksCompleted;
		int totalTasks;

		// info messages are displayed directly in the dialog
		// this variable stores the previous version of the text, so that the latest modification can be rewritten
		string previousInfoText;

		CancellationTokenSource cts;

		public bool CanBeClosed { get; set; }

		public ProgressDialog()
		{
			InitializeComponent();

			tasksCompleted = 0;
			totalTasks = 100;

			previousInfoText = "";

			CanBeClosed = false;

			cts = new CancellationTokenSource();

			FormClosing += (object sender, FormClosingEventArgs e) => 
			{
				e.Cancel = !CanBeClosed;
				cts.Cancel();
			};
		}

		/// <summary>
		/// Sets the total number of tasks which determines the maximum value of the progress bar.
		/// </summary>
		public void SetTotalTasks(int value)
		{
			if (totalTasks < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(totalTasks) + " has to be a positive integer.");
			}

			totalTasks = value;

			if (progressBar.InvokeRequired)
			{
				progressBar.BeginInvoke(new Action(
					() => { progressBar.Maximum = totalTasks; }
				));
			}
			else
			{
				progressBar.Maximum = totalTasks;
			}
		}

		/// <summary>
		/// Generates a UI event to increment the progress bar by one.
		/// </summary>
		public void IncrementProgressBarValue(int step = 1)
		{
			tasksCompleted += step;

			if (progressBar.InvokeRequired)
			{
				progressBar.Invoke(new Action(
					() => { progressBar.Value += step; }
				));
			}
			else
			{
				progressBar.Value += step;
			}
		}

		/// <summary>
		/// Appends a string to the text box.
		/// </summary>
		/// <param name="text">String to be appended</param>
		/// <param name="async">Specifies whether the thread should wait until the text is appended and displayed 
		///		in the UI thread (false) or not (true). It is irrelevant for calls which come from the UI thread.</param>
		/// <param name="updateLastLine">Specifies whether text should replace the last line (true) or not (false).</param>
		public void AddInfoMessage(string text, bool async = false, bool updateLastLine = false)
		{
			if (!updateLastLine)
			{
				previousInfoText = infoTextBox.Text;
			}

			if (infoTextBox.InvokeRequired)
			{
				var action = new Action(() =>
					{
						if (updateLastLine)
						{
							infoTextBox.Text = previousInfoText + "\r\n" + text;
						}
						else
						{
							infoTextBox.AppendText("\r\n" + text);
						}
					}
				);

				if (async)
				{
					infoTextBox.BeginInvoke(action);
				}
				else
				{
					infoTextBox.Invoke(action);
				}
				
			}
			else
			{
				if (updateLastLine)
				{
					infoTextBox.Text = previousInfoText + "\r\n" + text;
				}
				else
				{
					infoTextBox.AppendText("\r\n" + text);
				}
			}
		}

		/// <summary>
		/// Encapsulates AddInfoMessage() by highlighting the error message and adding information on how to continue after the error.
		/// </summary>
		public void ShowErrorMessage(string message, bool async = false, bool updateLastLine = false)
		{
			string errorMessage = "---> " + message + "<---";
			string finalMessage = "Close this window to continue...";

			AddInfoMessage(errorMessage + "\r\n" + finalMessage, async, updateLastLine);
		}

		/// <summary>
		/// Closes the form from any thread.
		/// </summary>
		public void CloseSafe()
		{
			CanBeClosed = true;

			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action(
					() => this.Close()	
				));
			}
			else
			{
				this.Close();
			}
		}

		public bool IsCancellationRequested()
		{
			return cts.IsCancellationRequested;
		}
	}
}
