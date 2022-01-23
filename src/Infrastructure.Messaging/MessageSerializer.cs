namespace Pitstop.Infrastructure.Messaging;

/// <summary>
/// Serializer used for all messages.
/// </summary>
public static class MessageSerializer
{
    private static JsonSerializerSettings _serializerSettings;

    /// <summary>
    /// Constructor.
    /// </summary>
    static MessageSerializer()
    {
        _serializerSettings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };
        _serializerSettings.Converters.Add(new StringEnumConverter
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        });
    }

    /// <summary>
    /// Serialize an object to a JSON string.
    /// </summary>
    /// <param name="value">The value to serialize.</param>
    public static string Serialize(object value)
    {
        return JsonConvert.SerializeObject(value, _serializerSettings);
    }

    /// <summary>
    /// Deserialize JSON to an object.
    /// </summary>
    /// <param name="value">The JSON data to deserialize.</param>
    public static JObject Deserialize(string value)
    {
        return JsonConvert.DeserializeObject<JObject>(value, _serializerSettings);
    }
}