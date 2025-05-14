using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB4OOP.Models
{
    public class Exam
    {
        private string name;
        private double score;
        private DateTime date;

        public Exam(string name, double score, DateTime date)
        {
            this.name = name;
            this.score = score;
            this.date = date;
        }

        public string Name => name;
        public double Score => score;
        public DateTime Date => date;
    }
}
