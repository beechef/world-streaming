using System;
using System.Collections.Generic;

namespace WorldStreaming
{
	[Serializable]
	public class ChunkData
	{
		public List<IStateData> StateData = new();
	}
}