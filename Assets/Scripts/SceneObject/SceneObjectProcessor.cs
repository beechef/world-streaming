using System;
using UnityEditor;
using UnityEngine;

namespace WorldStreaming
{
	public class SceneObjectProcessor : AssetPostprocessor
	{
		public void OnPostprocessPrefab(GameObject go)
		{
			var sceneObject = go.GetComponent<SceneObject>();
			if (!sceneObject) return;

			sceneObject.Id = default;
		}
	}
}