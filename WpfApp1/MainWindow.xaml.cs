using System;
using System.Drawing; // Потрібен System.Drawing.Common
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfImageMirror
{
    public partial class MainWindow : Window
    {
        private readonly Regex regexExtForImage = new Regex(@"^((bmp)|(gif)|(tiff?)|(jpe?g)|(png))$", RegexOptions.IgnoreCase);

        public MainWindow()
        {
            InitializeComponent();
            TxtFolderPath.Text = Directory.GetCurrentDirectory();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            TxtFolderPath.Text = Directory.GetCurrentDirectory();
            Log("Path reset to application startup folder.");
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = TxtFolderPath.Text;

            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("The specified folder does not exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            BtnStart.IsEnabled = false;
            LstLog.Items.Clear();
            Log("Scan started...");

            try
            {
                await Task.Run(() => ProcessImages(folderPath));
            }
            catch (Exception ex)
            {
                Log($"Critical error: {ex.Message}", true);
            }
            finally
            {
                BtnStart.IsEnabled = true;
                TxtStatus.Text = "Processing completed.";
                PbStatus.Value = 0;
            }
        }

        private void ProcessImages(string path)
        {
            //Image mirroring logic
        }

        private void Log(string message, bool isError = false)
        {
            var item = new System.Windows.Controls.ListBoxItem
            {
                Content = $"[{DateTime.Now:HH:mm:ss}] {message}",
                Foreground = isError ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Black
            };
            LstLog.Items.Add(item);
            LstLog.ScrollIntoView(item);
        }
    }
}