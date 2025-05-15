using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB4OOP.Models
{
    public class Exam
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public double Score { get; set; }
        public DateTime Date { get; set; }

        public Exam() { }

        public Exam(string name, DateTime date)
        {
            Name = name;            
            Date = date;
        }
    }
}
