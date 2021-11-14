using System;
using Gstd.Script;

namespace test
{
    class ScriptEngineTest : TestSuite
    {
        public ScriptEngineTest(Func<string, string> readData, Action<string> logInfo, Action<string> logOk, Action<string> logFail) : base(readData, logInfo, logOk, logFail)
        {
        }
        public override void Run()
        {
            TestOkSyntax();
            TestFailSyntax();
        }
        private void TestOkSyntax()
        {
            string[] scriptFilePaths = {
                "syntax/ok/Array.txt",
                "syntax/ok/Branch.txt",
                "syntax/ok/Comment.txt",
                "syntax/ok/EmptyFile.txt",
                "syntax/ok/Escape.txt",
                "syntax/ok/ExpressionsAndOperators.txt",
                "syntax/ok/Include.txt",
                "syntax/ok/LocalScope.txt",
                "syntax/ok/Loop.txt",
                "syntax/ok/MicroThread.txt",
                "syntax/ok/SampleA01.txt",
                "syntax/ok/SemicolonOnly.txt",
                "syntax/ok/Statement.txt",
                "syntax/ok/Subroutine.txt",
                "syntax/ok/UserDefineFunction.txt",
                "syntax/ok/Variable.txt"
            };
            LogInfo("[ScriptEngineTest] ok syntax");
            foreach (string scriptFilePath in scriptFilePaths)
            {
                bool result = ParseScript(scriptFilePath);
                if (result)
                {
                    LogFail(String.Format("\terror : {0}", scriptFilePath));
                    continue;
                }
                LogOk(String.Format("\tok : {0}", scriptFilePath));
            }
        }
        private void TestFailSyntax()
        {
            string[] scriptFilePaths = {
                "syntax/fail/Lesson-4-Array-1.txt",
                "syntax/fail/Lesson-4-Array-2.txt",
                "syntax/fail/Lesson-4-LocalAndGlobalVariable-1.txt",
                "syntax/fail/Lesson-4-LocalAndGlobalVariable-2.txt",
                "syntax/fail/Lesson-4-LocalAndGlobalVariable-3.txt",
                "syntax/fail/Lesson-4-LocalAndGlobalVariable-4.txt",
                "syntax/fail/Lesson-4-LocalAndGlobalVariable-5.txt",
                "syntax/fail/Lesson-4-MathematicalOperation-1.txt",
                "syntax/fail/Lesson-4-MathematicalOperation-2.txt",
                "syntax/fail/Lesson-4-MathematicalOperation-3.txt",
                "syntax/fail/Lesson-4-Variable-1.txt",
                "syntax/fail/Lesson-4-Variable-2.txt",
            };
            LogInfo("[ScriptEngineTest] fail syntax");
            foreach (string scriptFilePath in scriptFilePaths)
            {
                bool result = ParseScript(scriptFilePath);
                if (!result)
                {
                    LogFail(String.Format("\terror : {0}", scriptFilePath));
                    continue;
                }
                LogOk(String.Format("\tok : {0}", scriptFilePath));
            }
        }
        private bool ParseScript(string filePath)
        {
            ScriptTypeManager typeManager = new ScriptTypeManager();
            string source = ReadData(filePath);
            Function[] funcv = {};
            ScriptEngine engine = new ScriptEngine(typeManager, source, funcv);
            return engine.Error;
        }
    }
}
