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
using static MaterialDesignThemes.Wpf.Theme;

namespace LAB4OOP.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IStudentService _studentService = new StudentService();
        public MainWindow()
        {
            InitializeComponent();
            LoadStudentsToGrid();

        }
        private void LoadStudentsToGrid()
        {
            var students = _studentService.GetAllStudents();
            OrdersGrid.ItemsSource = students;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new OrderWindow();

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
