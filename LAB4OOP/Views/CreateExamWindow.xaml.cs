using LAB4OOP.Models;
using LAB4OOP.Services;
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
using System.Xml.Linq;

namespace LAB4OOP.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateExamWindow.xaml
    /// </summary>
    public partial class CreateExamWindow : Window
    {
        public Exam CreatedExam { get; private set; }
        private Exam _examBeingEdited;


        private readonly ExamService _examService;

        public CreateExamWindow(ExamService examService, Exam examToEdit = null)
        {
            InitializeComponent();
            _examService = examService;
            _examBeingEdited = examToEdit;

            if (_examBeingEdited != null)
            {
                NameExamTextBox.Text = _examBeingEdited.Name;
                DateExamDatePicker.SelectedDate = _examBeingEdited.Date;
                Title = "Редагування іспиту";
            }
            else
            {
                Title = "Створення іспиту";
            }
        }
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
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameExamTextBox.Text) || DateExamDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заповніть всі поля правильно.");
                return;
            }
            bool isExist = _examService.IsExamExists("Name",NameExamTextBox.Text);
            if (isExist)
            {
                MessageBox.Show("Такий екзамен вже існує");
            }
            else
            {              
                if (_examBeingEdited != null)
                {
                    _examBeingEdited.Name = NameExamTextBox.Text;
                    _examBeingEdited.Date = DateExamDatePicker.SelectedDate.Value;
                    _examService.UpdateExam(_examBeingEdited);
                    CreatedExam = _examBeingEdited;
                }
                else
                {

                    var newExam = new Exam(NameExamTextBox.Text, DateExamDatePicker.SelectedDate.Value);
                    _examService.AddExam(newExam);
                    CreatedExam = newExam;
                }
                this.DialogResult = true;
                this.Close();
            }
            
        }

    }
}
