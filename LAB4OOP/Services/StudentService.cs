using LAB4OOP.Helpers;
using LAB4OOP.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB4OOP.Services
{
    public class StudentService : IStudentService
    {

        private static readonly DatabaseHelper _dbHelper = new DatabaseHelper();

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();

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
                            Id = Convert.ToInt32(reader["Id"])
                        };

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
                string query = "INSERT INTO Students (FirstName, LastName, BirthDate, EducationLevel) VALUES (@FirstName, @LastName, @BirthDate, @EducationLevel)";

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FirstName", student.Person.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", student.Person.LastName);
                    cmd.Parameters.AddWithValue("@BirthDate", student.Person.BirthDate);
                    cmd.Parameters.AddWithValue("@EducationLevel", student.EducationLevel.ToString());

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                string query = "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, BirthDate = @BirthDate, EducationLevel = @EducationLevel WHERE Id = @Id";

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FirstName", student.Person.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", student.Person.LastName);
                    cmd.Parameters.AddWithValue("@BirthDate", student.Person.BirthDate);
                    cmd.Parameters.AddWithValue("@EducationLevel", student.EducationLevel.ToString());
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
