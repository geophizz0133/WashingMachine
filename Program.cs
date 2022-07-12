using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System.Reflection;


namespace WashingMachine
{
    public class Program
    {
      
        static void Main(string[] args)
        {
            Console.WriteLine("K");

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            FileHandler fileHandler = new FileHandler();
            EventLogger logger = new EventLogger();
            string googleDrivePath;

#if (DEBUG)
            googleDrivePath = @"//Volumes/More Space/Google Drive (cantoncommunitytvma@gmail.com)/";
            logger.Log("Debug Version " + version);
#else
            googleDrivePath = @"//Volumes/DriveSync/Google Drive/";
            logger.Log("Release Version " + version);
#endif

            String googlePath;
            googlePath = googleDrivePath + @"From CHS NOT Tightrope ready/000 Cable Submissions/";
            string washedPath = googleDrivePath + @"CHS-Washed Ready for Tightrope";


            Console.WriteLine("Checking for CHS Submissions");
            logger.Log(" Checking '000 Cable Submissions' folder");
            String[] googleFiles = Directory.GetFiles(googlePath);
            logger.Log(googleFiles.Length.ToString() + " Files found");

            if (googleFiles.Length == 0 || (googleFiles.Length ==1 && googleFiles[0].EndsWith(".DS_Store")))
            {
                Console.WriteLine("No new files found");
                logger.Log(" No new files found");
            }
            foreach(string entry in googleFiles)
            { logger.Log(entry); }

            foreach (string googleFile in googleFiles)
            {
                
                ShowHandler showHandler = new ShowHandler(googleDrivePath);
                string googleFileName =  @"""" + Path.GetFileNameWithoutExtension(googleFile) + Path.GetExtension(googleFile) + @"""";
                string outputGoogleFileName = @"""" + Path.GetFileNameWithoutExtension(googleFile) + ".mov" +  @"""";
                Console.WriteLine("Checking " + googleFileName);
                Console.WriteLine("Checking " + outputGoogleFileName);
                logger.Log(googleFileName + " " + showHandler.IsAlreadyDownloaded(googleFileName));
                if ((googleFile.EndsWith(".DS_Store") is false) && (showHandler.IsAlreadyDownloaded(googleFileName) is false))
                {
                    Console.Write("Initiating Transcoding of " + googleFileName + "...");
                    showHandler.TellDB(googleFileName);
                    ffMpegController ffMpeg = new ffMpegController();

                    ffMpeg.InputFile = @"""" + googleFile.Trim() + @"""";
                    ffMpeg.OutputFile = @"""" + washedPath + @"""" + @"/" + outputGoogleFileName.Trim();  //This may need to be changed to get a .MOV output
                    ffMpeg.MakeTightRopeFile(false);
                    logger.Log("Transcoding of " + googleFileName + "complete");

                    Thread.Sleep(5000);
                   
                    Console.WriteLine("DONE");
                }
                else
                {
                    if (showHandler.IsAlreadyDownloaded(googleFileName) is true)
                    { logger.Log("Show " + googleFileName + " already downloaded"); }
                    else
                    {logger.Log("File " + googleFileName + "Not found"); }
                }
            }
            logger.Log(" Done");
        }
    }
}
