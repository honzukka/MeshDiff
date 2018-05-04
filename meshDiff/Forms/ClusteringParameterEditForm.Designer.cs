namespace meshDiff
{
	partial class ClusteringParameterEditForm
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
			this.clusteringErrorGroupBox = new System.Windows.Forms.GroupBox();
			this.resolutionSignificanceValueLabel = new System.Windows.Forms.Label();
			this.resolutionSignificanceTrackBar = new System.Windows.Forms.TrackBar();
			this.resolutionSignificanceLabel = new System.Windows.Forms.Label();
			this.positionSignificanceValueLabel = new System.Windows.Forms.Label();
			this.positionSignificanceTrackBar = new System.Windows.Forms.TrackBar();
			this.positionSignificanceLabel = new System.Windows.Forms.Label();
			this.magnitudeSignificanceTrackBar = new System.Windows.Forms.TrackBar();
			this.magnitudeSignificanceValueLabel = new System.Windows.Forms.Label();
			this.magnitudeSignificanceLabel = new System.Windows.Forms.Label();
			this.directionSignificanceValueLabel = new System.Windows.Forms.Label();
			this.directionSignificanceTrackBar = new System.Windows.Forms.TrackBar();
			this.directionSignificanceLabel = new System.Windows.Forms.Label();
			this.saveButton = new System.Windows.Forms.Button();
			this.arrowsRadioButton = new System.Windows.Forms.RadioButton();
			this.colorsRadioButton = new System.Windows.Forms.RadioButton();
			this.clusteringErrorGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.resolutionSignificanceTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.positionSignificanceTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.magnitudeSignificanceTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.directionSignificanceTrackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// clusteringErrorGroupBox
			// 
			this.clusteringErrorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.clusteringErrorGroupBox.Controls.Add(this.resolutionSignificanceValueLabel);
			this.clusteringErrorGroupBox.Controls.Add(this.resolutionSignificanceTrackBar);
			this.clusteringErrorGroupBox.Controls.Add(this.resolutionSignificanceLabel);
			this.clusteringErrorGroupBox.Controls.Add(this.positionSignificanceValueLabel);
			this.clusteringErrorGroupBox.Controls.Add(this.positionSignificanceTrackBar);
			this.clusteringErrorGroupBox.Controls.Add(this.positionSignificanceLabel);
			this.clusteringErrorGroupBox.Controls.Add(this.magnitudeSignificanceTrackBar);
			this.clusteringErrorGroupBox.Controls.Add(this.magnitudeSignificanceValueLabel);
			this.clusteringErrorGroupBox.Controls.Add(this.magnitudeSignificanceLabel);
			this.clusteringErrorGroupBox.Controls.Add(this.directionSignificanceValueLabel);
			this.clusteringErrorGroupBox.Controls.Add(this.directionSignificanceTrackBar);
			this.clusteringErrorGroupBox.Controls.Add(this.directionSignificanceLabel);
			this.clusteringErrorGroupBox.Location = new System.Drawing.Point(12, 12);
			this.clusteringErrorGroupBox.Name = "clusteringErrorGroupBox";
			this.clusteringErrorGroupBox.Size = new System.Drawing.Size(682, 256);
			this.clusteringErrorGroupBox.TabIndex = 4;
			this.clusteringErrorGroupBox.TabStop = false;
			this.clusteringErrorGroupBox.Text = "Clustering Error";
			// 
			// resolutionSignificanceValueLabel
			// 
			this.resolutionSignificanceValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.resolutionSignificanceValueLabel.AutoSize = true;
			this.resolutionSignificanceValueLabel.Location = new System.Drawing.Point(605, 201);
			this.resolutionSignificanceValueLabel.Name = "resolutionSignificanceValueLabel";
			this.resolutionSignificanceValueLabel.Size = new System.Drawing.Size(163, 13);
			this.resolutionSignificanceValueLabel.TabIndex = 23;
			this.resolutionSignificanceValueLabel.Text = "resolutionSignificanceValueLabel";
			// 
			// resolutionSignificanceTrackBar
			// 
			this.resolutionSignificanceTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.resolutionSignificanceTrackBar.LargeChange = 10;
			this.resolutionSignificanceTrackBar.Location = new System.Drawing.Point(235, 201);
			this.resolutionSignificanceTrackBar.Maximum = 100;
			this.resolutionSignificanceTrackBar.Name = "resolutionSignificanceTrackBar";
			this.resolutionSignificanceTrackBar.Size = new System.Drawing.Size(345, 45);
			this.resolutionSignificanceTrackBar.TabIndex = 22;
			this.resolutionSignificanceTrackBar.Value = 25;
			this.resolutionSignificanceTrackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
			// 
			// resolutionSignificanceLabel
			// 
			this.resolutionSignificanceLabel.AutoSize = true;
			this.resolutionSignificanceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.resolutionSignificanceLabel.Location = new System.Drawing.Point(46, 201);
			this.resolutionSignificanceLabel.Name = "resolutionSignificanceLabel";
			this.resolutionSignificanceLabel.Size = new System.Drawing.Size(141, 13);
			this.resolutionSignificanceLabel.TabIndex = 21;
			this.resolutionSignificanceLabel.Text = "Resolution Significance";
			// 
			// positionSignificanceValueLabel
			// 
			this.positionSignificanceValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.positionSignificanceValueLabel.AutoSize = true;
			this.positionSignificanceValueLabel.Location = new System.Drawing.Point(605, 145);
			this.positionSignificanceValueLabel.Name = "positionSignificanceValueLabel";
			this.positionSignificanceValueLabel.Size = new System.Drawing.Size(135, 13);
			this.positionSignificanceValueLabel.TabIndex = 20;
			this.positionSignificanceValueLabel.Text = "gammaMultiplierValueLabel";
			// 
			// positionSignificanceTrackBar
			// 
			this.positionSignificanceTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.positionSignificanceTrackBar.LargeChange = 10;
			this.positionSignificanceTrackBar.Location = new System.Drawing.Point(235, 145);
			this.positionSignificanceTrackBar.Maximum = 100;
			this.positionSignificanceTrackBar.Name = "positionSignificanceTrackBar";
			this.positionSignificanceTrackBar.Size = new System.Drawing.Size(345, 45);
			this.positionSignificanceTrackBar.TabIndex = 19;
			this.positionSignificanceTrackBar.Value = 25;
			this.positionSignificanceTrackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
			// 
			// positionSignificanceLabel
			// 
			this.positionSignificanceLabel.AutoSize = true;
			this.positionSignificanceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.positionSignificanceLabel.Location = new System.Drawing.Point(46, 145);
			this.positionSignificanceLabel.Name = "positionSignificanceLabel";
			this.positionSignificanceLabel.Size = new System.Drawing.Size(126, 13);
			this.positionSignificanceLabel.TabIndex = 18;
			this.positionSignificanceLabel.Text = "Position Significance";
			// 
			// magnitudeSignificanceTrackBar
			// 
			this.magnitudeSignificanceTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.magnitudeSignificanceTrackBar.LargeChange = 10;
			this.magnitudeSignificanceTrackBar.Location = new System.Drawing.Point(235, 92);
			this.magnitudeSignificanceTrackBar.Maximum = 100;
			this.magnitudeSignificanceTrackBar.Name = "magnitudeSignificanceTrackBar";
			this.magnitudeSignificanceTrackBar.Size = new System.Drawing.Size(345, 45);
			this.magnitudeSignificanceTrackBar.TabIndex = 17;
			this.magnitudeSignificanceTrackBar.Value = 25;
			this.magnitudeSignificanceTrackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
			// 
			// magnitudeSignificanceValueLabel
			// 
			this.magnitudeSignificanceValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.magnitudeSignificanceValueLabel.AutoSize = true;
			this.magnitudeSignificanceValueLabel.Location = new System.Drawing.Point(605, 92);
			this.magnitudeSignificanceValueLabel.Name = "magnitudeSignificanceValueLabel";
			this.magnitudeSignificanceValueLabel.Size = new System.Drawing.Size(167, 13);
			this.magnitudeSignificanceValueLabel.TabIndex = 16;
			this.magnitudeSignificanceValueLabel.Text = "magnitudeSignificanceValueLabel";
			// 
			// magnitudeSignificanceLabel
			// 
			this.magnitudeSignificanceLabel.AutoSize = true;
			this.magnitudeSignificanceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.magnitudeSignificanceLabel.Location = new System.Drawing.Point(46, 92);
			this.magnitudeSignificanceLabel.Name = "magnitudeSignificanceLabel";
			this.magnitudeSignificanceLabel.Size = new System.Drawing.Size(140, 13);
			this.magnitudeSignificanceLabel.TabIndex = 15;
			this.magnitudeSignificanceLabel.Text = "Magnitude Significance";
			// 
			// directionSignificanceValueLabel
			// 
			this.directionSignificanceValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.directionSignificanceValueLabel.AutoSize = true;
			this.directionSignificanceValueLabel.Location = new System.Drawing.Point(605, 40);
			this.directionSignificanceValueLabel.Name = "directionSignificanceValueLabel";
			this.directionSignificanceValueLabel.Size = new System.Drawing.Size(158, 13);
			this.directionSignificanceValueLabel.TabIndex = 14;
			this.directionSignificanceValueLabel.Text = "directionSignificanceValueLabel";
			// 
			// directionSignificanceTrackBar
			// 
			this.directionSignificanceTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.directionSignificanceTrackBar.LargeChange = 10;
			this.directionSignificanceTrackBar.Location = new System.Drawing.Point(235, 40);
			this.directionSignificanceTrackBar.Maximum = 100;
			this.directionSignificanceTrackBar.Name = "directionSignificanceTrackBar";
			this.directionSignificanceTrackBar.Size = new System.Drawing.Size(345, 45);
			this.directionSignificanceTrackBar.TabIndex = 13;
			this.directionSignificanceTrackBar.Value = 25;
			this.directionSignificanceTrackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
			// 
			// directionSignificanceLabel
			// 
			this.directionSignificanceLabel.AutoSize = true;
			this.directionSignificanceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.directionSignificanceLabel.Location = new System.Drawing.Point(46, 40);
			this.directionSignificanceLabel.Name = "directionSignificanceLabel";
			this.directionSignificanceLabel.Size = new System.Drawing.Size(132, 13);
			this.directionSignificanceLabel.TabIndex = 12;
			this.directionSignificanceLabel.Text = "Direction Significance";
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(557, 282);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(81, 27);
			this.saveButton.TabIndex = 6;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// arrowsRadioButton
			// 
			this.arrowsRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.arrowsRadioButton.AutoSize = true;
			this.arrowsRadioButton.Location = new System.Drawing.Point(61, 287);
			this.arrowsRadioButton.Name = "arrowsRadioButton";
			this.arrowsRadioButton.Size = new System.Drawing.Size(157, 17);
			this.arrowsRadioButton.TabIndex = 7;
			this.arrowsRadioButton.TabStop = true;
			this.arrowsRadioButton.Text = "Arrow Clustering Parameters";
			this.arrowsRadioButton.UseVisualStyleBackColor = true;
			this.arrowsRadioButton.CheckedChanged += new System.EventHandler(this.arrowsRadioButton_CheckedChanged);
			// 
			// colorsRadioButton
			// 
			this.colorsRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.colorsRadioButton.AutoSize = true;
			this.colorsRadioButton.Location = new System.Drawing.Point(229, 287);
			this.colorsRadioButton.Name = "colorsRadioButton";
			this.colorsRadioButton.Size = new System.Drawing.Size(154, 17);
			this.colorsRadioButton.TabIndex = 8;
			this.colorsRadioButton.TabStop = true;
			this.colorsRadioButton.Text = "Color Clustering Parameters";
			this.colorsRadioButton.UseVisualStyleBackColor = true;
			this.colorsRadioButton.CheckedChanged += new System.EventHandler(this.colorsRadioButton_CheckedChanged);
			// 
			// ClusteringParameterEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(707, 338);
			this.Controls.Add(this.colorsRadioButton);
			this.Controls.Add(this.arrowsRadioButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.clusteringErrorGroupBox);
			this.MinimumSize = new System.Drawing.Size(562, 377);
			this.Name = "ClusteringParameterEditForm";
			this.Text = "Mesh Diff - Clustering Parameters";
			this.clusteringErrorGroupBox.ResumeLayout(false);
			this.clusteringErrorGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.resolutionSignificanceTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.positionSignificanceTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.magnitudeSignificanceTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.directionSignificanceTrackBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.GroupBox clusteringErrorGroupBox;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.TrackBar magnitudeSignificanceTrackBar;
		private System.Windows.Forms.Label magnitudeSignificanceValueLabel;
		private System.Windows.Forms.Label magnitudeSignificanceLabel;
		private System.Windows.Forms.Label directionSignificanceValueLabel;
		private System.Windows.Forms.TrackBar directionSignificanceTrackBar;
		private System.Windows.Forms.Label directionSignificanceLabel;
		private System.Windows.Forms.TrackBar positionSignificanceTrackBar;
		private System.Windows.Forms.Label positionSignificanceLabel;
		private System.Windows.Forms.Label positionSignificanceValueLabel;
		private System.Windows.Forms.Label resolutionSignificanceValueLabel;
		private System.Windows.Forms.TrackBar resolutionSignificanceTrackBar;
		private System.Windows.Forms.Label resolutionSignificanceLabel;
		private System.Windows.Forms.RadioButton arrowsRadioButton;
		private System.Windows.Forms.RadioButton colorsRadioButton;
	}
}