using LAB4OOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LAB4OOP.Views
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public Student CreatedStudent { get; private set; }
        public OrderWindow()
        {
            InitializeComponent();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
        private void Border_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    DragMove();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(CostBox.Text, out double examScore) || !double.TryParse(SeredBox.Text, out double avg))
            {
                MessageBox.Show("Введіть правильні числа для оцінки і середнього балу.");
                return;
            }

            if (BirthDatePicker.SelectedDate == null || DateBox.SelectedDate == null || ServiceCombo.SelectedItem == null)
            {
                MessageBox.Show("Заповніть усі поля.");
                return;
            }

            var person = new Person(FirstNameBox.Text, LastNameBox.Text, BirthDatePicker.SelectedDate.Value);
            var level = (EducationLevel)ServiceCombo.SelectedItem;
            var student = new Student(person, level);

            var exam = new Exam(AddressBox.Text, examScore, DateBox.SelectedDate.Value);
            student.AddExam(exam);

            CreatedStudent = student;
            this.DialogResult = true;
            this.Close();
        }
    }
}
