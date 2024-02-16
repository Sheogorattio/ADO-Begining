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
using WpfApp1.DAL.DTO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private readonly User _user;
        public UserWindow(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = _user.Id.ToString();
            NameTextBox.Text = _user.Name;
            LoginTextBox.Text = _user.Login;
            DkTextBox.Text = _user.PasswordHash;
            BirtDateTextBox.SelectedDate = _user.BirthDate;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = null;
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult= true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChangePswd_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new PasswordChangeForm().ShowDialog();
            this.Show();
        }
    }
}
