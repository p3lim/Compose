namespace Compose
{
	partial class Register
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
			this.acceptButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// acceptButton
			// 
			this.acceptButton.Location = new System.Drawing.Point(44, 56);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 0;
			this.acceptButton.Text = "Accept";
			this.acceptButton.UseVisualStyleBackColor = true;
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(129, 56);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// clearButton
			// 
			this.clearButton.Location = new System.Drawing.Point(44, 56);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(75, 23);
			this.clearButton.TabIndex = 2;
			this.clearButton.Text = "Clear";
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label.Location = new System.Drawing.Point(12, 13);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(34, 15);
			this.label.TabIndex = 3;
			this.label.Text = "label";
			// 
			// Register
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(246, 91);
			this.ControlBox = false;
			this.Controls.Add(this.label);
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.acceptButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Register";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Label label;
	}
}