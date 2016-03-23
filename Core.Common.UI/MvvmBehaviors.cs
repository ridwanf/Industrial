using System.Windows;

namespace Core.Common.UI
{
    public static class MvvmBehaviors
    {
        public static string GetLoadeMethodName(DependencyObject obj)
        {
            return (string)obj.GetValue(LoadedMethodProperty);
        }

        public static void SetLoadedMethodName(DependencyObject obj, string value)
        {
            obj.SetValue(LoadedMethodProperty, value);
        }

        public static DependencyProperty LoadedMethodProperty =
            DependencyProperty.RegisterAttached("LoadedMethodName",
                typeof(string), typeof(MvvmBehaviors), new PropertyMetadata(null, OnLoadedMethodName));

        private static void OnLoadedMethodName(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            if (element!=null)
            {
                element.Loaded += (s, e2) =>
                {
                    var viewModel = element.DataContext;
                    if (viewModel == null)
                        return;
                    var methodInfo = viewModel.GetType().GetMethod(e.NewValue.ToString());
                    if (methodInfo != null)
                        methodInfo.Invoke(viewModel, null);
                };
            }
        }
    }
}
