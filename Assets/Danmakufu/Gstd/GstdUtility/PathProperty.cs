using System.IO;
#if _UNITY
using System;
using System.Text;
#endif

namespace Gstd
{
    namespace GstdUtility
    {
        sealed class PathProperty
        {
            public static string GetFileDirectory(string path)
            {
                if ("" == path)
                {
                    return "";
                }
#if _WINDOWS
                string fileDirectory = Path.GetDirectoryName(path);
                if (null == fileDirectory)
                {
                    fileDirectory = Path.GetPathRoot(path);
                }
                else if ("\\" != fileDirectory && !fileDirectory.EndsWith("\\"))
                {
                    fileDirectory += '\\';
                }
                return fileDirectory;
#else
                if ("/" == path)
                {
                    return "/";
                }
                string fileDirectory = Path.GetDirectoryName(path);
                if (null == fileDirectory)
                {
                    fileDirectory = "";
                }
                else if ("/" != fileDirectory)
                {
                    fileDirectory += '/';
                }
                return fileDirectory;
#endif
            }
            public static string GetDirectoryName(string path)
            {
                string dir = GetFileDirectory(path);
                if ("" == dir)
                {
                    return "";
                }
#if _WINDOWS
                dir = dir.Replace(@"\", @"/");
#else
                if ("/" == dir)
                {
                    return "";
                }
#endif
                string[] strs = dir.Split('/');
                if (strs.Length < 2)
                {
#if _WINDOWS
                    return Path.GetPathRoot(path);
#else
                    return "";
#endif
                }
                return strs[strs.Length - 2];
            }
            public static string GetFileName(string path)
            {
                return Path.GetFileName(path);
            }
            public static string GetDriveName(string path)
            {
                if ("" == path)
                {
                    return "";
                }
                string driveName = Path.GetPathRoot(path);
                if (null == driveName)
                {
                    return "";
                }
#if _WINDOWS
                if (driveName.EndsWith("\\"))
                {
                    driveName = driveName.Remove(driveName.Length - 1);
                }
#endif
                return driveName;
            }
            public static string GetFileNameWithoutExtension(string path)
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            public static string GetFileExtension(string path)
            {
                return Path.GetExtension(path);
            }
            public static string GetModuleName()
            {
#if _UNITY
                return "Danmakufu";
#else
                string path = System.Reflection.Assembly.GetEntryAssembly().Location;
                return GetFileNameWithoutExtension(path);
#endif
            }
            public static string GetModuleDirectory()
            {
#if _UNITY
                return "";
#else
                string path = System.Reflection.Assembly.GetEntryAssembly().Location;
                return GetFileDirectory(path);
#endif
            }
            public static string GetDirectoryWithoutModuleDirectory(string path)
            {
#if _UNITY
                return path;
#else
                string res = GetFileDirectory(path);
                string dirModule = GetModuleDirectory();
                if (res.StartsWith(dirModule))
                {
                    res = res.Substring(dirModule.Length);
                }
                return res;
#endif
            }
            public static string GetPathWithoutModuleDirectory(string path)
            {
                string dirModule = GetModuleDirectory();
                dirModule = Canonicalize(dirModule);
#if _WINDOWS
#else
                dirModule = ReplaceYenToSlash(dirModule);
#endif
                path = Canonicalize(path);
#if _WINDOWS
#else
                path = ReplaceYenToSlash(path);
#endif

                string res = path;
                if (res.StartsWith(dirModule))
                {
                    res = res.Substring(dirModule.Length);
                }
                return res;
            }
            public static string GetRelativeDirectory(string from, string to)
            {
                try
                {
#if _UNITY
                    string path = RelativePath(from, to);
#else
                    string path = Path.GetRelativePath(from, to);
#endif
                    if (path != from && path != to)
                    {
                        return GetFileDirectory(path);
                    }
                    return "";
                }
                catch (System.ArgumentException)
                {
                    return "";
                }
            }
            public static string ReplaceYenToSlash(string path)
            {
                string res = path.Replace(@"\", @"/");
                return res;
            }
            public static string Canonicalize(string srcPath)
            {
                return Path.GetFullPath(srcPath);
            }
            public static string GetUnique(string srcPath)
            {
                string res = srcPath.Replace(@"\", @"/");
                res = Canonicalize(res);
                res = ReplaceYenToSlash(res);
                return res;
            }
#if _UNITY
            private static string RelativePath(string absPath, string relTo)
            {
                string[] absDirs = absPath.Split('\\');
                string[] relDirs = relTo.Split('\\');

                // Get the shortest of the two paths
                int len = absDirs.Length < relDirs.Length ? absDirs.Length :
                relDirs.Length;

                // Use to determine where in the loop we exited
                int lastCommonRoot = -1;
                int index;

                // Find common root
                for (index = 0; index < len; index++)
                {
                    if (absDirs[index] == relDirs[index]) lastCommonRoot = index;
                    else break;
                }

                // If we didn't find a common prefix then throw
                if (lastCommonRoot == -1)
                {
                    throw new ArgumentException("Paths do not have a common base");
                }

                // Build up the relative path
                StringBuilder relativePath = new StringBuilder();

                // Add on the ..
                for (index = lastCommonRoot + 1; index < absDirs.Length; index++)
                {
                    if (absDirs[index].Length > 0) relativePath.Append("..\\");
                }

                // Add on the folders
                for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++)
                {
                    relativePath.Append(relDirs[index] + "\\");
                }
                relativePath.Append(relDirs[relDirs.Length - 1]);

                return relativePath.ToString();
            }
#endif
        }
    }
}
