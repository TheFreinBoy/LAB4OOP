using LAB4OOP.Models;
using LAB4OOP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace LAB4OOP.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IStudentService _studentService = new StudentService();
        private readonly ExamService _examService;
        public MainWindow()
        {
            InitializeComponent();
            LoadStudentsToGrid();           
            _examService = new ExamService();          
        }
        private void LoadStudentsToGrid()
        {
            
            var students = _studentService.GetAllStudents();          
            OrdersGrid.ItemsSource = students;
        }
        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is Student studentToDelete)
            {
                var result = MessageBox.Show($"Ви впевнені, що хочете видалити студента {studentToDelete.FullName}?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _studentService.DeleteStudent(studentToDelete.Id);
                    LoadStudentsToGrid();
                }
            }
        }
        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is Student studentToEdit)
            {
                var orderWindow = new OrderWindow(_examService, studentToEdit);

                if (orderWindow.ShowDialog() == true)
                {
                    var updatedStudent = orderWindow.CreatedStudent;

                    if (updatedStudent != null)
                    {
                        updatedStudent.Id = studentToEdit.Id;
                        _studentService.UpdateStudent(updatedStudent);
                        LoadStudentsToGrid();
                    }
                }
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new OrderWindow(_examService, null);

            if (orderWindow.ShowDialog() == true)
            {
                var student = orderWindow.CreatedStudent;

                if (student != null)
                {
                    _studentService.AddStudent(student);
                    LoadStudentsToGrid();
                }
            }
        }

      
    }
}
