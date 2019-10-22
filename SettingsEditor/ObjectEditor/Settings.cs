using System;
using System.IO;

namespace SettingsEditor
{
	/// <summary>
	/// Class to allow editing of settings objects by user
	/// </summary>
	public static class Settings
	{
		/// <summary>
		/// Folder where settings files will be stored for the application.
		/// </summary>
		/// <remarks>
		/// Default setting is the location that the application runs from.
		/// </remarks>
		public static string Directory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

		/// <summary>
		/// Retrieve an object of the specified type from the specified settings file
		/// </summary>
		/// <typeparam name="T">the type the settings object</typeparam>
		/// <param name="filename">XML file where settings are stored</param>
		/// <returns>an object containing settings from the file</returns>
		public static T Load<T>(string filename)
		{
			// Form the path to the file.
			string filepath = GetSettingsFilePath(filename);

			// Start with a null settings class.
			T settings = default;

			// If the file exists...
			if (File.Exists(filepath))
			{
				// Retrieve settings from file into the object.
				settings = Serializer.DeserializeXML<T>(filepath);
			}

			// If the file didn't exist or otherwise couldn't be deserialized,
			// put default values into the settings object.
			if (settings == null)
			{
				settings = Activator.CreateInstance<T>();
			}

			return settings;
		}

		/// <summary>
		/// Save the object of the specified type to the specified local settings file
		/// </summary>
		/// <typeparam name="T">the type the settings object</typeparam>
		/// <param name="settings">Object to be serialized to XML</param>
		/// <param name="fileName">XML file where settings are stored</param>
		public static void Save<T>(T settings, string filename)
		{
			// Form the path to the file.
			string filepath = GetSettingsFilePath(filename);

			// Save the settings as an XML file.
			Serializer.SerializeXML(settings, filepath);
		}

		#region HelperMethods

		/// <summary>
		/// Get the full path to an XML file where settings are stored.
		/// </summary>
		/// <param name="settingsName">name of the file (not including the extension)</param>
		/// <returns>full file path</returns>
		private static string GetSettingsFilePath(string fileName)
		{
			return Path.Combine(Directory, fileName.Replace(' ', '_') + ".xml");
		}

		#endregion
	}
}
