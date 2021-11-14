using System.Collections.Generic;
using Gstd.Script;

namespace Gstd
{
    namespace ScriptClient
    {
        sealed class ScriptEngineCache
        {
            private Dictionary<string, ScriptEngineData> cache;

            public ScriptEngineCache()
            {
            }
            //public virtual ~ScriptEngineCache();
            public void Clear()
            {
                cache.Clear();
            }
            public void AddCache(string name, ScriptEngineData data)
            {
                cache[name] = data;
            }
            public ScriptEngineData GetCache(string name)
            {
                if (!IsExists(name))
                {
                    return null;
                }
                return cache[name];
            }
            public bool IsExists(string name)
            {
                return cache.ContainsKey(name);
            }
        }
    }
}
