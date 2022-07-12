using System;
using System.IO;
using System.Text;

namespace WashingMachine
{
    class FileHandler
    {
        protected EventLogger logger = new EventLogger();

        public FileHandler()
        {
            
        }
        public void PushToTightRope(string inputGoogleFilePath,string inputGoogleFileName)
        {
            string inputGoogleFile = inputGoogleFilePath + @"/" + inputGoogleFileName;
            string outputTightRopePath = @"//Volumes/Content/";
            string outPutTightRopeFileName = outputTightRopePath + inputGoogleFileName;

            logger.Log(" Inititing File Transfer " + inputGoogleFileName);

            try
            {
                File.Copy(inputGoogleFile, outPutTightRopeFileName);
                logger.Log(" Uploaded " + outPutTightRopeFileName + " successfully");
            }
            catch (Exception ex)
            {
                logger.Log(" Upload Failed " + inputGoogleFileName + " " + ex.StackTrace.ToString());
            }
            finally
            {
                logger.Log(" File Move attempt complete");
            }
        }

        public void CleanupGoogleFolders(string inputGoogleFilePath, string inputGoogleFileName)
        {
            logger.Log(" Initiating Google Folder cleanup");
            string inputGoogleFile = inputGoogleFilePath + @"/" + inputGoogleFileName;
            string doneGoogleFile = @"//Volumes/DriveSync/Uploaded/" + inputGoogleFileName;
            try
            {
                File.Move(inputGoogleFile, doneGoogleFile, true);
            }
            catch (Exception ex)
            {
                logger.Log("  Google File cleanup failed " + ex.StackTrace.ToString());
            }
            logger.Log(" Google Drive cleanup complete");
        }
    }
}
