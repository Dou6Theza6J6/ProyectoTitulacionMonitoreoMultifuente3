using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Microsoft.Extensions.Configuration; // <-- Asegúrate de tener este 'using'
using System; // <-- Asegúrate de tener este 'using'

namespace MonitoreoMultifuente3
{
    public partial class MainWindow : Window
    {
        // Constructor vacío, como lo tenías
        public MainWindow()
        {
            InitializeComponent();

            // 1. Asigna el HostPage (tu index.html)
            // Esta línea faltaba en el código que pegaste y es esencial.
            blazorWebView.HostPage = "wwwroot/index.html";

            // 2. Asigna los servicios que YA fueron creados en App.xaml.cs
            // Esta es la única línea que necesitas para los servicios.
            blazorWebView.Services = App.AppHost.Services;

            // ¡NO AÑADAS NADA MÁS AQUÍ!
            // No crees un 'new ServiceCollection()'.
            // No llames a 'AddDbContextFactory' ni 'AddSingleton' aquí.
        }
    }
}