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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Industrial.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ShowMessageDialog(object sender, RoutedEventArgs e)
        {
            // This demo runs on .Net 4.0, but we're using the Microsoft.Bcl.Async package so we have async/await support
            // The package is only used by the demo and not a dependency of the library!
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Hi",
                NegativeButtonText = "Go away!",
                FirstAuxiliaryButtonText = "Cancel",
                ColorScheme = MetroDialogOptions.ColorScheme
            };

            MessageDialogResult result = await this.ShowMessageAsync("Hello!", "Welcome to the world of metro!",
                MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

            if (result != MessageDialogResult.FirstAuxiliary)
                await this.ShowMessageAsync("Result", "You said: " + (result == MessageDialogResult.Affirmative ? mySettings.AffirmativeButtonText : mySettings.NegativeButtonText +
                    Environment.NewLine + Environment.NewLine + "This dialog will follow the Use Accent setting."));
        }

    }
}
