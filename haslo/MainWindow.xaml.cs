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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using BCrypt.Net;

namespace haslo
{
    public partial class MainWindow : Window
    {
        private HashContext _hashContext = new HashContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;

            if (md5RadioButton.IsChecked == true)
            {
                _hashContext.SetHashStrategy(new MD5HashStrategy());
            }
            else if (sha1RadioButton.IsChecked == true)
            {
                _hashContext.SetHashStrategy(new SHA1HashStrategy());
            }
            else if (sha256RadioButton.IsChecked == true)
            {
                _hashContext.SetHashStrategy(new SHA256HashStrategy());
            }
            else if (bcryptRadioButton.IsChecked == true)
            {
                _hashContext.SetHashStrategy(new BCryptHashStrategy());
            }
            else
            {
                MessageBox.Show("Wybierz algorytm haszujący.");
                return;
            }

            string hashedOutput = _hashContext.Hash(input);
            OutputTextBox.Text = hashedOutput;
        }
    }

    public interface IHashStrategy
    {
        string Hash(string input);
    }

    public class MD5HashStrategy : IHashStrategy
    {
        public string Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hash);
            }
        }
    }

    public class SHA1HashStrategy : IHashStrategy
    {
        public string Hash(string input)
        {
            using (var sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hash);
            }
        }
    }

    public class SHA256HashStrategy : IHashStrategy
    {
        public string Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hash);
            }
        }
    }

    public class BCryptHashStrategy : IHashStrategy
    {
        public string Hash(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }
    }

    public class HashContext
    {
        private IHashStrategy _hashStrategy;

        public void SetHashStrategy(IHashStrategy hashStrategy)
        {
            _hashStrategy = hashStrategy;
        }

        public string Hash(string input)
        {
            if (_hashStrategy == null) throw new InvalidOperationException("Hash strategy is not set.");
            return _hashStrategy.Hash(input);
        }
    }
}
