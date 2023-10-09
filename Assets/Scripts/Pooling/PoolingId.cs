using WorldStreaming.Storage;

namespace WorldStreaming.Pooling
{
	public static class PoolingId
	{
		private static readonly StoragePoolingIdData _data;

		static PoolingId()
		{
			_data = StorageController.Instance.GetStorage<StoragePoolingId>().Data;
		}

		public static long GetId()
		{
			return _data.FreeIds.Count == 0 ? _data.Id++ : _data.FreeIds.Pop();
		}

		public static void ReturnId(long id)
		{
			_data.FreeIds.Push(id);
		}
	}
}