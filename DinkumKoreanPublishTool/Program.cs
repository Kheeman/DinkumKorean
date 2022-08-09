using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;

namespace DinkumKoreanPublishTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            // 패키지 폴더 생성
            DirectoryInfo buildDir = new DirectoryInfo("DinkumKoreanPublish");
            if (buildDir.Exists)
            {
                buildDir.Delete(true);
            }
            buildDir.Create();
            DirectoryInfo buildDir1 = new DirectoryInfo("DinkumKoreanPublish/DinkumKorean");
            if (buildDir1.Exists)
            {
                buildDir1.Delete(true);
            }
            buildDir1.Create();
            DirectoryInfo buildDir2 = new DirectoryInfo("DinkumKoreanPublish/DinkumKorean_WithBepInEx");
            if (buildDir2.Exists)
            {
                buildDir2.Delete(true);
            }
            buildDir2.Create();
            // Bep 없는 버전
            Console.WriteLine("BepInEx 없는 버전 패키징 시작...");
            // 구성 파일 복사
            CopyFile("BepInEx/config/xiaoye97.I2LocPatch.cfg", "DinkumKoreanPublish/DinkumKorean/BepInEx/config/xiaoye97.I2LocPatch.cfg");
            CopyFile("BepInEx/config/xiaoye97.I18NFont4UnityGame.cfg", "DinkumKoreanPublish/DinkumKorean/BepInEx/config/xiaoye97.I18NFont4UnityGame.cfg");
            // 플러그인 복사
            CopyFile("BepInEx/plugins/DinkumKorean.dll", "DinkumKoreanPublish/DinkumKorean/BepInEx/plugins/DinkumKorean.dll");
            CopyFile("BepInEx/plugins/I2LocPatch.dll", "DinkumKoreanPublish/DinkumKorean/BepInEx/plugins/I2LocPatch.dll");
            CopyFile("BepInEx/plugins/I18NFont4UnityGame.dll", "DinkumKoreanPublish/DinkumKorean/BepInEx/plugins/I18NFont4UnityGame.dll");
            CopyFile("BepInEx/plugins/XYModLib.dll", "DinkumKoreanPublish/DinkumKorean/BepInEx/plugins/XYModLib.dll");
            CopyFile("BepInEx/plugins/Newtonsoft.Json.dll", "DinkumKoreanPublish/DinkumKorean/BepInEx/plugins/Newtonsoft.Json.dll");
            // 글꼴 및 텍스트 복사
            CopyDirectory("BepInEx/plugins/I2LocPatch", "DinkumKoreanPublish/DinkumKorean/BepInEx/plugins/I2LocPatch");
            CopyDirectory("BepInEx/plugins/I18NFont4UnityGame", "DinkumKoreanPublish/DinkumKorean/BepInEx/plugins/I18NFont4UnityGame");
            // 폴더 압축
            Console.WriteLine("BepInEx 없는 버전 압축 시작...");
            ZipFile("DinkumKoreanPublish/DinkumKorean", "DinkumKorean_V1_X_0.zip");

            // Bep 버전
            Console.WriteLine("BepInEx버전 패키징 시작...");
            // CopyBepInEx
            CopyDirectory("BepInEx/core", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/BepInEx/core");
            CopyFile("doorstop_config.ini", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/doorstop_config.ini");
            CopyFile("winhttp.dll", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/winhttp.dll");
            // 구성 파일 복사
            CopyFile("BepInEx/config/xiaoye97.I2LocPatch.cfg", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/BepInEx/config/xiaoye97.I2LocPatch.cfg");
            CopyFile("BepInEx/config/xiaoye97.I18NFont4UnityGame.cfg", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/BepInEx/config/xiaoye97.I18NFont4UnityGame.cfg");
            // 플러그인 복사
            CopyFile("BepInEx/plugins/DinkumKorean.dll", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/BepInEx/plugins/DinkumKorean.dll");
            CopyFile("BepInEx/plugins/I2LocPatch.dll", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/BepInEx/plugins/I2LocPatch.dll");
            CopyFile("BepInEx/plugins/I18NFont4UnityGame.dll", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/BepInEx/plugins/I18NFont4UnityGame.dll");
            CopyFile("BepInEx/plugins/XYModLib.dll", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/BepInEx/plugins/XYModLib.dll");
            CopyFile("BepInEx/plugins/Newtonsoft.Json.dll", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/BepInEx/plugins/Newtonsoft.Json.dll");
            // 글꼴 및 텍스트 복사
            CopyDirectory("BepInEx/plugins/I2LocPatch", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/BepInEx/plugins/I2LocPatch");
            CopyDirectory("BepInEx/plugins/I18NFont4UnityGame", "DinkumKoreanPublish/DinkumKorean_WithBepInEx/BepInEx/plugins/I18NFont4UnityGame");
            // 폴더 압축
            Console.WriteLine("BepInEx버전 압축 시작...");
            ZipFile("DinkumKoreanPublish/DinkumKorean_WithBepInEx", "DinkumKorean_V1_X_0_WithBepInEx.zip");
            sw.Stop();
            Console.WriteLine($"실행완료, 총 시간{sw.ElapsedMilliseconds}ms");
            Console.ReadLine();
        }

        public static void CopyDirectory(string srcPath, string destPath, List<string> ignoreDirs = null, List<string> ignoreFiles = null)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                if (ignoreDirs == null) ignoreDirs = new List<string>();
                if (ignoreFiles == null) ignoreFiles = new List<string>();
                DirectoryInfo dir = new DirectoryInfo(srcPath); FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //디렉토리 아래의 파일 및 하위 디렉토리 가져오기(하위 디렉토리 제외)
                foreach (FileSystemInfo i in fileinfo)
                {
                    // 폴더인지 확인
                    if (i is DirectoryInfo)
                    {
                        // 무시해야 하는지 확인하고, 무시해야 하는 경우 건너뜀.
                        if (ignoreDirs.Contains(i.Name))
                        {
                            continue;
                        }
                        if (!Directory.Exists(destPath + "\\" + i.Name))
                        {
                            // 이 폴더가 대상 디렉터리에 없으면 하위 폴더를 만듭니다.
                            Directory.CreateDirectory(destPath + "\\" + i.Name);
                        }
                        // 하위 폴더 복사를 위한 재귀 호출
                        CopyDirectory(i.FullName, destPath + "\\" + i.Name, ignoreDirs, ignoreFiles);
                    }
                    else
                    {
                        // 무시해야 하는지 확인하고, 무시해야 하는 경우 건너뜀.
                        if (ignoreFiles.Contains(i.Name))
                        {
                            continue;
                        }
                        // 폴더가 아닌 경우 파일을 복사하고 true는 동일한 이름의 파일을 덮어쓸 수 있음을 의미합니다.
                        File.Copy(i.FullName, destPath + "\\" + i.Name, true);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void CopyFilesWithSearch(string srcPath, string destPath, List<string> fileContainsName = null)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                if (fileContainsName == null) fileContainsName = new List<string>();
                DirectoryInfo dir = new DirectoryInfo(srcPath); FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //디렉토리 아래의 파일 및 하위 디렉토리 가져오기(하위 디렉토리 제외)
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is FileInfo)
                    {
                        bool canCopy = false;
                        // 복사가 필요한지 확인
                        foreach (var name in fileContainsName)
                        {
                            if (i.Name.Contains(name))
                            {
                                canCopy = true;
                                break;
                            }
                        }
                        if (canCopy)
                        {
                            // 폴더가 아닌 경우 파일을 복사하고 true는 동일한 이름의 파일을 덮어쓸 수 있음을 의미합니다.
                            File.Copy(i.FullName, destPath + "\\" + i.Name, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void CopyFile(string srcPath, string destPath)
        {
            FileInfo src = new FileInfo(srcPath);
            if (src.Exists)
            {
                FileInfo dest = new FileInfo(destPath);
                if (!dest.Directory.Exists)
                {
                    dest.Directory.Create();
                }
                File.Copy(src.FullName, destPath, true);
            }
        }

        #region 文件压缩

        /// <summary>
        /// 将文件夹压缩
        /// </summary>
        /// <param name="srcFiles">文件夹路径</param>
        /// <param name="strZip">压缩之后的名称</param>
        public static void ZipFile(string srcFiles, string strZip)
        {
            ZipOutputStream zipStream = null;
            try
            {
                var len = srcFiles.Length;
                var strlen = srcFiles[len - 1];
                if (srcFiles[srcFiles.Length - 1] != Path.DirectorySeparatorChar)
                {
                    srcFiles += Path.DirectorySeparatorChar;
                }
                zipStream = new ZipOutputStream(File.Create(strZip));
                zipStream.SetLevel(6);
                zip(srcFiles, zipStream, srcFiles);
                zipStream.Finish();
                zipStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Clear Resource
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
            }
        }

        /// <summary>
        /// 将文件夹压缩
        /// </summary>
        /// <param name="srcFiles">文件夹路径</param>
        /// <param name="outstream">压缩包流</param>
        /// <param name="strZip">压缩之后的名称</param>
        public static void zip(string srcFiles, ZipOutputStream outstream, string staticFile)
        {
            if (srcFiles[srcFiles.Length - 1] != Path.DirectorySeparatorChar)
            {
                srcFiles += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();
            //获取指定目录下所有文件和子目录文件名称
            string[] filenames = Directory.GetFileSystemEntries(srcFiles);
            //遍历文件
            foreach (string file in filenames)
            {
                if (Directory.Exists(file))
                {
                    zip(file, outstream, staticFile);
                }
                //否则，直接压缩文件
                else
                {
                    //打开文件
                    FileStream fs = File.OpenRead(file);
                    //定义缓存区对象
                    byte[] buffer = new byte[fs.Length];
                    //通过字符流，读取文件
                    fs.Read(buffer, 0, buffer.Length);
                    //得到目录下的文件（比如:D:\Debug1\test）,test
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    outstream.PutNextEntry(entry);
                    //写文件
                    outstream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        #endregion 文件压缩
    }
}