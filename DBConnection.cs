using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;


namespace WashingMachine
{
    public class DBConnection
    {
        public string DBLocation { get; set; }
        public EventLogger Logger { get; set; } = new EventLogger();
        public SqliteConnection DigucatorConnection { get; set; } //= new SqliteConnection();

        

        public DBConnection(string DataLocation)
        {
            Logger.Log("Initiating Database Connection to " + DataLocation);
            DBLocation = DataLocation + @"/Log/DigucatorData/Downloads.db";
            DigucatorConnection = new SqliteConnection("Data Source = " + DBLocation);
            Logger.Log("Database Connection Established");
        }

        public void TellDB(string SQLCommand)
        {
            SqliteCommand sqliteCommand = new SqliteCommand();
            sqliteCommand.CommandText = "INSERT INTO Downloads VALUES(" + @"" + SQLCommand + @"" + ")";
            TellDB(sqliteCommand);
        }
        public void TellDB(SqliteCommand SQLCommand)
        {
            Logger.Log("DBConnection.TellDB Started: " + SQLCommand.CommandText);
            SQLCommand.Connection = DigucatorConnection;
            try
            {
                DigucatorConnection.Open();
                SQLCommand.ExecuteNonQuery();
                DigucatorConnection.Close();
                Logger.Log("DBConnection.TellDB Sucessful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + SQLCommand.CommandText);
                Logger.Log("DBConnection.TellDB - Error: " + ex.Message);
            }
        }

        public List<string> AskDBforStringList(string SQLRequest)
        {
            try
            {
                DigucatorConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.InnerException);
            }

            SqliteCommand commandLine = new SqliteCommand(SQLRequest);
            commandLine.Connection = DigucatorConnection;
            DigucatorConnection.Open();
            SqliteDataReader dataReader = commandLine.ExecuteReader();
            List<string> returnData = new List<string>();

            while (dataReader.Read())
            {
                returnData.Add(dataReader.GetValue(0).ToString());
            }
            return returnData;
        }
    }
}
