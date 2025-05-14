using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB4OOP.Models
{
    public class Student
    {
        private Person person;
        private EducationLevel educationLevel;
        private List<Exam> exams;

        public Student(Person person, EducationLevel educationLevel)
        {
            this.person = person;
            this.educationLevel = educationLevel;
            exams = new List<Exam>();
        }

        public Person Person => person;
        public EducationLevel EducationLevel => educationLevel;
        public List<Exam> Exams => exams;

        public void AddExam(Exam exam)
        {
            exams.Add(exam);
        }

        public double AverageScore => exams.Any() ? exams.Average(e => e.Score) : 0.0;

        public override string ToString()
        {
            return $"{person.FullName}, {educationLevel}, {exams.Count} екзаменів, Середній бал: {AverageScore:F2}";
        }

        public string ToStringShort()
        {
            return $"{person.LastName} — {AverageScore:F2}";
        }

        public string FullName => person.FullName;
        public string ExamName => exams.LastOrDefault()?.Name ?? "";
        public double? ExamScore => exams.LastOrDefault()?.Score;
        public DateTime? ExamDate => exams.LastOrDefault()?.Date;
        public int Id {  get; set; }
    }

}
