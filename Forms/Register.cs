using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
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
	public partial class Register : Form
	{
		public static bool active;

		private static Keys modifier;
		private KeyboardHookListener keyboardHook;

		public Register()
		{
			InitializeComponent();

			Shown += Register_Shown;

			keyboardHook = new KeyboardHookListener(new GlobalHooker());
			keyboardHook.Enabled = true;
			keyboardHook.KeyDown += keyboardHook_KeyDown;
		}

		private void Register_Shown(object sender, EventArgs e)
		{
			label.Text = "Press any\u2122 key";
			acceptButton.Hide();
			clearButton.Show();
			active = true;
		}

		private void keyboardHook_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Escape && e.KeyCode != Keys.Back && e.KeyCode != Keys.Enter)
			{
				e.SuppressKeyPress = true;
				label.Text = String.Format("Use \"{0}\" as the new key?", e.KeyCode);
				modifier = e.KeyCode;

				acceptButton.Show();
				clearButton.Hide();
			}
		}

		private void Exit()
		{
			keyboardHook.Enabled = false;
			active = false;

			this.Close();
		}

		private void ApplyBindings(int newModifier)
		{
			Settings.SetModifier(newModifier);

			Options options = Application.OpenForms["Options"] as Options;
			options.UpdateDetails();

			Exit();
		}

		private void acceptButton_Click(object sender, EventArgs e)
		{
			ApplyBindings((int)modifier);
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			ApplyBindings(0);
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			Exit();
		}
	}
}
