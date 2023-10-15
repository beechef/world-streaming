using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WorldStreaming
{
	[Serializable]
	public class ChunkData
	{
		public List<IStateData> StateData = new();
		[JsonIgnore] public List<IStateComponent> StateComponents = new();
	}
}