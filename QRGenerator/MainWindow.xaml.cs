using QRCoder;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QRGenerator {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
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

            byte[] qrCodeBytes = qrCode.GetGraphic(20);

            MemoryStream stream = new MemoryStream(qrCodeBytes);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            QRCodeImage.Source = image;
        }
    }
}
