
using System;
using System.Collections;
using System.Collections.Generic;
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
    var resultClient = await server.ReceiveAsync();
    new Task(async () =>
    {
        var result = resultClient.RemoteEndPoint;
        int i = 0;
        while (true)
        {
            var imgStream = captureScreenAsync();
            var chunks = imgStream.ToArray().Chunk(ushort.MaxValue - 29).ToList();
            await Console.Out.WriteLineAsync(chunks.Count().ToString());
            foreach (var item in chunks)
                await server.SendAsync(item, result);

        }
    }).Start();

}


MemoryStream? captureScreenAsync()
{
    using (Bitmap? bitmap = new Bitmap(1920, 1080))
    {
        using (Graphics gr = Graphics.FromImage(bitmap))
            gr?.CopyFromScreen(0, 0, 0, 0, new Size(1920, 1080));
        using (MemoryStream memoryStream = new MemoryStream())
        {
            bitmap?.Save(memoryStream, ImageFormat.Jpeg);
            return memoryStream;
        }
    }

}


