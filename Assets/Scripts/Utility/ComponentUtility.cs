﻿using System.Collections.Generic;
using UnityEngine;

namespace WorldStreaming
{
	public static class ComponentUtility
	{
		public static List<T> GetAllComponentInChildren<T>(this GameObject go)
		{
			var components = new List<T>();

			var childComponents = go.GetComponents<T>();
			if (childComponents != null) components.AddRange(childComponents);

			for (var i = 0; i < go.transform.childCount; i++)
			{
				components.AddRange(GetAllComponentInChildren<T>(go.transform.GetChild(i).gameObject));
			}

			return components;
		}

		public static T GetOrAddComponent<T>(this GameObject go) where T : Component
		{
			return go.GetComponent<T>() ?? go.AddComponent<T>();
		}
	}
}