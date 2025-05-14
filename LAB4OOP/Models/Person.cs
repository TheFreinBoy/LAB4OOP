using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB4OOP.Models
{
    public class Person
    {
        private string firstName;
        private string lastName;
        private DateTime birthDate;

        public Person(string firstName, string lastName, DateTime birthDate)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
        }

        public string FirstName => firstName;
        public string LastName => lastName;
        public DateTime BirthDate => birthDate;

        public string FullName => $"{lastName}, {firstName}";
    }
}
