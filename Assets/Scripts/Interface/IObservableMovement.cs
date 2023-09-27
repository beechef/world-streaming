using System;
using UnityEngine;

namespace WorldStreaming
{
	public interface IObservableMovement
	{
		public Action<Vector3> OnMoved { get; set; }
	}
}