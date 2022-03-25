using System.Text.Json;

namespace WebComicProvider.Models
{
    public static class Extensions
    {
        /// <summary>
        /// Converts an object to its JSON representation
        /// </summary>
        /// <typeparam name="T">The data type being serialized</typeparam>
        /// <param name="o">The object to serialize</param>
        /// <returns>JSON</returns>
        public static string ToJson<T>(this T o) => JsonSerializer.Serialize(o);

        /// <summary>
        /// Converts a JSON string to an object of the given type, if compatible
        /// </summary>
        /// <typeparam name="T">The target data type</typeparam>
        /// <param name="json">The JSON to deserialize</param>
        /// <returns>Object</returns>
        public static T? FromJson<T>(this string json) => JsonSerializer.Deserialize<T>(json);
    }
}
