using System;
using Gstd.Script;

namespace cs
{
    public class TestRunner
    {
        private Func<string, string> readFunc;
        private Action<string> logFunc;
        private Action<string> logErrorFunc;

        public TestRunner(Func<string, string> readFunc, Action<string> logFunc, Action<string> logErrorFunc)
        {
            this.readFunc = readFunc;
            this.logFunc = logFunc;
            this.logErrorFunc = logErrorFunc;
        }
        public void RunAllTests(string scriptFolderPath)
        {
            RunSyntaxTest(scriptFolderPath);
        }
        private void RunSyntaxTest(string scriptFolderPath)
        {
            // test data from : https://touhougc.web.fc2.com/products/th_dnh_help_v3.html
            string[] testFilePaths = {
                "/syntax/EMptyFile.txt",
                "/syntax/SemicolonOnly.txt",
                "/syntax/Statement.txt",
                "/syntax/Variable.txt",
                "/syntax/Array.txt",
                "/syntax/LocalScope.txt",
                "/syntax/ExpressionsAndOperators.txt",
                "/syntax/Branch.txt",
                "/syntax/Loop.txt",
                "/syntax/Escape.txt",
                "/syntax/Subroutine.txt",
                "/syntax/UserDefineFunction.txt",
                "/syntax/MicroThread.txt",
                "/syntax/Comment.txt",
                "/syntax/Include.txt"
            };
            foreach (string filePath in testFilePaths)
            {
                RunTest(scriptFolderPath + filePath);
            }
        }
        private void RunTest(string filePath)
        {
            try
            {
                ScriptTypeManager typeManager = new ScriptTypeManager();
                string source = readFunc(filePath);
                Function[] funcv = {};
                ScriptEngine engine = new ScriptEngine(typeManager, source, funcv);
                if (engine.Error)
                {
                    logErrorFunc(String.Format("FAIL  : {0}", filePath));
                    logErrorFunc(String.Format("\tLine {0} : {1}", engine.ErrorLine, engine.ErrorMessage));
                }
                else
                {
                    logFunc(String.Format("OK    : {0}", filePath));
                }
            }
            catch (Exception e)
            {
                logErrorFunc(String.Format("ERROR : {0}", filePath));
                logErrorFunc(String.Format("\tLine {0}\n{1}", e.Message, e.StackTrace));
            }
        }
    }
}
