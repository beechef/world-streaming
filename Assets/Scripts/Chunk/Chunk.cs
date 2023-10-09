using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WorldStreaming
{
	public class Chunk : MonoBehaviour, ISimulatedCycle
	{
		public ChunkInfo info;
		[ShowInInspector] private List<ISimulatedCycle> _simulatedCycles;

		private void OnEnable()
		{
			gameObject.name = name;
			transform.position = info.position;
		}

		public void OnStart()
		{
		}

		public void OnUpdate(float deltaTime)
		{
			foreach (var cycle in _simulatedCycles)
			{
				cycle.OnUpdate(deltaTime);
			}
		}

		public void OnStop()
		{
		}

		public void Bake()
		{
			if (string.IsNullOrEmpty(name)) name = gameObject.name;

			info.position = transform.position;
			info.rect = new Rect(info.position.x - info.size.x / 2f, info.position.z - info.size.y / 2f, info.size.x, info.size.y);

			_simulatedCycles = gameObject.GetAllComponentInChildren<ISimulatedCycle>();
		}
	}
}