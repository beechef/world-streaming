using System;
using UnityEngine;

namespace WorldStreaming
{
	public interface IObservableMovement
	{
		public Vector3 Position { get; }
		public Action<Vector3> OnMoved { get; set; }
	}
}