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
        private Student _studentBeingEdited;
        private Student _editingStudent;

        public OrderWindow(ExamService examService, Student studentToEdit = null) 
        {
            InitializeComponent();
            _examService = examService;
            _studentBeingEdited = studentToEdit;
            BirthDatePicker.DisplayDateEnd = DateTime.Today.AddYears(-17);

            LoadExistingExams();
            if (studentToEdit != null)
            {
                _editingStudent = studentToEdit;
                FirstNameBox.Text = studentToEdit.Person.FirstName;
                LastNameBox.Text = studentToEdit.Person.LastName;
                BirthDatePicker.SelectedDate = studentToEdit.Person.BirthDate;
                ServiceCombo.SelectedItem = studentToEdit.EducationLevel;
                ExamComboBox.SelectedValue = studentToEdit.ExamId;
                CostBox.Text = studentToEdit.Score.ToString();
                SeredBox.Text = studentToEdit.AverageScore.ToString();
            }

        }      

        private void LoadExistingExams()
        {          
            _exams = _examService.GetAllExams();
            ExamComboBox.ItemsSource = _exams;
             
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
            if (BirthDatePicker.SelectedDate == null || ServiceCombo.SelectedItem == null || string.IsNullOrEmpty(FirstNameBox.Text) || string.IsNullOrEmpty(LastNameBox.Text) || string.IsNullOrEmpty(ServiceCombo.Text)
                || string.IsNullOrEmpty(ExamComboBox.Text) || string.IsNullOrEmpty(CostBox.Text) || string.IsNullOrEmpty(SeredBox.Text))
            {
                MessageBox.Show("Заповніть усі поля.");
                return;
            }
            if (!IsValidName(FirstNameBox.Text) || !IsValidName(LastNameBox.Text))
            {
                MessageBox.Show("Ім’я та прізвище повинні містити лише літери та бути не менше 3 символів.");
                return;
            }
            if (!int.TryParse(CostBox.Text, out int Score) || !double.TryParse(SeredBox.Text, out double avg) || Score > 100 || avg > 100)
            {
                MessageBox.Show("Введіть правильні числа для оцінки і середнього балу.");             
                return;
            }
            
            var selectedExam = ExamComboBox.SelectedItem as Exam;
            var person = new Person(FirstNameBox.Text, LastNameBox.Text, BirthDatePicker.SelectedDate.Value);
            var level = (EducationLevel)ServiceCombo.SelectedItem;
            var student = new Student(person, level);
            {
                student.Score = Score;
                student.AverageScore = avg;
                student.ExamId = selectedExam.Id;
            }
            if (_editingStudent != null)
            {
                student.Id = _editingStudent.Id;
            }

            CreatedStudent = student;
            
            this.DialogResult = true;
            this.Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new CreateExamWindow(_examService);
            if (createWindow.ShowDialog() == true)
            {
                var newExam = createWindow.CreatedExam;

                _exams.Add(newExam);
                ExamComboBox.ItemsSource = null;
                ExamComboBox.ItemsSource = _exams;

                ExamComboBox.SelectedItem = newExam;
            }
        }
        private void EditExam_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Exam examToEdit)
            {
                var editWindow = new CreateExamWindow(_examService, examToEdit);
                if (editWindow.ShowDialog() == true)
                {
                    LoadExistingExams();
                    ExamComboBox.SelectedItem = _exams.FirstOrDefault(x => x.Id == examToEdit.Id);
                }
            }
        }

        private void DeleteExam_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Exam examToDelete)
            {
                var result = MessageBox.Show($"Ви впевнені, що хочете видалити іспит '{examToDelete.Name}'?",
                                             "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _examService.DeleteExam(examToDelete.Id);
                    LoadExistingExams();
                    ExamComboBox.SelectedIndex = -1;
                }
            }
        }
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !double.TryParse(((TextBox)sender).Text + e.Text, out _);
        }
        private bool IsValidName(string name)
        {
            return name.All(char.IsLetter) && name.Length >= 3;
        }
        
    }
}
