using System;
using System.IO;

namespace WashingMachine
{
    public class EventLogger
    {


        String googleLogFile = @"//Volumes/DriveSync/Google Drive/Log/WashingMachineLog.txt";


        public EventLogger()
        {
#if (DEBUG)
                googleLogFile = @"//Volumes/More Space/CHSGoogledrive/My Drive/Log/WashingMachineLog.txt";
#endif
        }

        public void Log(string Message)
        {
            File.AppendAllText(googleLogFile, DateTime.Now.ToString() + " " + Message + Environment.NewLine);
        }
    }
}
