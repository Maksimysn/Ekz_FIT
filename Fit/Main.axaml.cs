using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Fit;

public partial class Main : Window
{
    public Main()
    {
        InitializeComponent();
    }
    

    private void OpenYslygaForm(object? sender, RoutedEventArgs e)
    {
        Yslyga YslygaForm = new Yslyga();
        YslygaForm.Show();
    }
    
}