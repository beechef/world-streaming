using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WorldStreaming
{
	public class ChunkInfo : MonoBehaviour
	{
		public int id;

		public Vector2Int size;
		public Vector3 position;
		public Rect rect;

		[ShowInInspector] private List<ISimulatedCycle> _simulatedCycles;


		[Button]
		public void Bake()
		{
			position = transform.position;
			rect = new Rect(position.x, position.z, size.x, size.y);

			_simulatedCycles = gameObject.GetAllComponentInChildren<ISimulatedCycle>();
		}
	}
}