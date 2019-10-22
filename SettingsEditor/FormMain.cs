using System;
using System.Windows.Forms;

namespace SettingsEditor
{
	public partial class FormMain : Form
	{
		// This object holds settings that we'd like to serialize/deserialize/edit.
		private TestSettings _testSettings;

		public FormMain()
		{
			InitializeComponent();

			// Populate the directory text box.
			textBoxDirectory.Text = Settings.Directory;
		}

		/// <summary>
		/// When the user clicks the "Browse" button, open a folder browser.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			// Create a file browser dialog.
			FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

			// Display the dialog to the user.
			DialogResult result = folderBrowser.ShowDialog();

			// If the user clicks "Open"...
			if (result == DialogResult.OK)
			{
				textBoxDirectory.Text = folderBrowser.SelectedPath;
			}
		}

		/// <summary>
		/// When the "Load" button is clicked, de-serialize the settings file into an object.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonLoad_Click(object sender, EventArgs e)
		{
			// Set the directory.
			Settings.Directory = textBoxDirectory.Text;

			// Load the settings file.
			_testSettings = Settings.Load<TestSettings>(textBoxFilename.Text);
		}

		/// <summary>
		/// When the "Edit" button is clicked, open the Settings editor.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonEdit_Click(object sender, EventArgs e)
		{
			// Set the directory.
			Settings.Directory = textBoxDirectory.Text;

			// Open the settings editor.
			EditSettings<TestSettings>(textBoxFilename.Text);
		}

		/// <summary>
		/// Present a settings file to the user to edit.
		/// </summary>
		/// <param name="filename"></param>
		private void EditSettings<T>(string filename) where T : Attribute
		{
			try
			{
				// Fetch equipment settings.
				T settings = Settings.Load<T>(filename);

				// Create and show a new object editor with the equipment settings.
				FormObjectEditor objectEditor = new FormObjectEditor();
				objectEditor.AddObject<T>(settings, "Label");
				DialogResult result = objectEditor.ShowDialog();

				// If user selects "OK," save the settings.
				if (result == DialogResult.OK)
				{
					Settings.Save(objectEditor.FetchObject<T>(), filename);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Could not edit settings." + Environment.NewLine + ex.Message, "Error");
			}
		}

		/// <summary>
		/// When the "Save" button is clicked, serialize the settings object back into a file.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonSave_Click(object sender, EventArgs e)
		{
			Settings.Save(_testSettings, textBoxFilename.Text);
		}
	}
}
