using LAB4OOP.Models;
using LAB4OOP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        private List<Exam> _exams = new List<Exam>();

        private readonly ExamService _examService;

        private Exam _addExamPlaceholder = new Exam { Name = "➕ Додати іспит" };

        public OrderWindow(ExamService examService)
        {
            InitializeComponent();
            _examService = examService;
            

            _exams = new List<Exam> { _addExamPlaceholder };
            ExamComboBox.ItemsSource = _exams;
            LoadExistingExams();
            
        }
        

        private void LoadExistingExams()
        {          
            _exams = _examService.GetAllExams();
            _exams.Add(_addExamPlaceholder);
            ExamComboBox.ItemsSource = _exams;
            ExamComboBox.DisplayMemberPath = "Name"; 
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
        private void ButtonExit_Click(object sender, RoutedEventArgs e) => DialogResult = false;

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(CostBox.Text, out double Score) || !double.TryParse(SeredBox.Text, out double avg))
            {
                MessageBox.Show("Введіть правильні числа для оцінки і середнього балу.");
                return;
            }

            if (BirthDatePicker.SelectedDate == null || ServiceCombo.SelectedItem == null)
            {
                MessageBox.Show("Заповніть усі поля.");
                return;
            }
            var selectedExam = ExamComboBox.SelectedItem as Exam;
            var person = new Person(FirstNameBox.Text, LastNameBox.Text, BirthDatePicker.SelectedDate.Value);
            var level = (EducationLevel)ServiceCombo.SelectedItem;
            var student = new Student(person, level);
            student.Score = Score;
            student.AverageScore = avg;
            student.ExamId = selectedExam.Id;

            CreatedStudent = student;
            
            this.DialogResult = true;
            this.Close();
        }
        private void ExamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ExamComboBox.SelectedItem as Exam;
            if (selected == null) return;

            if (selected == _addExamPlaceholder)
            {
                var createWindow = new CreateExamWindow(_examService);               
                if (createWindow.ShowDialog() == true)
                {
                    
                    var newExam = createWindow.CreatedExam;
                    _exams.Insert(_exams.Count - 1, newExam); 
                    ExamComboBox.ItemsSource = null;
                    ExamComboBox.ItemsSource = _exams;                  
                }

                ExamComboBox.SelectedIndex = -1;
            }
            
        }

    }
}
