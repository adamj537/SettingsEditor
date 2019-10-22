﻿using System.IO;
using System.Xml.Serialization;

namespace SettingsEditor
{
	/// <summary>
	/// A generic class used to serialize objects.
	/// </summary>
	/// <remarks>
	/// Inspired by the following articles:
	/// https://docs.microsoft.com/en-us/dotnet/standard/serialization/how-to-serialize-an-object
	/// https://docs.microsoft.com/en-us/dotnet/standard/serialization/how-to-deserialize-an-object
	/// </remarks>
	public class Serializer
	{
		/// <summary>
		/// Save a serializable object to an XML file.
		/// </summary>
		/// <typeparam name="T">the object's class type</typeparam>
		/// <param name="obj">the object to be serialized</param>
		/// <param name="filename">filename to save the resulting XML as</param>
		public static void SerializeXML<T>(T myObject, string filename)
		{
			// Create a serializer using the type of the object to be serialized.
			XmlSerializer mySerializer = new XmlSerializer(typeof(T));

			// Create a StreamWriter object.
			using (StreamWriter myWriter = new StreamWriter(filename))
			{
				// Write the file.
				mySerializer.Serialize(myWriter, myObject);

				// Close the file.
				myWriter.Close();
			}
		}

		/// <summary>
		/// Return a class object from a serialized XML file.
		/// </summary>
		/// <typeparam name="T">the object's class type</typeparam>
		/// <param name="filename">filename containing the serialized class</param>
		/// <returns>deserialized object</returns>
		public static T DeserializeXML<T>(string filename)
		{
			// Create a serializer using the type of the object to be deserialized.
			XmlSerializer mySerializer = new XmlSerializer(typeof(T));

			// Create a filestream to read the file.
			using (FileStream myFileStream = new FileStream(filename, FileMode.Open))
			{
				// Read the file.
				T data = (T)mySerializer.Deserialize(myFileStream);

				// Close the file.
				myFileStream.Close();

				// Deserialize the file and cast to the object type.
				return data;
			}
		}
	}
}
