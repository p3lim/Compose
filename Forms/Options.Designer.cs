namespace Compose
{
	partial class Options
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
			this.bindButton = new System.Windows.Forms.Button();
			this.bootCheck = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// bindButton
			// 
			this.bindButton.Location = new System.Drawing.Point(12, 12);
			this.bindButton.Name = "bindButton";
			this.bindButton.Size = new System.Drawing.Size(222, 32);
			this.bindButton.TabIndex = 0;
			this.bindButton.Text = "bindButton";
			this.bindButton.UseVisualStyleBackColor = true;
			this.bindButton.Click += new System.EventHandler(this.bindButton_Click);
			// 
			// bootCheck
			// 
			this.bootCheck.AutoSize = true;
			this.bootCheck.Location = new System.Drawing.Point(13, 50);
			this.bootCheck.Name = "bootCheck";
			this.bootCheck.Size = new System.Drawing.Size(117, 17);
			this.bootCheck.TabIndex = 1;
			this.bootCheck.Text = "Start with Windows";
			this.bootCheck.UseVisualStyleBackColor = true;
			this.bootCheck.CheckedChanged += new System.EventHandler(this.bootCheck_CheckedChanged);
			// 
			// Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(248, 73);
			this.Controls.Add(this.bootCheck);
			this.Controls.Add(this.bindButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Options";
			this.Text = "Compose";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button bindButton;
		private System.Windows.Forms.CheckBox bootCheck;
	}
}