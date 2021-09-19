using UnityEngine;

namespace cs
{
    public class UnityTestRunner : MonoBehaviour
    {
        void Start()
        {
            var runner = new TestRunner(Read, Debug.Log, Debug.LogError);
            runner.RunAllTests("DanmakufuTest");
        }
        public static string Read(string filePath)
        {
            if (filePath.EndsWith(".txt"))
            {
                filePath = filePath.Remove(filePath.Length - 4);
            }
            return Resources.Load<TextAsset>(filePath).text;
        }
    }
}
