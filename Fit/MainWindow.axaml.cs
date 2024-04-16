using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace Fit;

public partial class MainWindow : Window
{
    private MySqlConnection _sqlConnection;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
        // Проверяем, что поля логина и пароля не пусты
        if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Text))
        {
            Console.WriteLine("Ошибка: логин или пароль не могут быть пустыми");
            return;
        }
       
        string connectionString = "server=10.10.1.24;database=abd8;port=3306;User Id=user01;password=user01pro";
       
        using (_sqlConnection = new MySqlConnection(connectionString))
        {
            _sqlConnection.Open();
       
            string selectQuery = "SELECT username FROM admins WHERE username = @Username AND password = @Password";
            using (MySqlCommand sqlCommand = new MySqlCommand(selectQuery, _sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@Username", LoginTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@Password", PasswordTextBox.Text);
       
                var result = sqlCommand.ExecuteScalar();
                if (result != null)
                {
                    Main window = new Main();
                    Hide();
                    window.Show();
                }
                else
                {
                    // Если учетные данные неверные или не найдены, выведите сообщение об ошибке
                    Console.WriteLine("Ошибка: неверный логин или пароль");
                }
            }
        }
    }

    private void RegistrationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Reg regWindow = new Reg();
        regWindow.ShowDialog(this); // Передаем this для указания текущего окна как владельца
    }
}