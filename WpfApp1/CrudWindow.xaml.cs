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
using System.Windows.Shapes;
using WpfApp1.DAL.DAO;
using WpfApp1.DAL.DTO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CrudWindow.xaml
    /// </summary>
    public partial class CrudWindow : Window
    {
        public ObservableCollection<User> Users { get; set; }
        public CrudWindow()
        {
            Users = new();
            this.DataContext = this;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var user in UserDao.GetAll())
            {
                Users.Add(user);
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(sender is ListViewItem item)
            {
                if(item.Content is User user)
                {
                    bool? result = new UserWindow(user).ShowDialog();
                    MessageBox.Show(result.ToString() ?? "null");
                }
            }
        }
    }
}
