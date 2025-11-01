using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds a value to a list inside a dictionary. Creates the list if it doesn’t exist.
        /// </summary>
        public static void AddToListValue<TKey, TValue>(
            this Dictionary<TKey, List<TValue>> dict,
            TKey key,
            TValue value)
        {
            if (!dict.ContainsKey(key))
                dict[key] = new List<TValue>();

            dict[key].Add(value);
        }
    }
}
