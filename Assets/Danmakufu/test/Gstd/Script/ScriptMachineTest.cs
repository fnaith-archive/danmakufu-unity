using System;
using Gstd.Script;

namespace test
{
    class ScriptMachineTest : TestSuite
    {
        public ScriptMachineTest(Func<string, string> readData, Action<string> logInfo, Action<string> logOk, Action<string> logFail) : base(readData, logInfo, logOk, logFail)
        {
        }
        public override void Run()
        {
            TestOkScript();
            TestFailScript();
        }
        private void TestOkScript()
        {
            string[] scriptFilePaths = {
                "script/ok/Lesson-1-Init.txt",
                "script/ok/Lesson-4-Array.txt",
                "script/ok/Lesson-4-Comment.txt",
                "script/ok/Lesson-4-LocalAndGlobalVariable.txt",
                "script/ok/Lesson-4-MathematicalOperation.txt",
                "script/ok/Lesson-4-String.txt",
                "script/ok/Lesson-4-Variable.txt",
                "script/ok/Lesson-5-Boolean.txt",
                "script/ok/Lesson-5-Function.txt",
                "script/ok/Lesson-5-IfElse.txt",
                "script/ok/Lesson-5-Loop.txt",
                "script/ok/Task.txt"
            };
            LogInfo("[ScriptMachineTest] ok script");
            foreach (string scriptFilePath in scriptFilePaths)
            {
                TestResult result = RunScript(scriptFilePath);
                if (result.engineError)
                {
                    LogFail(String.Format("\tengine error : {0}", scriptFilePath));
                    continue;
                }
                if (result.machineError)
                {
                    LogFail(String.Format("\tmachine error : {0}", scriptFilePath));
                    continue;
                }
                LogOk(String.Format("\tok : {0}", scriptFilePath));
            }
        }
        private void TestFailScript()
        {
            string[] scriptFilePaths = {
                "script/fail/Lesson-4-ChangeType.txt",
                "script/fail/Lesson-4-Array-1.txt",
                "script/fail/Lesson-4-Array-2.txt",
                "script/fail/Lesson-4-Array-3.txt",
                "script/fail/Lesson-5-Subroutine.txt"
            };
            LogInfo("[ScriptMachineTest] fail script");
            foreach (string scriptFilePath in scriptFilePaths)
            {
                TestResult result = RunScript(scriptFilePath);
                if (result.engineError)
                {
                    LogFail(String.Format("\tengine error : {0}", scriptFilePath));
                    continue;
                }
                if (!result.machineError)
                {
                    LogFail(String.Format("\tmachine error : {0}", scriptFilePath));
                    continue;
                }
                LogOk(String.Format("\tok : {0}", scriptFilePath));
            }
        }
        private TestResult RunScript(string filePath)
        {
            ScriptTypeManager typeManager = new ScriptTypeManager();
            string source = ReadData(filePath);
            Function[] funcv = {};
            ScriptEngine engine = new ScriptEngine(typeManager, source, funcv);
            ScriptMachine machine = new ScriptMachine(engine);
            machine.Run();
            //LogOk(String.Format("{0} : {1}", machine.ErrorLine, machine.ErrorMessage));
            return new TestResult(engine.Error, machine.Error);
        }
        private class TestResult {
            public bool engineError;
            public bool machineError;
            public TestResult(bool engine_error_, bool machine_error_) {
                engineError = engine_error_;
                machineError = machine_error_;
            }
        }
    }
}
