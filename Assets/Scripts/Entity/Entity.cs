using System;
using UnityEngine;
using WorldStreaming.StateData;

namespace WorldStreaming.Entity
{
	public abstract class Entity : MonoBehaviour, ISimulatedCycle, IStateComponent, IObservableMovement
	{
		public Vector3 Position => transform.position;
		public Action<Vector3> OnMoved { get; set; }

		public abstract StateDataType StateDataType { get; }

		public abstract IStateData StateData { get; }

		public abstract ISimulatedCycleData Data { get; }

		public abstract void InitDefaultData();

		public abstract void SetData(IStateData data);

		public abstract void OnStart();

		public abstract void OnUpdate(float deltaTime);
		public abstract void OnSimulate(float deltaTime);
		public abstract void OnStop();
	}
}