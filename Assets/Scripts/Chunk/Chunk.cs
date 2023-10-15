using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WorldStreaming
{
	[RequireComponent(typeof(ChunkDataController))]
	public class Chunk : MonoBehaviour
	{
		public ChunkInfo info;
		[ShowInInspector] public ISimulatedCycleData Data => null;

		[ShowInInspector] private List<ISimulatedCycle> _simulatedCycles;
		private readonly Simulator _simulator = new();

		private ChunkDataController _dataController;

		private void Awake()
		{
			_dataController = GetComponent<ChunkDataController>();
		}

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
			_dataController.InitDefaultData();
		}

		public void Init(ChunkData data)
		{
			_dataController.Init(data);
		}

		public void OnLoad()
		{
			Debug.Log($"Load {info.name}");

			_simulatedCycles = gameObject.GetAllComponentInChildren<ISimulatedCycle>();
			_simulator.Add(_simulatedCycles);
		}

		public void OnUnload()
		{
			Debug.Log($"Unload {info.name}");
		}

		public void OnSave()
		{
		}

		public ChunkData GetData()
		{
			return _dataController.Data;
		}
	}
}