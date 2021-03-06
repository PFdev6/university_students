﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using University_students.Models;
using University_students.ViewModel;

namespace University_students.View
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserPage : UserControl
    {
        public UserPage()
        {
            InitializeComponent();
            DataContext = new UserViewModel();
        }

        private void dclickSubjectProgress(object sender, MouseButtonEventArgs e)
        {
            ItemsControl item = e.Source as ItemsControl;
            UserViewModel subjectProgress = item.DataContext as UserViewModel;
            new CustomBoxes.ViewWorkouts(subjectProgress.SelectedSubject, subjectProgress.SelectedSubject.User).Show();
        }
    }
}
