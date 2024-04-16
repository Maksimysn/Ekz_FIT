using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace Fit;

public partial class Reg : Window
{
    private MySqlConnection _sqlConnection;
    public Reg()
    {
        InitializeComponent();
    }

    private void RegistrationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        // Проверяем, что поля логина и пароля не пусты
        if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordBox.Text) || string.IsNullOrEmpty(ConfirmPasswordBox.Text))
        {
            Console.WriteLine("Ошибка: логин и пароли не могут быть пустыми");
            return;
        }

        // Проверяем, что пароли совпадают
        if (PasswordBox.Text != ConfirmPasswordBox.Text)
        {
            Console.WriteLine("Ошибка: пароли не совпадают");
            return;
        }

        string connectionString = "server=10.10.1.24;database=abd8;port=3306;User Id=user01;password=user01pro";

        using (_sqlConnection = new MySqlConnection(connectionString))
        {
            _sqlConnection.Open();

            string insertQuery = "INSERT INTO users (username, password) VALUES (@Username, @Password)";
            using (MySqlCommand sqlCommand = new MySqlCommand(insertQuery, _sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@Username", LoginTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@Password", PasswordBox.Text);

                int rowsAffected = sqlCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Пользователь успешно зарегистрирован");
                    Close(); // Закрываем окно регистрации после успешной регистрации
                }
                else
                {
                    Console.WriteLine("Ошибка при регистрации пользователя");
                }
            }
        }
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(); 
    }

}