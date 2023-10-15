using System;

namespace WorldStreaming.StateData
{
	[Serializable]
	public class StateDataBananaTree : IStateData, ISceneObject, ISimulatedCycleData
	{
		public string IdSceneObject { get; set; }
		public StateDataType Type => StateDataType.BananaTree;

		public int Count;
		public float Timer;
		
		public long LastTick { get; set; }
	}
}