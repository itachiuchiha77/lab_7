using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using lab_7.ViewModels;

namespace lab_7.Views
{
    public partial class WindowView : UserControl
    {
        public WindowView()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void ClickButtonFile(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileButton = sender as MenuItem;
                if (fileButton != null)
                {
                    fileButton.ContextMenu.Open();
                    if (fileButton.ContextMenu.IsOpen)
                    {
                        fileButton.ContextMenu.FindControl<MenuItem>("Button_Save").Click += async delegate
                        {
                            var taskGetPathToFile = new SaveFileDialog().ShowAsync((Window)this.Parent);
                            string path = await taskGetPathToFile;
                            var context = this.Parent.DataContext as MainWindowViewModel;
                            if (path != null)
                            {
                                context.WriteStatesToFile(path);
                            }
                            context.OpenWindowView();
                        };
                        fileButton.ContextMenu.FindControl<MenuItem>("Button_Exit").Click += delegate
                        {
                            var window = this.Parent as MainWindow;
                            window.Close();
                        };
                        fileButton.ContextMenu.FindControl<MenuItem>("Button_Load").Click += async delegate
                        {
                            var taskGetPathToFile = new OpenFileDialog().ShowAsync((Window)this.Parent);
                            string[]? path = await taskGetPathToFile;
                            var context = this.Parent.DataContext as MainWindowViewModel;
                            if (path != null)
                            {
                                context.ReadStatesFromFile(string.Join(@"/", path));
                            }
                            context.OpenWindowView();
                        };
                    }
                }
            }
            catch
            {
                return;
            }
        }
        private void ClickButtonAbout(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialogWindow = new AboutWindowView();
                dialogWindow.ShowDialog((Window)this.Parent);
            }
            catch
            {
                return;
            }
        }
    }
}
