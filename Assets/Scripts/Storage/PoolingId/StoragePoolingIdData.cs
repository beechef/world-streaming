using System;
using System.Collections.Generic;

namespace WorldStreaming.Storage
{
	[Serializable]
	public class StoragePoolingIdData
	{
		public long Id;
		public Stack<long> FreeIds = new();
	}
}