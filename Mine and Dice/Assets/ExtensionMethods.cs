
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ExtensionMethods {
	public static void ClearAllChildren(this Transform transform)
	{
		int childs = transform.childCount;
		for (int i = childs - 1; i >= 0; i--)
		{
			GameObject.Destroy(transform.GetChild(i).gameObject);
		}
	}
}
