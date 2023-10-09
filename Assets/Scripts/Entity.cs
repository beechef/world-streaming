using System;
using UnityEngine;

namespace WorldStreaming
{
	public class Entity : MonoBehaviour, ISimulatedCycle, IObservableMovement
	{
		public Vector3 Position => transform.position;
		public Action<Vector3> OnMoved { get; set; }

		public void OnStart()
		{
		}

		public void OnUpdate(float deltaTime)
		{
		}

		public void OnStop()
		{
		}

		private void Update()
		{
			OnMoved?.Invoke(transform.position);
		}
	}
}