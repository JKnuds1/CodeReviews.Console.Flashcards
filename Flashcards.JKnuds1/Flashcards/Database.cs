using System;
using System.Data.SqlClient;
using Dapper;

namespace Flashcards
{
    class Database
    {
        //public string connectionstring = "DataSource = FlashcardDatabase.db";  
        internal void CreateDatabase()
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection Succesful");
                }

                catch (SqlException ex)
                {
                    Console.WriteLine("Connection failed");
                    Console.WriteLine(ex.Message);
                }

            }
            Console.WriteLine("Finished");
        }
        internal static void CreateFlashCardTable()
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                connection.Open();
                connection.Execute("IF NOT EXISTS (" +
                                   "SELECT * FROM INFORMATION_SCHEMA.TABLES " +
                                   "WHERE TABLE_NAME = 'Flashcards') " +
                                   "BEGIN " +
                                   "CREATE TABLE dbo.Flashcards (" +
                                   "Id INT IDENTITY(1,1) PRIMARY KEY, " +
                                   "Front VARCHAR(50), " +
                                   "Back VARCHAR(50), " +
                                   "Stack VARCHAR(50) FOREIGN KEY REFERENCES Stack(Name) ON DELETE CASCADE); " +
                                   "END");


                connection.Close();
            }
        }
        internal static void CreateStudySessionTable()
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                connection.Open();
                connection.Execute("IF NOT EXISTS (" +
                                   "SELECT * FROM INFORMATION_SCHEMA.TABLES " +
                                   "WHERE TABLE_NAME = 'StudySession') " +
                                   "BEGIN " +
                                   "CREATE TABLE dbo.StudySession (" +
                                   "Id INT IDENTITY(1,1) PRIMARY KEY, " +
                                   "Date VARCHAR(50), " +
                                   "Score INT, " +
                                   "Stack VARCHAR(50) FOREIGN KEY REFERENCES Stack(Name) ON DELETE CASCADE); " +
                                   "END");


                connection.Close();
            }
        }
        internal static void CreateStackTable()
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                connection.Open();
                connection.Execute("IF NOT EXISTS (" +
                                   "SELECT * FROM INFORMATION_SCHEMA.TABLES " +
                                   "WHERE TABLE_NAME = 'Stack') " +
                                   "BEGIN " +
                                   "CREATE TABLE dbo.Stack (" +
                                   "Name VARCHAR(50) PRIMARY KEY); " +
                                   "END");


                connection.Close();
            }
        }
    }

}
