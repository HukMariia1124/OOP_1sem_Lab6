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

        private enum MirrorMode : byte
        {
            Vertical = 1,
            Horizontal = 2,
            Both = 3
        }

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

            BtnStartVertical.IsEnabled = false;
            BtnStartHorizontal.IsEnabled = false;
            BtnStart.IsEnabled = false;
            LstLog.Items.Clear();
            Log("Scan started...");

            try
            {
                if (sender == BtnStartVertical)
                    await Task.Run(() => ProcessImages(folderPath, MirrorMode.Vertical));
                else if (sender == BtnStartHorizontal)
                    await Task.Run(() => ProcessImages(folderPath, MirrorMode.Horizontal));
                else if (sender == BtnStart)
                    await Task.Run(() => ProcessImages(folderPath, MirrorMode.Both));
            }
            catch (Exception ex)
            {
                Log($"Critical error: {ex.Message}", true);
            }
            finally
            {
                BtnStartVertical.IsEnabled = true;
                BtnStartHorizontal.IsEnabled = true;
                BtnStart.IsEnabled = true;
                TxtStatus.Text = "Processing completed.";
                PbStatus.Value = 0;
            }
        }

        private void ProcessImages(string path, MirrorMode mode)
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
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                        string newPath;
                        switch (mode)
                        {
                            case MirrorMode.Vertical:
                                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                newPath = Path.Combine(path, $"{nameWithoutExt}-mirroredY.gif");
                                break;
                            case MirrorMode.Horizontal:
                                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                newPath = Path.Combine(path, $"{nameWithoutExt}-mirroredX.gif");
                                break;
                            case MirrorMode.Both:
                            default:
                                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                newPath = Path.Combine(path, $"{nameWithoutExt}-mirroredXY.gif");
                                break;
                        }

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