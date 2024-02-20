using MySqlX.XDevAPI.Common;
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
    /// Interaction logic for PasswordChangeForm.xaml
    /// </summary>
    public partial class PasswordChangeForm : Window
    {
        private readonly User _user;
        public CrudActions SelectedAction { get; private set; }
        public PasswordChangeForm(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CamparePasswords())
            {
                SelectedAction = CrudActions.Update;
                _user.PasswordHash = App.md5(NewPasswordTextBox.Password);
            }
            DialogResult = false;
        }

        private Boolean CamparePasswords()
        {
            return NewPasswordTextBox.Password == RepeatNewPasswordTextBox.Password;
        }
    }
}
