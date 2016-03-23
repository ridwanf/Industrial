using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Industrial.Wpf.Infrastructures
{
    public static class DialogService
    {
     //   private readonly MetroWindow metroWindow;

        //public DialogService(MetroWindow metroWindow)
        //{
        //    this.metroWindow = metroWindow;
        //}

        public static Task<MessageDialogResult> AskQuestionAsync(string title, string message)
        {
            var metroWindow = (MetroWindow)Application.Current.MainWindow;
            var settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };
            return metroWindow.ShowMessageAsync(title, message,
                MessageDialogStyle.AffirmativeAndNegative, settings);
        }

        public static Task<ProgressDialogController> ShowProgressAsync(string title, string message)
        {
            var metroWindow = (MetroWindow)Application.Current.MainWindow;

            return metroWindow.ShowProgressAsync(title, message);
        }

        public static Task ShowMessageAsync(string title, string message)
        {
            var metroWindow = (MetroWindow)Application.Current.MainWindow;

            return metroWindow.ShowMessageAsync(title, message);
        }
    }

    public interface IDialogService
    {
        Task<MessageDialogResult> AskQuestionAsync(string title, string message);
        Task<ProgressDialogController> ShowProgressAsync(string title, string message);
        Task ShowMessageAsync(string title, string message);
    }
}
