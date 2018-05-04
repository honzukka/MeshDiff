namespace meshDiff
{
	partial class MetricsEditForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.distanceRadioButton = new System.Windows.Forms.RadioButton();
			this.projectedDistanceRadioButton = new System.Windows.Forms.RadioButton();
			this.saveButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// distanceRadioButton
			// 
			this.distanceRadioButton.AutoSize = true;
			this.distanceRadioButton.Location = new System.Drawing.Point(40, 39);
			this.distanceRadioButton.Name = "distanceRadioButton";
			this.distanceRadioButton.Size = new System.Drawing.Size(67, 17);
			this.distanceRadioButton.TabIndex = 0;
			this.distanceRadioButton.TabStop = true;
			this.distanceRadioButton.Text = "Distance";
			this.distanceRadioButton.UseVisualStyleBackColor = true;
			// 
			// projectedDistanceRadioButton
			// 
			this.projectedDistanceRadioButton.AutoSize = true;
			this.projectedDistanceRadioButton.Location = new System.Drawing.Point(40, 70);
			this.projectedDistanceRadioButton.Name = "projectedDistanceRadioButton";
			this.projectedDistanceRadioButton.Size = new System.Drawing.Size(151, 17);
			this.projectedDistanceRadioButton.TabIndex = 1;
			this.projectedDistanceRadioButton.TabStop = true;
			this.projectedDistanceRadioButton.Text = "Normal-Projected Distance";
			this.projectedDistanceRadioButton.UseVisualStyleBackColor = true;
			// 
			// saveButton
			// 
			this.saveButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.saveButton.Location = new System.Drawing.Point(111, 146);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(63, 29);
			this.saveButton.TabIndex = 2;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// MetricsEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 187);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.projectedDistanceRadioButton);
			this.Controls.Add(this.distanceRadioButton);
			this.MinimumSize = new System.Drawing.Size(300, 226);
			this.Name = "MetricsEditForm";
			this.Text = "MeshDiff - Metrics";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton distanceRadioButton;
		private System.Windows.Forms.RadioButton projectedDistanceRadioButton;
		private System.Windows.Forms.Button saveButton;
	}
}