using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net;
using System;
using DotImaging;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using SixLabors.ImageSharp.PixelFormats;


namespace ClientSideWebTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UdpController : ControllerBase
    {
        byte[] buffer = new byte[ushort.MaxValue - 29];
        IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
        IPEndPoint remoteep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27000);
        UdpClient client;
        int maxlen;
        int len = 0;
        List<byte> list = new List<byte>();
        private static bool once= true;
        public async Task<IActionResult> SendUdpPacket()
        {
            
         
        }
            [HttpPost("send")]
        public async Task<IActionResult> GetImage()
        {
            do
            {
                var result = await client.ReceiveAsync();
                buffer = result.Buffer;
                len = buffer.Length;
                list.AddRange(buffer);
            } while (len == maxlen);
            return decimal; 

        }
        public Bitmap ConvertToBitmap(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                using (Image<Rgba32> image = Image.Load(stream))
                {
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        image.SaveAsBmp(outputStream);
                        return new Bitmap(outputStream);
                    }
                }
            }
        }

    }
}
