using Microsoft.Win32;
using QRCoder;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QRGenerator {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private byte[]? _qrCodeBytes;
        public MainWindow() {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e) {
            string content = ContentTextBox.Text;
            if (string.IsNullOrWhiteSpace(content)) {
                MessageBox.Show("Please enter some content to generate a QR code.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

            if (TransparentBackgroundRadioButton.IsChecked == true) {
                _qrCodeBytes = qrCode.GetGraphic(20, System.Drawing.Color.Black, System.Drawing.Color.Transparent);
            } else {
                _qrCodeBytes = qrCode.GetGraphic(20, System.Drawing.Color.Black, System.Drawing.Color.White);
            }

            MemoryStream stream = new MemoryStream(_qrCodeBytes);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            QRCodeImage.Source = image;
            SaveButton.IsEnabled = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            if (_qrCodeBytes == null) {
                MessageBox.Show("Please generate a QR code before saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog {
                InitialDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads"),
                FileName = Guid.NewGuid().ToString() + ".png",
                DefaultExt = ".png",
                Filter = "PNG Image|*.png"
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result != true) {
                return;
            }

            File.WriteAllBytes(saveFileDialog.FileName, _qrCodeBytes);
        }
    }
}
