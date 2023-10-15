using System;

namespace WorldStreaming.StateData
{
	[Serializable]
	public class StateDataMonkey : IStateData
	{
		public StateDataType Type => StateDataType.Monkey;
	}
}