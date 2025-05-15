using LAB4OOP.Helpers;
using LAB4OOP.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace LAB4OOP.Services
{
    public class StudentService : IStudentService
    {
        private static readonly DatabaseHelper _dbHelper = new DatabaseHelper();
        private Dictionary<int, Exam> GetAllExamsDict()
        {
            var exams = new Dictionary<int, Exam>();

            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Exams";

                using (var cmd = new SQLiteCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var exam = new Exam(
                            reader["Name"].ToString(),
                            DateTime.Parse(reader["Date"].ToString())
                        )
                        {
                            Id = Convert.ToInt32(reader["Id"])                           
                        };
                        exams[exam.Id] = exam;
                    }
                }
            }

            return exams;
        }

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();
            var exams = GetAllExamsDict();

            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Students";

                using (var cmd = new SQLiteCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new Person(
                            reader["FirstName"].ToString(),
                            reader["LastName"].ToString(),
                            DateTime.Parse(reader["BirthDate"].ToString())
                        );

                        var student = new Student(
                            person,
                            (EducationLevel)Enum.Parse(typeof(EducationLevel), reader["EducationLevel"].ToString())
                        )
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ExamId = Convert.ToInt32(reader["ExamId"]),
                            Score = Convert.ToDouble(reader["Score"]),
                            AverageScore = Convert.ToDouble(reader["AverageScore"])
                        };

                        if (exams.ContainsKey(student.ExamId))
                        {
                            student.AddExam(exams[student.ExamId]);
                        }

                        students.Add(student);
                    }
                }
            }

            return students;
        }


        public void AddStudent(Student student)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                string query = @"INSERT INTO Students 
                                (FirstName, LastName, BirthDate, EducationLevel, ExamId, Score, AverageScore) 
                                VALUES 
                                (@FirstName, @LastName, @BirthDate, @EducationLevel, @ExamId, @Score, @AverageScore)";

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FirstName", student.Person.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", student.Person.LastName);
                    cmd.Parameters.AddWithValue("@BirthDate", student.Person.BirthDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@EducationLevel", student.EducationLevel.ToString());
                    cmd.Parameters.AddWithValue("@ExamId", student.ExamId);
                    cmd.Parameters.AddWithValue("@Score", student.Score);
                    cmd.Parameters.AddWithValue("@AverageScore", student.AverageScore);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                string query = @"UPDATE Students SET 
                                    FirstName = @FirstName, 
                                    LastName = @LastName, 
                                    BirthDate = @BirthDate, 
                                    EducationLevel = @EducationLevel,
                                    ExamId = @ExamId,
                                    Score = @Score,
                                    AverageScore = @AverageScore
                                WHERE Id = @Id";

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FirstName", student.Person.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", student.Person.LastName);
                    cmd.Parameters.AddWithValue("@BirthDate", student.Person.BirthDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@EducationLevel", student.EducationLevel.ToString());
                    cmd.Parameters.AddWithValue("@ExamId", student.ExamId);
                    cmd.Parameters.AddWithValue("@Score", student.Score);
                    cmd.Parameters.AddWithValue("@AverageScore", student.AverageScore);
                    cmd.Parameters.AddWithValue("@Id", student.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Students WHERE Id = @Id";

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", studentId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
