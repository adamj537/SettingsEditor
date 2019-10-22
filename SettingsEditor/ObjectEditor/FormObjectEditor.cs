using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace SettingsEditor
{
	public partial class FormObjectEditor : Form
    {
        public FormObjectEditor()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Parse an object's properties into the form.
		/// </summary>
		/// <typeparam name="objectType"></typeparam>
		/// <param name="propertiesObject">object with properties to be parsed into the form</param>
		/// <param name="propertyForText">name of the object's property providing text to show in the form</param>
		public void AddObject<objectType>(object propertiesObject, string propertyForText)
			where objectType : Attribute
		{
			if (propertiesObject != null)
			{
                // See if we can get the property propertyForText from the item
                PropertyInfo propertyInfoForText = propertiesObject.GetType().GetProperty(propertyForText);

				// TODO:  Tree view isn't implemented correctly.
				// If we can't access the property then it might not be a valid object, or not a valid property so return.
				if (propertyInfoForText != null)
				{
					Add<objectType>(propertiesObject, null, propertyForText);
				}

				// Add object to properties grid.
				propertyGrid.SelectedObject = propertiesObject;
			}
		}

		/// <summary>
		/// Get the object (presumably to save it after user modifications).
		/// </summary>
		/// <typeparam name="T">must be same type as passed into "AddObject" method</typeparam>
		/// <returns>object from the property grid</returns>
		public T FetchObject<T>()
		{
			return (T)propertyGrid.SelectedObject;
		}

		/// <summary>
		/// Add an object to the treeview.
		/// </summary>
		/// <typeparam name="TAttribute"></typeparam>
		/// <param name="item"></param>
		/// <param name="treeNodeParent"></param>
		/// <param name="sPropertyForText"></param>
		/// <returns></returns>
		private TreeNode Add<TAttribute>(object item, TreeNode treeNodeParent, string sPropertyForText)
		  where TAttribute : Attribute
		{
			if (null != item)
			{
				// See if we can get the property sPropertyForText from the item
				PropertyInfo propertyInfoForText = item.GetType().GetProperty(sPropertyForText);

				// if we can't access the property then it might not be a valid object, or not a valid property so return.
				if (null == propertyInfoForText)
				{
					return null;
				}

				// Create a new tree node
				TreeNode treeNode = new TreeNode(propertyInfoForText.GetValue(item, null).ToString())
				{
					// Store a reference to the actual object as the TreeNode's Tag property
					Tag = item
				};

				// if there's a valid parent add the new treenode to the parent
				if (null != treeNodeParent)
				{
					treeNodeParent.Nodes.Add(treeNode);
				}

				// Is item a array or container of objects? i.e. if it implements IEnumerable we can enumerate over
				// those objects to see if they can be added to the tree.
				if (item is IEnumerable enumerableObject)
				{
					foreach (object itemInEnumerable in enumerableObject)
					{
						Add<TAttribute>(itemInEnumerable, treeNode, sPropertyForText);
					}
				}

				// Get the object's properties and see if there are any that have the attribute TreeNodeAttribute assigned to it
				PropertyInfo[] propertyInfos = item.GetType().GetProperties();

				foreach (PropertyInfo propertyInfo in propertyInfos)
				{
					// Check all attribs available on the property
					object[] attribs = propertyInfo.GetCustomAttributes(false);

					foreach (object attribute in attribs)
					{
						// Try and add the return value of the property to the tree,
						// if the property returns an object that is not an instance of INamedObjectItem then it will be null which is
						// caught at the begining of this method.
						Add<CategoryAttribute>(propertyInfo.GetValue(item, null), treeNode, sPropertyForText);
					}
				}

				return treeNode;
			}

			return null;
		}
	}
}
