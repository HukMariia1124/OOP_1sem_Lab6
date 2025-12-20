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
            string[] files;
            try
            {
                files = Directory.GetFiles(path);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => Log($"No access to files: {ex.Message}", true));
                return;
            }

            int totalFiles = files.Length;
            int processedCount = 0;

            Dispatcher.Invoke(() => PbStatus.Maximum = totalFiles);

            foreach (string fileName in files)
            {
                processedCount++;
                Dispatcher.Invoke(() =>
                {
                    PbStatus.Value = processedCount;
                    TxtStatus.Text = $"Processing: {Path.GetFileName(fileName)} ({processedCount}/{totalFiles})";
                });

                if (fileName.Contains("-mirrored")) continue;

                try
                {
                    using (Bitmap bitmap = new Bitmap(fileName))
                    {
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

                        string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);

                        string newPath = Path.Combine(path, nameWithoutExt + "-mirrored.gif");

                        bitmap.Save(newPath, ImageFormat.Gif);

                        Dispatcher.Invoke(() => Log($"[OK] {Path.GetFileName(fileName)} -> saved as GIF."));
                    }
                }
                catch (Exception)
                {
                    if (regexExtForImage.IsMatch(Path.GetExtension(fileName).TrimStart('.')))
                    {
                        Dispatcher.Invoke(() => Log($"[ERR] {Path.GetFileName(fileName)}: file is corrupted or format is not supported.", true));
                    }
                    else
                    {
                        Dispatcher.Invoke(() => Log($"[SKIP] {Path.GetFileName(fileName)}: not an image."));
                    }
                }
                //System.Threading.Thread.Sleep(500);
            }
        }

        private void Log(string message, bool isError = false)
        {
            var item = new System.Windows.Controls.ListBoxItem
            {
                Content = $"[{DateTime.Now:HH:mm:ss}] {message}"
            };

            if (isError)
                item.Foreground = System.Windows.Media.Brushes.Red;
            else if (message.Contains("[SKIP]"))
                item.Foreground = System.Windows.Media.Brushes.LightGray;
            else
                item.Foreground = System.Windows.Media.Brushes.Black;

            LstLog.Items.Add(item);
            LstLog.ScrollIntoView(item);
        }
    }
}