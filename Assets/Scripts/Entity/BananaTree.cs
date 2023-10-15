using Sirenix.OdinInspector;
using UnityEngine;
using WorldStreaming.StateData;

namespace WorldStreaming.Entity
{
	public class BananaTree : Entity
	{
		public override StateDataType StateDataType => StateDataType.BananaTree;

		public override IStateData StateData => _data;
		public override bool IsSave => true;

		public override ISimulatedCycleData Data => _data;

		[ShowInInspector] private StateDataBananaTree _data;

		public override void InitDefaultData()
		{
			_data = new StateDataBananaTree();

			var sceneObject = GetComponent<SceneObject>();
			if (!sceneObject) return;

			_data.IdSceneObject = sceneObject.Id;
		}

		public override void SetData(IStateData data)
		{
			_data = data as StateDataBananaTree;
		}

		public override void OnStart()
		{
		}

		public override void OnUpdate(float deltaTime)
		{
			if (_data.Timer < 5f)
			{
				_data.Timer += deltaTime;
				return;
			}

			_data.Count++;
			_data.Timer -= 5f;

			Debug.Log("Ok");
		}

		public override void OnStop()
		{
		}
	}
}