using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Data.SQLite;
using LAB4OOP.Models;

namespace LAB4OOP.Helpers
{
    public class DatabaseHelper
    {
        private string _databaseFile = "mydatabase.db";
        private string _connectionString;

        public DatabaseHelper()
        {
            _connectionString = $"Data Source={_databaseFile};Version=3;";
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            if (!File.Exists(_databaseFile))
            {
                SQLiteConnection.CreateFile(_databaseFile);
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    CreateTables(connection);
                }
                Console.WriteLine("Створюємо БД");
            }
            Console.WriteLine("База даних вже є");
        }
        public void CreateTables(SQLiteConnection connection)
        {

                string createStudentsTable = @"
                CREATE TABLE IF NOT EXISTS Students (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    BirthDate TEXT NOT NULL,
                    EducationLevel TEXT NOT NULL,
                    ExamId INTEGER NOT NULL,
                    Score REAL NOT NULL,
                    AverageScore REAL NOT NULL,
                    FOREIGN KEY (ExamId) REFERENCES Exams(Id)                 
                );";
                ExecuteQuery(createStudentsTable, connection);
                string createExamsTable = @"
                CREATE TABLE IF NOT EXISTS Exams (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Date TEXT NOT NULL                
                );";           
                ExecuteQuery(createExamsTable, connection);


        }
        private void ExecuteQuery(string query, SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }     
        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

    }

}
