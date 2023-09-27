using System;
using UnityEngine;

namespace WorldStreaming
{
	public class Entity : MonoBehaviour, ISimulatedCycle, IObservableMovement
	{
		public Action<Vector3> OnMoved { get; set; }

		public void OnUpdate(float deltaTime)
		{
		}

		private void Update()
		{
			OnMoved?.Invoke(transform.position);
		}
	}
}