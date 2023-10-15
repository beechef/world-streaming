namespace WorldStreaming.Storage
{
	public static class ChunkStorageRules
	{
		public const string ChunkDataStorageKey = "chunk_data_storage_{0}";

		public static string GetChunkDataStorageKey(ChunkInfo chunkInfo)
		{
			return string.Format(ChunkDataStorageKey, chunkInfo.id);
		}
	}
}