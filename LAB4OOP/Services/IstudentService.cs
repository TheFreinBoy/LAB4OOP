using LAB4OOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB4OOP.Services
{
    public interface IStudentService
    {
        List<Student> GetAllStudents();
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int studentId);
    }
}

