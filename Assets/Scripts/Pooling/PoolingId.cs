using WorldStreaming.Storage;

namespace WorldStreaming.Pooling
{
	public class PoolingId
	{
		private readonly StoragePoolingIdData _data;

		public PoolingId(StoragePoolingIdData data)
		{
			_data = data;
		}

		public long GetId()
		{
			return _data.FreeIds.Count == 0 ? _data.Id++ : _data.FreeIds.Pop();
		}

		public void ReturnId(long id)
		{
			_data.FreeIds.Push(id);
		}
	}
}