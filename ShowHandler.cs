using System;
using System.Collections.Generic;


namespace WashingMachine
{
    public class ShowHandler
    {
        EventLogger logger = new EventLogger();
        string sqlFilePath;
        public ShowHandler(string dbFilePath)
        {
            sqlFilePath = dbFilePath;
        }

        public void TellDB(string FileName)
        {
            DBConnection DigucatorConnection = new DBConnection(sqlFilePath);
            DigucatorConnection.TellDB(FileName);
        }

        public bool IsAlreadyDownloaded(string FileName)
        {
            DBConnection DigucatorDB = new DBConnection(sqlFilePath);
            string sqlString = @"SELECT FileName from Downloads Where FileName = " + FileName;
            logger.Log("Checking DB for " + FileName + Environment.NewLine + sqlString);
            List<string> downloadedShows = DigucatorDB.AskDBforStringList(sqlString);
            foreach (string showname in downloadedShows)
            {
                logger.Log(showname + " found in DB");
            } 
            if (downloadedShows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
