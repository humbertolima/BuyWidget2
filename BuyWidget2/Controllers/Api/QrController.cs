using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using BuyWidget2.Models;


namespace BuyWidget2.Controllers.Api
{
    public class QrController : ApiController
    {
        //GET /api/qr/
          public Image GetBitmap()
          {
            //  QR qr = new QR("200", "200", "http://www.google.com/");
              QR qr = new QR();

              byte[] data = File.ReadAllBytes(@"C:\Users\luisr\Desktop\QR\test.jpg");

              // Read in the data but do not close, before using the stream.
              Stream originalBinaryDataStream = new MemoryStream(data);
              Bitmap image = new Bitmap(originalBinaryDataStream);



              return image;


          }
    }
}
