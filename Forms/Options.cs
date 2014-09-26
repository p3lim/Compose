using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compose
{
	public partial class Options : Form
	{
		private bool frozen;

		public Options()
		{
			InitializeComponent();

			Shown += Options_Shown;
		}

		public void UpdateDetails()
		{
			if (Settings.GetModifier() > 0)
				bindButton.Text = String.Format("Hotkey: {0}", (Keys)Settings.GetModifier());
			else
				bindButton.Text = "Hotkey: None";
		}

		void Options_Shown(object sender, EventArgs e)
		{
			frozen = true;
			bootCheck.Checked = Settings.ShouldAutoRun();
			scrollToggle.Checked = Settings.ShouldIndicate();
			frozen = false;

			UpdateDetails();
		}

		private void bindButton_Click(object sender, EventArgs e)
		{
			var register = new Register();
			register.ShowDialog(this);
		}

		private void bootCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (!frozen)
				Settings.SetShouldAutoRun(bootCheck.Checked);
		}

		private void scrollToggle_CheckedChanged(object sender, EventArgs e)
		{
			if (!frozen)
				Settings.SetShouldIndicate(scrollToggle.Checked);
		}
	}
}
