using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldStreaming
{
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[SerializeField] [HideInInspector] private readonly List<TKey> _keys = new();
		[SerializeField] [HideInInspector] private readonly List<TValue> _values = new();

		public void OnBeforeSerialize()
		{
			_keys.Clear();
			_values.Clear();

			foreach (var keyPair in this)
			{
				_keys.Add(keyPair.Key);
				_values.Add(keyPair.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			Clear();

			for (var i = 0; i < _keys.Count; i++)
			{
				Add(_keys[i], _values[i]);
			}

			_keys.Clear();
			_values.Clear();
		}
	}
}