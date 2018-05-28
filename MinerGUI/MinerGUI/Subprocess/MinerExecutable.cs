using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MinerGUI.Subprocess
{
    class MinerExecutable
    {

        internal static void Clear(string algo)
        {
            string path = "./bats";
            string[] fileArray = Directory.GetFiles(path, "*.bat");
            for (int i = 0; i < fileArray.Count(); i++)
            {
                String[] a1 = fileArray[i].Split('\\');
                String[] a2 = a1[a1.Count() - 1].Split('/');
                String b = Regex.Replace(a2[a2.Count() - 1], @"[\d-]", string.Empty);
                if ((b).Equals(algo + ".bat"))
                {
                    try
                    {
                        File.Delete(fileArray[i]);
                    } catch (Exception) { }
                }
            }
        }

        public static String Build(String algo, String exe, String args, bool includeSetX)
        {
            string path = "./bats";
            CreateFolder(path);
            String finalBatPath = GetNextBatName(path, algo);
            String setX = "";
            if (includeSetX)
            {
                setX += "setx GPU_FORCE_64BIT_PTR 1" + Environment.NewLine;
                setX += "setx GPU_MAX_HEAP_SIZE 100" + Environment.NewLine;
                setX += "setx GPU_USE_SYNC_OBJECTS 1" + Environment.NewLine;
                setX += "setx GPU_MAX_ALLOC_PERCENT 100" + Environment.NewLine;
                setX += "setx GPU_SINGLE_ALLOC_PERCENT 100" + Environment.NewLine;
            }
            string launchString = exe + " " + args + Environment.NewLine;
            File.WriteAllText(finalBatPath, setX + launchString);
            return finalBatPath;
        }

        public static String SimpleBuild(String algo, String exe, String data)
        {
            string path = "./bats";
            CreateFolder(path);
            string[] fileArray = Directory.GetFiles(path, "*.bat");
            String finalBatPath = GetNextBatName(path, algo);
            File.WriteAllText(finalBatPath, data);
            return finalBatPath;
        }
        private static void CreateFolder(String path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        private static String GetNextBatName(String path, String algo)
        {

            string[] fileArray = Directory.GetFiles(path, "*.bat");
            string[] fileNamesArray = new String[fileArray.Count()];
            for(int i = 0; i < fileArray.Count(); i++) 
            {
                String[] a1 = fileArray[i].Split('\\');
                String[] a2 = a1[a1.Count()-1].Split('/');
                fileNamesArray[i] = a2[a2.Count() - 1];
            }
            int startBat = 0;
            while (fileNamesArray.Contains(algo + startBat + ".bat"))
            {
                startBat++;
            }
            return path + "/" + algo + startBat + ".bat";
        }
    }
}
