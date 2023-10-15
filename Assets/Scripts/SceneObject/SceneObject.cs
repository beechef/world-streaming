using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WorldStreaming
{
	public class SceneObject : MonoBehaviour
	{
		[ShowInInspector] public string Id = string.Empty;

		private void Awake()
		{
			SceneObjectController.SceneObjects.TryAdd(Id, this);
		}

		public void GenerateId()
		{
			if (string.IsNullOrEmpty(Id)) Id = Guid.NewGuid().ToString();
		}

		private void OnDestroy()
		{
			SceneObjectController.SceneObjects.Remove(Id);
		}
	}
}