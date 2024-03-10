
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27000);
var server = new UdpClient(endpoint);
EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

var buffer = new byte[ushort.MaxValue - 29];


while (true)
{
    var result = await server.ReceiveAsync();
    new Task(async () =>
    {
        var imgStream = captureScreenAsync();
        var chunks = imgStream.ToArray().Chunk(ushort.MaxValue - 29).ToList();
        await Console.Out.WriteLineAsync(chunks.Count().ToString());
        foreach (var item in chunks)
        {
            await server.SendAsync(item, result.RemoteEndPoint);
            await Console.Out.WriteLineAsync(item.Length.ToString());
        }
    }).Start();

}


Bitmap captureScreenAsync()
{
    Rectangle bounds = Screen.GetBounds(Point.Empty);
    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
    {
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
        }
        bitmap.Save("test.jpg", ImageFormat.Jpeg);
        return bitmap;
    }
}
//MemoryStream? captureScreenAsync()
//{
//    using (Bitmap? bitmap = new Bitmap(1920, 1080))
//    {
//        using (Graphics gr = Graphics.FromImage(bitmap))
//            gr?.CopyFromScreen(0, 0, 0, 0, new Size(1920, 1080));
//        using (MemoryStream memoryStream = new MemoryStream())
//        {
//            bitmap?.Save(memoryStream, ImageFormat.Png);
//            return memoryStream;
//        }
//    }

//}


