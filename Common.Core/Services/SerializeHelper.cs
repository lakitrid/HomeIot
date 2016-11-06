using Common.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Common.Services
{
    public static class SerializeHelper
    {
        /// <summary>
        /// Serializes an object
        /// </summary>
        /// <param name="element">element to serialize</param>
        /// <returns>serialized element</returns>
        public static string Serialize<T>(T element)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Culture = CultureInfo.InvariantCulture,
                TypeNameHandling = TypeNameHandling.All
            };

            JsonSerializer serializer = JsonSerializer.Create(settings);
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, element);
                return writer.ToString();
            }
        }

        public static T Deserialize<T>(byte[] rawMessage) where T : class
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Culture = CultureInfo.InvariantCulture,
                TypeNameHandling = TypeNameHandling.All
            };

            JsonSerializer serializer = JsonSerializer.Create(settings);
            try
            {
                string text = Encoding.UTF8.GetString(rawMessage, 0, rawMessage.Length);

                using (TextReader textReader = new StringReader(text))
                {
                    T result = serializer.Deserialize<T>(new JsonTextReader(textReader));
                    return result;
                }
            }
            catch (Exception exc)
            {
                return null;
            }
        }
    }
}
