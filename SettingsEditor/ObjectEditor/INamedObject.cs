namespace SettingsEditor
{
	/// <summary>
	/// A settings object, displayable via treeview.
	/// </summary>
	/// <remarks>
	/// Derived classes must have the "Serializable" attribute.
	/// </remarks>
	public interface INamedObject
	{
		string Name { get; set; }
	}
}
