using System;
using Gstd.GstdUtility;

namespace test
{
    class PathPropertyTest : TestSuite
    {
        public PathPropertyTest(Func<string, string> readData, Action<string> logInfo, Action<string> logOk, Action<string> logFail) : base(readData, logInfo, logOk, logFail)
        {
        }
        public override void Run()
        {
            Run("[PathPropertyTest] GetFileDirectory", () => {
#if _WINDOWS
                AssertEquals("C:\\MyDir\\MySubDir\\", PathProperty.GetFileDirectory("C:\\MyDir\\MySubDir\\myfile.ext"));
                AssertEquals("C:\\MyDir\\", PathProperty.GetFileDirectory("C:\\MyDir\\MySubDir"));
                AssertEquals("C:\\MyDir\\", PathProperty.GetFileDirectory("C:\\MyDir\\"));
                AssertEquals("C:\\", PathProperty.GetFileDirectory("C:\\MyDir"));
                AssertEquals("C:\\", PathProperty.GetFileDirectory("C:\\"));
                AssertEquals("C:", PathProperty.GetFileDirectory("C:"));
                AssertEquals("", PathProperty.GetFileDirectory(""));
#else
                AssertEquals("/MyDir/MySubDir/", PathProperty.GetFileDirectory("/MyDir/MySubDir/myfile.ext"));
                AssertEquals("/MyDir/", PathProperty.GetFileDirectory("/MyDir/MySubDir"));
                AssertEquals("/MyDir/", PathProperty.GetFileDirectory("/MyDir/"));
                AssertEquals("/", PathProperty.GetFileDirectory("/MyDir"));
                AssertEquals("/", PathProperty.GetFileDirectory("/"));
                AssertEquals("", PathProperty.GetFileDirectory(""));
#endif
            });
            Run("[PathPropertyTest] GetDirectoryName", () => {
#if _WINDOWS
                AssertEquals("MySubDir", PathProperty.GetDirectoryName("C:\\MyDir\\MySubDir\\myfile.ext"));
                AssertEquals("MyDir", PathProperty.GetDirectoryName("C:\\MyDir\\MySubDir"));
                AssertEquals("MyDir", PathProperty.GetDirectoryName("C:\\MyDir\\"));
                AssertEquals("C:", PathProperty.GetDirectoryName("C:\\MyDir"));
                AssertEquals("C:", PathProperty.GetDirectoryName("C:\\"));
                AssertEquals("C:", PathProperty.GetDirectoryName("C:"));
                AssertEquals("", PathProperty.GetDirectoryName(""));
#else
                AssertEquals("MySubDir", PathProperty.GetDirectoryName("/MyDir/MySubDir/myfile.ext"));
                AssertEquals("MyDir", PathProperty.GetDirectoryName("/MyDir/MySubDir"));
                AssertEquals("MyDir", PathProperty.GetDirectoryName("/MyDir/"));
                AssertEquals("", PathProperty.GetDirectoryName("/MyDir"));
                AssertEquals("", PathProperty.GetDirectoryName("/"));
                AssertEquals("", PathProperty.GetDirectoryName(""));
#endif
            });
            Run("[PathPropertyTest] GetFileName", () => {
#if _WINDOWS
                AssertEquals("myfile.ext", PathProperty.GetFileName("C:\\MyDir\\myfile.ext"));
                AssertEquals("", PathProperty.GetFileName("C:\\MyDir\\"));
                AssertEquals("MyDir", PathProperty.GetFileName("C:\\MyDir"));
                AssertEquals("", PathProperty.GetFileName("C:\\"));
                AssertEquals("", PathProperty.GetFileName("C:"));
                AssertEquals("", PathProperty.GetFileName(""));
#else
                AssertEquals("myfile.ext", PathProperty.GetFileName("/MyDir/myfile.ext"));
                AssertEquals("", PathProperty.GetFileName("/MyDir/"));
                AssertEquals("MyDir", PathProperty.GetFileName("/MyDir"));
                AssertEquals("", PathProperty.GetFileName("/"));
                AssertEquals("", PathProperty.GetFileName(""));
#endif
            });
            Run("[PathPropertyTest] GetDriveName", () => {
#if _WINDOWS
                AssertEquals("C:", PathProperty.GetDriveName("C:\\MyDir\\myfile.ext"));
                AssertEquals("C:", PathProperty.GetDriveName("C:\\MyDir\\"));
                AssertEquals("C:", PathProperty.GetDriveName("C:\\MyDir"));
                AssertEquals("C:", PathProperty.GetDriveName("C:\\"));
                AssertEquals("C:", PathProperty.GetDriveName("C:"));
                AssertEquals("", PathProperty.GetDriveName(""));
#else
                AssertEquals("/", PathProperty.GetDriveName("/MyDir/myfile.ext"));
                AssertEquals("/", PathProperty.GetDriveName("/MyDir/"));
                AssertEquals("/", PathProperty.GetDriveName("/MyDir"));
                AssertEquals("/", PathProperty.GetDriveName("/"));
                AssertEquals("", PathProperty.GetDriveName(""));
#endif
            });
            Run("[PathPropertyTest] GetFileNameWithoutExtension", () => {
#if _WINDOWS
                AssertEquals("myfile", PathProperty.GetFileNameWithoutExtension("C:\\MyDir\\myfile.ext"));
                AssertEquals("", PathProperty.GetFileNameWithoutExtension("C:\\MyDir\\"));
                AssertEquals("MyDir", PathProperty.GetFileNameWithoutExtension("C:\\MyDir"));
                AssertEquals("", PathProperty.GetFileNameWithoutExtension("C:\\"));
                AssertEquals("", PathProperty.GetFileNameWithoutExtension("C:"));
                AssertEquals("", PathProperty.GetFileNameWithoutExtension(""));
#else
                AssertEquals("myfile", PathProperty.GetFileNameWithoutExtension("/MyDir/myfile.ext"));
                AssertEquals("", PathProperty.GetFileNameWithoutExtension("/MyDir/"));
                AssertEquals("MyDir", PathProperty.GetFileNameWithoutExtension("/MyDir"));
                AssertEquals("", PathProperty.GetFileNameWithoutExtension("/"));
                AssertEquals("", PathProperty.GetFileNameWithoutExtension(""));
#endif
            });
            Run("[PathPropertyTest] GetFileExtension", () => {
#if _WINDOWS
                AssertEquals(".ext", PathProperty.GetFileExtension("C:\\MyDir\\myfile.ext"));
                AssertEquals("", PathProperty.GetFileExtension("C:\\MyDir\\"));
                AssertEquals("", PathProperty.GetFileExtension("C:\\MyDir"));
                AssertEquals("", PathProperty.GetFileExtension("C:\\"));
                AssertEquals("", PathProperty.GetFileExtension("C:"));
                AssertEquals("", PathProperty.GetFileExtension(""));
#else
                AssertEquals(".ext", PathProperty.GetFileExtension("/MyDir/myfile.ext"));
                AssertEquals("", PathProperty.GetFileExtension("/MyDir/"));
                AssertEquals("", PathProperty.GetFileExtension("/MyDir"));
                AssertEquals("", PathProperty.GetFileExtension("/"));
                AssertEquals("", PathProperty.GetFileExtension(""));
#endif
            });
            Run("[PathPropertyTest] GetModuleName", () => {
#if _WINDOWS
                AssertEquals("Danmakufu", PathProperty.GetModuleName());
#else
                AssertEquals("Danmakufu", PathProperty.GetModuleName());
#endif
            });
            Run("[PathPropertyTest] GetModuleDirectory", () => {
#if _UNITY
                AssertEquals("", PathProperty.GetModuleDirectory());
#elif _WINDOWS
                AssertEquals("D:\\Repository\\Test\\bin\\Debug\\net5.0\\", PathProperty.GetModuleDirectory());
#else
                AssertEquals("/Users/wilson/Desktop/ph3/Test/bin/Debug/net5.0/", PathProperty.GetModuleDirectory());
#endif
            });
            Run("[PathPropertyTest] GetDirectoryWithoutModuleDirectory", () => {
#if _UNITY
#elif _WINDOWS
				AssertEquals("MyDir\\MySubDir\\", PathProperty.GetDirectoryWithoutModuleDirectory("D:\\Repository\\Test\\bin\\Debug\\net5.0\\MyDir\\MySubDir\\myfile.ext"));
				AssertEquals("MyDir\\", PathProperty.GetDirectoryWithoutModuleDirectory("D:\\Repository\\Test\\bin\\Debug\\net5.0\\MyDir\\MySubDir"));
				AssertEquals("MyDir\\", PathProperty.GetDirectoryWithoutModuleDirectory("D:\\Repository\\Test\\bin\\Debug\\net5.0\\MyDir\\"));
				AssertEquals("", PathProperty.GetDirectoryWithoutModuleDirectory("D:\\Repository\\Test\\bin\\Debug\\net5.0\\MyDir"));
				AssertEquals("", PathProperty.GetDirectoryWithoutModuleDirectory("D:\\Repository\\Test\\bin\\Debug\\net5.0\\"));
				AssertEquals("\\a\\b\\path\\", PathProperty.GetDirectoryWithoutModuleDirectory("/a/b/path/"));
#else
				AssertEquals("MyDir/MySubDir/", PathProperty.GetDirectoryWithoutModuleDirectory("/Users/wilson/Desktop/ph3/Test/bin/Debug/net5.0/MyDir/MySubDir/myfile.ext"));
				AssertEquals("MyDir/", PathProperty.GetDirectoryWithoutModuleDirectory("/Users/wilson/Desktop/ph3/Test/bin/Debug/net5.0/MyDir/MySubDir"));
				AssertEquals("MyDir/", PathProperty.GetDirectoryWithoutModuleDirectory("/Users/wilson/Desktop/ph3/Test/bin/Debug/net5.0/MyDir/"));
				AssertEquals("", PathProperty.GetDirectoryWithoutModuleDirectory("/Users/wilson/Desktop/ph3/Test/bin/Debug/net5.0/MyDir"));
				AssertEquals("", PathProperty.GetDirectoryWithoutModuleDirectory("/Users/wilson/Desktop/ph3/Test/bin/Debug/net5.0/"));
				AssertEquals("/a/b/path/", PathProperty.GetDirectoryWithoutModuleDirectory("/a/b/path/"));
#endif
            });
            Run("[PathPropertyTest] GetPathWithoutModuleDirectory", () => {
#if _UNITY
#elif _WINDOWS
                AssertEquals("MyDir\\MySubDir\\myfile.ext", PathProperty.GetPathWithoutModuleDirectory("D:\\Repository\\Test\\bin\\Debug\\net5.0\\MyDir/MySubDir/myfile.ext"));
#else
                AssertEquals("MyDir/MySubDir/myfile.ext", PathProperty.GetPathWithoutModuleDirectory("/Users/wilson/Desktop/ph3/Test/bin/Debug/net5.0/MyDir/MySubDir/myfile.ext"));
#endif
                // TODO dirModule needs Canonicalize
                // dirModule => D:/Repository/danmakufu-ph3/Test/./bin_th_dnh/
                // path => D:/Repository/danmakufu-ph3/Test/bin_th_dnh/MyDir/MySubDir/myfile.ext
                //AssertEquals("D:/Repository/danmakufu-ph3/Test/bin_th_dnh/MyDir/MySubDir/myfile.ext", PathProperty.GetPathWithoutModuleDirectory(std::wstring(L"D:\\Repository\\danmakufu-ph3\\Test\\bin_th_dnh\\MyDir\\MySubDir\\myfile.ext"))));
            });
            Run("[PathPropertyTest] GetRelativeDirectory", () => {
#if _WINDOWS
                AssertEquals("..\\..\\x\\y\\file\\", PathProperty.GetRelativeDirectory("C:\\a\\b\\path\\", "C:\\a\\x\\y\\file\\"));
                AssertEquals("..\\..\\x\\y\\file\\", PathProperty.GetRelativeDirectory("C:\\a\\b\\path", "C:\\a\\x\\y\\file\\"));
                AssertEquals("..\\..\\x\\y\\", PathProperty.GetRelativeDirectory("C:\\a\\b\\path\\", "C:\\a\\x\\y\\file"));
                AssertEquals("..\\..\\x\\y\\", PathProperty.GetRelativeDirectory("C:\\a\\b\\path", "C:\\a\\x\\y\\file"));
                AssertEquals("..\\..\\..\\x\\y\\file\\", PathProperty.GetRelativeDirectory("C:\\a\\b\\path\\myfile.ext", "C:\\a\\x\\y\\file\\"));
                AssertEquals("", PathProperty.GetRelativeDirectory("C:\\a\\b\\path", "D:\\a\\x\\y\\file"));
                AssertEquals("", PathProperty.GetRelativeDirectory("C:\\a\\b\\path", ""));
#else
                AssertEquals("../../x/y/file/", PathProperty.GetRelativeDirectory("/a/b/path/", "/a/x/y/file/"));
                AssertEquals("../../x/y/file/", PathProperty.GetRelativeDirectory("/a/b/path", "/a/x/y/file/"));
                AssertEquals("../../x/y/", PathProperty.GetRelativeDirectory("/a/b/path/", "/a/x/y/file"));
                AssertEquals("../../x/y/", PathProperty.GetRelativeDirectory("/a/b/path", "/a/x/y/file"));
                AssertEquals("../../../x/y/file/", PathProperty.GetRelativeDirectory("/a/b/path/myfile.ext", "/a/x/y/file/"));
                AssertEquals("", PathProperty.GetRelativeDirectory("/a/b/path", ""));
#endif
            });
            Run("[PathPropertyTest] ReplaceYenToSlash", () => {
#if _WINDOWS
                AssertEquals("C:/MyDir/myfile.ext", PathProperty.ReplaceYenToSlash("C:\\MyDir\\myfile.ext"));
                AssertEquals("C:/MyDir/", PathProperty.ReplaceYenToSlash("C:\\MyDir\\"));
                AssertEquals("C:/MyDir", PathProperty.ReplaceYenToSlash("C:\\MyDir"));
                AssertEquals("C:/", PathProperty.ReplaceYenToSlash("C:\\"));
                AssertEquals("C:", PathProperty.ReplaceYenToSlash("C:"));
                AssertEquals("", PathProperty.ReplaceYenToSlash(""));
#else
                AssertEquals("/MyDir/myfile.ext", PathProperty.ReplaceYenToSlash("\\MyDir\\myfile.ext"));
                AssertEquals("/MyDir/", PathProperty.ReplaceYenToSlash("\\MyDir\\"));
                AssertEquals("/MyDir", PathProperty.ReplaceYenToSlash("\\MyDir"));
                AssertEquals("/", PathProperty.ReplaceYenToSlash("\\"));
                AssertEquals("", PathProperty.ReplaceYenToSlash(""));
#endif
            });
            Run("[PathPropertyTest] Canonicalize", () => {
#if _WINDOWS
                AssertEquals("C:\\name_1\\name_3", PathProperty.Canonicalize("C:\\name_1\\.\\name_2\\..\\name_3"));
                AssertEquals("C:\\name_2\\name_3", PathProperty.Canonicalize("C:\\name_1\\..\\name_2\\.\\name_3"));
                AssertEquals("C:\\name_1\\name_2\\name_4", PathProperty.Canonicalize("C:\\name_1\\name_2\\.\\name_3\\..\\name_4"));
                AssertEquals("C:\\name_1\\name_2", PathProperty.Canonicalize("C:\\name_1\\.\\name_2\\.\\name_3\\..\\name_4\\.."));
                AssertEquals("C:\\", PathProperty.Canonicalize("C:\\.."));
#else
                AssertEquals("/name_1/name_3", PathProperty.Canonicalize("/name_1/./name_2/../name_3"));
                AssertEquals("/name_2/name_3", PathProperty.Canonicalize("/name_1/../name_2/./name_3"));
                AssertEquals("/name_1/name_2/name_4", PathProperty.Canonicalize("/name_1/name_2/./name_3/../name_4"));
                AssertEquals("/name_1/name_2", PathProperty.Canonicalize("/name_1/./name_2/./name_3/../name_4/.."));
                AssertEquals("/", PathProperty.Canonicalize("/.."));
#endif
            });
            Run("[PathPropertyTest] GetUnique", () => {
#if _WINDOWS
                AssertEquals("C:/name_1/name_3", PathProperty.GetUnique("C:\\name_1\\.\\name_2\\..\\name_3"));
                AssertEquals("C:/name_2/name_3", PathProperty.GetUnique("C:\\name_1\\..\\name_2\\.\\name_3"));
                AssertEquals("C:/name_1/name_2/name_4", PathProperty.GetUnique("C:\\name_1\\name_2\\.\\name_3\\..\\name_4"));
                AssertEquals("C:/name_1/name_2", PathProperty.GetUnique("C:\\name_1\\.\\name_2\\.\\name_3\\..\\name_4\\.."));
                AssertEquals("C:/", PathProperty.GetUnique("C:\\.."));
#else
                AssertEquals("/name_1/name_3", PathProperty.GetUnique("/name_1/./name_2/../name_3"));
                AssertEquals("/name_2/name_3", PathProperty.GetUnique("/name_1/../name_2/./name_3"));
                AssertEquals("/name_1/name_2/name_4", PathProperty.GetUnique("/name_1/name_2/./name_3/../name_4"));
                AssertEquals("/name_1/name_2", PathProperty.GetUnique("/name_1/./name_2/./name_3/../name_4/.."));
                AssertEquals("/", PathProperty.GetUnique("/.."));
#endif
            });
        }
    }
}
