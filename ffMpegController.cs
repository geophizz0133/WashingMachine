using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WashingMachine
{
    class ffMpegController
    {
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public string InputFPS { get; set; } 
        public string OutputFPS { get; set; }
        public string OutputScale { get; set; }
        public string OutputCodec { get; set; }
        EventLogger logger = new EventLogger();

        public void MakeFile(string optionalOutputScale = "1920",string optionalOutputFPS = "59.94") //The optional are set to maintain backward compatibility with MakeTightRopeFile
        {
            OutputFPS = optionalOutputFPS;
            OutputScale = optionalOutputScale;
            StringBuilder sbffMpegCommand = new StringBuilder();

            sbffMpegCommand.Append(" -i " + InputFile + " -vf scale=" + OutputScale + ":-1" + " -r " + OutputFPS + " " + OutputFile); //Experiment - This works for everything except AVI files
            RunffMpeg(sbffMpegCommand.ToString());

        }

        public void MakeTightRopeFile()
        {
            OutputFPS = "59.94";
            OutputScale = "720";
            StringBuilder sbffMpegCommand = new StringBuilder();

            sbffMpegCommand.Append(" -i " + InputFile + " -vf scale=" + OutputScale + ":-1" + " -r " + OutputFPS + " " + OutputFile); //Experiment - This works for everything except AVI files
            //sbffMpegCommand.Append(" -i " + InputFile + " -vf scale=1920:1080:force_original_aspect_ratio=decrease,pad=1920:1080:" + " -r " + OutputFPS + " " + OutputFile); //Experiment for converting any sized Zoom files
            RunffMpeg(sbffMpegCommand.ToString());
        }

        public void MakeTightRopeFile(bool ForceAspectRatio)
        {
            logger.Log("Executing MakeTightRopeFile ");
            OutputFPS = "59.94";
            OutputScale = "720";
            StringBuilder sbffMpegCommand = new StringBuilder();

            if (ForceAspectRatio == true)
            {
                sbffMpegCommand.Append(" -i " + InputFile + " -vf scale=" + OutputScale + ":-1" + " -r " + OutputFPS + " " + OutputFile); //Experiment - This works for everything except AVI files
            }
            else
            {
                sbffMpegCommand.Append(" -i " + InputFile + " -vf scale=1280:720:force_original_aspect_ratio=decrease,pad=1280:720:" + " -r " + OutputFPS + " " + OutputFile); //Experiment for converting any sized Zoom files
            }

            logger.Log("Starting ffMpeg with the commandline " + sbffMpegCommand.ToString());

            try
            {
                RunffMpeg(sbffMpegCommand.ToString());
            }
            catch (Exception ex)
            {
                logger.Log(ex.ToString());
            }
                   
              
        }


        public void RunffMpeg(string commandline)
        {
#if (DEBUG)

            //NOTE - for the Mac, ffmpeg has to be copied to the bin folder manually
            Process ffMpeg = Process.Start("ffmpeg", commandline);
            ffMpeg.WaitForExit();
#else
            Process ffMpeg = new Process();
            ffMpeg.StartInfo.WorkingDirectory = @"/Applications/WashingMachine/ffmpeg";
            ffMpeg.Start();
            ffMpeg.WaitForExit();
#endif

        }
    }
}
