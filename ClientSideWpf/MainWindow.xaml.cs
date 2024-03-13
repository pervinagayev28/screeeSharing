using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


namespace ClientSideWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(MainWindow));



        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }


        private async void Clicked(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtbox.Text))
                btn.IsEnabled = true;
            else
                return;
            btn.Visibility = Visibility.Hidden;
            img.Visibility = Visibility.Visible;
            var buffer = new byte[ushort.MaxValue - 29];
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
            IPEndPoint remoteep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27000);
            var client = new UdpClient(endpoint);
            await client.SendAsync(Encoding.UTF8.GetBytes(txtbox.Text), remoteep);
            int maxlen = buffer.Length;
            int len = 0;
            var list = new List<byte>();
            while (true)
            {
                try
                {
                    do
                    {
                        var result = await client.ReceiveAsync();
                        buffer = result.Buffer;
                        len = buffer.Length;
                        list.AddRange(buffer);
                    } while (len == maxlen);
                    img.Source = Convert(list.ToArray());
                    list.Clear();
                }
                catch (Exception)
                {
                }
            }
        }




        static BitmapImage Convert(byte[] byteArray)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(byteArray);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            return image;
        }
        //static ImageSource Convert(byte[] byteArray)
        //{
        //    using (MemoryStream memoryStream = new MemoryStream(byteArray))
        //    {
        //        Image image = Image.FromStream(memoryStream);
        //        BitmapImage bitmapImage = new BitmapImage();
        //        bitmapImage.BeginInit();
        //        bitmapImage.StreamSource = memoryStream;
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmapImage.EndInit();
        //        bitmapImage.Freeze();
        //        return bitmapImage;
        //    }
        //}


    }
}


