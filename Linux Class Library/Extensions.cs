namespace Skyline.DataMiner.Utils.Linux
{
	using System.Collections.Generic;

	internal static class Extensions
	{
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			if (dictionary.TryGetValue(key, out TValue value))
			{
				return value;
			}
			return default;
		}
	}
}