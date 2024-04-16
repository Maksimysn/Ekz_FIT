using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace Fit;

public partial class Yslyga : Window
{
    private Services _selectedServices;
    private MySqlConnection _connection;
    private List<Services> _services;
    private string _connString = "server=10.10.1.24;database=abd8;port=3306;User Id=user01;password=user01pro";
    
    public Yslyga()
    {
        InitializeComponent();
        
        ShowTable();
        _connection = new MySqlConnection(_connString);
        ServicesGrid.SelectionChanged += ServicesGrid_SelectionChanged;
        SearchTextBox.TextChanged += SearchTextBox_TextChanged;
        
        
        ComboBox.SelectionChanged += ComboBox_SelectionChanged;
        ComboBox.ItemsSource = GetPol(); 
    }

    
    public void ShowTable()
    {
        string sql = "SELECT * FROM Services";

        _services = new List<Services>(); 

        using (MySqlConnection connection = new MySqlConnection(_connString))
        {
            connection.Open();

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var currentServices = new Services
                    {
                        ServicesID = reader.GetInt32("ServicesID"),
                        NameN = reader.GetString("NameN"),
                        NameF = reader.GetString("NameF"),
                        Telephone = reader.GetInt32("Telephone"),
                        Date = reader.GetString("Date"),
                        Pol = reader.GetString("Pol"),
                        Skidka = reader.GetInt32("Skidka"),
                    };

                    _services.Add(currentServices); 
                }
            }
        }

        ServicesGrid.ItemsSource = _services; 
    }
    
    
    
    
    
    
    
    private void ComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ComboBox.SelectedItem is string selectedPol)
        {
            FilterByPol(selectedPol);
        }
    }

    
    
    private List<string> GetPol()
    {
        List<string> pol = new List<string>();

        using (MySqlConnection connection = new MySqlConnection(_connString))
        {
            connection.Open();

            string sql = "SELECT DISTINCT Pol FROM Services";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                pol.Add(reader.GetString("Pol"));
            }

            reader.Close();
        }

        return pol;
    }
    
    private void FilterByPol(string pol)
    {
        string sql = "SELECT * FROM Services WHERE Pol = @Pol";

        List<Services> filteredServices = new List<Services>();

        using (MySqlConnection connection = new MySqlConnection(_connString))
        {
            connection.Open();

            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Pol", pol);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var currentServices = new Services
                    {
                        ServicesID = reader.GetInt32("ServicesID"),
                        NameN = reader.GetString("NameN"),
                        NameF = reader.GetString("NameF"),
                        Telephone = reader.GetInt32("Telephone"),
                        Date = reader.GetString("Date"),
                        Pol = reader.GetString("Pol"),
                        Skidka = reader.GetInt32("Skidka"),
                    };

                    filteredServices.Add(currentServices);
                }
            }
        }

        ServicesGrid.ItemsSource = filteredServices;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    private void ResetFilterButton_Click(object? sender, RoutedEventArgs e)
    {
        // Сбросить фильтр и отобразить полную таблицу
        ShowTable();
 
        // Очистить выбор в ComboBox
        ComboBox.SelectedItem = null;
    }

    private void SortBy_Click(object? sender, RoutedEventArgs e)
    {
        
    }

    private void OpenNextForm_Click(object? sender, RoutedEventArgs e)
    {
        var yslyga_DForm = new D_Yslyga(this);
        yslyga_DForm.Show();
    }

    private void DeleteGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedServices != null)
        {
            DeleteServices(_selectedServices.ServicesID);
        }
    }
    
    private void DeleteServices(int servicesID)
    {
        using (_connection = new MySqlConnection(_connString))
        {
            _connection.Open();
            string queryString = $"DELETE FROM Services WHERE ServicesID = {servicesID}";
            MySqlCommand command = new MySqlCommand(queryString, _connection);
            command.ExecuteNonQuery();
        }

        ShowTable();
    }

    
    
    
    

    private void EditGrid_Click(object? sender, RoutedEventArgs e)
    {
        
    }

    private void ServicesGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ServicesGrid.SelectedItem is Services selectedServices)
        {
            _selectedServices = selectedServices;
        }
    }
    
    
    private void SearchTextBox_TextChanged(object sender, RoutedEventArgs e)
    {
        string searchQuery = SearchTextBox.Text.Trim();
        if (!string.IsNullOrEmpty(searchQuery))
        {
            SearchAndRefreshTable(searchQuery);
        }
        else
        {
            ShowTable();
        }
    }

    private void SearchAndRefreshTable(string searchQuery)
    {
        if (_services == null || _services.Count == 0)
        {
            return;
        }

        searchQuery = searchQuery.ToLower();
        
        var filteredServices = _services
            .Where(v =>
                v.Pol.ToLower().Contains(searchQuery) ||
                v.NameF.ToString().Contains(searchQuery))
            .ToList();
        
        ServicesGrid.ItemsSource = filteredServices;
    }
    
    
    
    
    
}
    