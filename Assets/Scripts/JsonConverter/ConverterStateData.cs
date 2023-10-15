using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorldStreaming.StateData;

namespace WorldStreaming.JsonConverter
{
	public class ConverterStateData : Newtonsoft.Json.JsonConverter
	{
		public override bool CanWrite => false;

		public override bool CanConvert(Type objectType)
		{
			return typeof(IStateData).IsAssignableFrom(objectType);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType is JsonToken.Null) return null;

			var jObject = JObject.Load(reader);
			var type = jObject[nameof(IStateData.Type)]!.ToObject<StateDataType>();
			var stateData = FactoryStateData.Create(type);

			serializer.Populate(jObject.CreateReader(), stateData);
			return stateData;
		}
	}
}