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
                    EducationLevel TEXT NOT NULL
                );";
                ExecuteQuery(createStudentsTable, connection);
                string createExamsTable = @"
                CREATE TABLE IF NOT EXISTS Exams (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentId INTEGER NOT NULL,
                    Name TEXT NOT NULL,
                    Score REAL NOT NULL,
                    Date TEXT NOT NULL,
                    FOREIGN KEY (StudentId) REFERENCES Students(Id) ON DELETE CASCADE
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

        public void AddStudent(Student student)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO Students (FirstName, LastName, BirthDate, EducationLevel) VALUES (@f, @l, @b, @e); SELECT last_insert_rowid();";
                    cmd.Parameters.AddWithValue("@f", student.Person.FirstName);
                    cmd.Parameters.AddWithValue("@l", student.Person.LastName);
                    cmd.Parameters.AddWithValue("@b", student.Person.BirthDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@e", student.EducationLevel.ToString());
                    var studentId = (long)cmd.ExecuteScalar();

                    foreach (var exam in student.Exams)
                    {
                        var examCmd = connection.CreateCommand();
                        examCmd.CommandText = "INSERT INTO Exams (StudentId, Name, Score, Date) VALUES (@sid, @n, @s, @d)";
                        examCmd.Parameters.AddWithValue("@sid", studentId);
                        examCmd.Parameters.AddWithValue("@n", exam.Name);
                        examCmd.Parameters.AddWithValue("@s", exam.Score);
                        examCmd.Parameters.AddWithValue("@d", exam.Date.ToString("yyyy-MM-dd"));
                        examCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }
        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

    }

}
