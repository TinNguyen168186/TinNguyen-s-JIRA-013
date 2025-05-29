using System;
using System.Collections.Generic;
using System.Threading;

namespace Core.ShareData
{
    public class DataStorage
    {
        private static ThreadLocal<Dictionary<string, object>> _data = 
            new ThreadLocal<Dictionary<string, object>>(() => new Dictionary<string, object>());

        public static void SetData(string key, object value)
        {
            var dict = _data.Value ?? throw new InvalidOperationException("Thread-local storage not initialized.");
            dict[key] = value;
        }

        public static object? GetData(string key)
        {
            var dict = _data.Value;
            if (dict != null && dict.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }

        public static void ClearData()
        {
            _data.Value?.Clear();
        }

    }
}