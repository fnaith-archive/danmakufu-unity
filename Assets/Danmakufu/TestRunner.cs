using System;
using test;

namespace cs
{
    public class TestRunner
    {
        private Func<string, string> readData;
        private Action<string> logInfo;
        private Action<string> logOk;
        private Action<string> logError;
        public TestRunner(Func<string, string> readData, Action<string> logInfo, Action<string> logOk, Action<string> logError)
        {
            this.readData = readData;
            this.logInfo = logInfo;
            this.logOk = logOk;
            this.logError = logError;
        }
        public void RunAllTests(string testDirPath)
        {
            Func<string, string> readDataFunc = filePath => readData(testDirPath + "data/" + filePath);
            new ScriptEngineTest(readDataFunc, logInfo, logOk, logError).Run();
            new ScriptMachineTest(readDataFunc, logInfo, logOk, logError).Run();
            new PathPropertyTest(readDataFunc, logInfo, logOk, logError).Run();
        }
    }
}
