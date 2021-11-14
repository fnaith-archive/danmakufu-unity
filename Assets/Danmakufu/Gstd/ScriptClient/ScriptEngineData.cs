using System.Collections.Generic;
using Gstd.Script;

namespace Gstd
{
    namespace ScriptClient
    {
        sealed class ScriptEngineData
        {
            private string path;
            private string source;
            private ScriptEngine engine;
            private ScriptFileLineMap mapLine;

            public ScriptEngineData()
            {
                mapLine = new ScriptFileLineMap();
            }
            //public virtual ~ScriptEngineData();

            public void SetPath(string path)
            {
                this.path = path;
            }
            public string GetPath()
            {
                return path;
            }
            public void SetSource(string source)
            {
                this.source = source;
            }
            public string GetSource()
            {
                return source;
            }
            public void SetEngine(ScriptEngine engine)
            {
                this.engine = engine;
            }
            public ScriptEngine GetEngine()
            {
                return engine;
            }
            public ScriptFileLineMap GetScriptFileLineMap()
            {
                return mapLine;
            }
            public void SetScriptFileLineMap(ScriptFileLineMap mapLine)
            {
                this.mapLine = mapLine;
            }
        }
    }
}
