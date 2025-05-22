using System;
using System.Collections.Generic;
using System.Data.SQLite;
using LAB4OOP.Models;
using LAB4OOP.Helpers;

namespace LAB4OOP.Services
{
    public class ExamService
    {
        private readonly DatabaseHelper _dbHelper;
        public ExamService()
        {
            _dbHelper = new DatabaseHelper();
        }


        public void AddExam(Exam exam)
        {

            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Exams (Name, Date) VALUES (@name, @date); SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@name", exam.Name);
                cmd.Parameters.AddWithValue("@date", exam.Date.ToString("yyyy-MM-dd"));
                exam.Id = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public bool IsExamExists(string column, string value)
        {
            using (var connection = _dbHelper.GetConnection())
            {

                connection.Open();
                var query = $"SELECT COUNT(*) FROM Exams WHERE {column} = @Value";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Value", value);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public List<Exam> GetAllExams()
        {
            var exams = new List<Exam>();

            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT Id, Name, Date FROM Exams";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var exam = new Exam
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = DateTime.Parse(reader.GetString(2))
                        };
                        exams.Add(exam);
                    }
                }
            }

            return exams;
        }


        public void UpdateExam(Exam exam)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE Exams SET Name = @name, Date = @date WHERE Id = @id";
                cmd.Parameters.AddWithValue("@name", exam.Name);
                cmd.Parameters.AddWithValue("@date", exam.Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@id", exam.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteExam(int examId)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Exams WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", examId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
