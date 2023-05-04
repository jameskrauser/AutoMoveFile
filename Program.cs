using System;
using System.IO;
using System.Threading;
using System.Text;
using System.Data.SqlTypes;

//OperatingSystem: Mac
//before: add permision with source Folder and destination folder

// ini.txt file
//source:/ Users / james / Desktop / Youtube /
//destination:/ Volumes / SSD1T / Youtube /
//interval:1

// how to running
// open terminal with dotnet ./Move2.dll

namespace DirectoryMover
{
    class Program
    {
        static void Main(string[] args)
        {

            string filePath = @"ini.txt";
            
            //read param from ini.txt file
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist :{0} ", filePath);
                return;
            }

            string[] textFromFile = File.ReadAllLines(filePath);
            foreach (string line in textFromFile)
            {
                Console.WriteLine("read " + line);
            }
            string StartFolderName = textFromFile[0].Trim().Substring(7);
            string dFolderName = textFromFile[1].Trim().Substring(12);
            string interval_s = textFromFile[2].Trim().Substring(9);
            int interval = int.Parse(interval_s);

            //this is mac os read file formate
            //string path = @"file:/Users/james/dwhelper/A.mp4";
            //string path2 = @"file:/Volumes/SSD1T/dwhelper3/A.mp4";


            string subfolderName = "";
            Console.WriteLine("Auth: Jameskrauser version:20230504v1");
            Console.WriteLine("source location: " + StartFolderName);
            Console.WriteLine("Destination location: " + dFolderName);
            Console.WriteLine("Check folder every " + interval + " mins");
            Console.WriteLine("Press ctrl Z to stop");
            Console.SetCursorPosition(0, 3);
            Console.Write("Checking time " + DateTime.Now.ToString("HH:mm:ss"));

            while (true)
            {


                try
                {
                    //get sub folder name 
                    foreach (string s in Directory.GetDirectories(StartFolderName))
                    {
                        subfolderName = StartFolderName + s.Remove(0, StartFolderName.Length) + "/";
                        string[] allFiles = Directory.GetFiles(subfolderName, "*.mp4");

                        string checkFolderexist = dFolderName + s.Remove(0, StartFolderName.Length) + "/";
                        // check destination folder exist otherwise create folder.
                        if (!Directory.Exists(checkFolderexist))
                        {
                            Directory.CreateDirectory(checkFolderexist);
                        }

                        foreach (var file in allFiles)
                        {
                            var fileName = Path.GetFileNameWithoutExtension(file);
                            var extension = Path.GetExtension(file);
                            var destFileName = checkFolderexist + fileName + extension;

                            if (!File.Exists(destFileName))
                            {
                                //File.Copy(file, destFileName);
                                File.Move(file, destFileName);

                            }
                            else
                            {
                                Console.WriteLine(" file exists, dont move file to SSD:" + destFileName);
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                //1000*60 = 1 min

                Thread.Sleep(1000 * 60 * interval);
                Console.SetCursorPosition(0, 3);
                Console.Write("Checking time " + DateTime.Now.ToString("HH:mm:ss"));

            }
        }

     
    }
}