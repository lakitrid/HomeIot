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
                Culture = CultureInfo.InvariantCulture
            };

            JsonSerializer serializer = JsonSerializer.Create(settings);
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, element);
                return writer.ToString();
            }
        }
    }
}
