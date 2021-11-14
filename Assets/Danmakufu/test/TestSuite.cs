using System;

namespace test
{
    abstract class TestSuite
    {
        private Func<string, string> readData;
        private Action<string> logInfo;
        private Action<string> logOk;
        private Action<string> logFail;
        protected TestSuite(Func<string, string> readData, Action<string> logInfo,
                            Action<string> logOk, Action<string> logFail)
        {
            this.readData = readData;
            this.logInfo = logInfo;
            this.logOk = logOk;
            this.logFail = logFail;
        }
        protected string ReadData(string filePath)
        {
            return readData(filePath);
        }
        protected void LogInfo(string message)
        {
#if _REPORT_INFO
            logInfo(message);
#endif
        }
        protected void LogOk(string message)
        {
#if _REPORT_OK
            logOk(message);
#endif
        }
        protected void LogFail(string message)
        {
#if _REPORT_FAIL
            logFail(message);
#endif
        }
        protected void Assert(bool condition)
        {
            if (!condition)
            {
                throw new Exception("Assert Failed");
            }
        }
        protected void AssertEquals(string expected, string actual)
        {
            if (expected != actual)
            {
                throw new Exception(null != actual ? actual : "(null)");
            }
        }
        protected void AssertThrow(Action action)
        {
            bool hasError = false;
            try
            {
                action();
            }
            catch
            {
                hasError = true;
            }
            if (!hasError)
            {
                throw new Exception("No Throw");
            }
        }
        protected void Run(string name, Action testCase)
        {
            try
            {
                LogInfo(name);
                testCase();
                LogOk("ok");
            }
            catch (Exception e)
            {
                LogFail("fail : " + e.ToString());
            }
        }
        public abstract void Run();
    };
}
