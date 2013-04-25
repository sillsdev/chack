namespace SampleApp
{
	partial class GeckoTestDlg
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
			if (disposing)
			{
				if (components != null)
					components.Dispose();
				if (_timer != null)
				{
					_timer.Stop();
					_timer.Tick -= OnTimerTick;
					_timer.Dispose();
				}
			}
			components = null;
			_timer = null;
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // GeckoTestDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 461);
            this.Name = "GeckoTestDlg";
            this.Text = "GeckoTestDlg";
            this.ResumeLayout(false);

		}

		#endregion
	}
}
