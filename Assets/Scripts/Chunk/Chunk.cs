using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WorldStreaming.StateData;

namespace WorldStreaming
{
	public class Chunk : MonoBehaviour
	{
		public ChunkInfo info;
		[ShowInInspector] public ISimulatedCycleData Data => null;

		[ShowInInspector] private List<ISimulatedCycle> _simulatedCycles;
		private readonly Simulator _simulator = new();

		private ChunkData _data;

		private void OnEnable()
		{
			gameObject.name = name;
			transform.position = info.position;
		}

		public void OnStart()
		{
			_simulator.Start();
		}

		public void Update()
		{
			_simulator.Update(Time.deltaTime);
		}

		public void OnStop()
		{
			_simulator.Stop();
		}

		public void Bake()
		{
			if (string.IsNullOrEmpty(name)) name = gameObject.name;

			info.position = transform.position;
			info.rect = new Rect(info.position.x - info.size.x / 2f, info.position.z - info.size.y / 2f, info.size.x, info.size.y);

			var sceneObjects = gameObject.GetAllComponentInChildren<SceneObject>();
			foreach (var sceneObject in sceneObjects)
			{
				sceneObject.GenerateId();
			}
		}

		public void InitDefaultData()
		{
			_data = new ChunkData();

			var stateComponents = gameObject.GetAllComponentInChildren<IStateComponent>();
			var currentTick = DateTime.UtcNow.Ticks;

			foreach (var stateComponent in stateComponents)
			{
				stateComponent.InitDefaultData();

				if (stateComponent.StateData is ISimulatedCycleData cycleData)
				{
					cycleData.LastTick = currentTick;
				}

				_data.StateData.Add(stateComponent.StateData);
			}
		}

		public void Init(ChunkData data)
		{
			_data = data;

			foreach (var stateData in data.StateData)
			{
				FactoryStateComponent.Create(stateData);
			}
		}

		public void OnLoad()
		{
			_simulatedCycles = gameObject.GetAllComponentInChildren<ISimulatedCycle>();
			_simulator.Add(_simulatedCycles);
		}

		public void OnUnload()
		{
		}

		public void OnSave()
		{
		}

		public ChunkData GetData()
		{
			return _data;
		}
	}
}