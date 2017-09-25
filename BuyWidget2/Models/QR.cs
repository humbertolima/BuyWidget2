using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using MessagingToolkit.QRCode.Codec;
using RestSharp.Extensions;
using ZXing;

namespace BuyWidget2.Models
{
    public class QR
    {
        private int width { get; set; }
        private int height { get; set; }
       // private int Text { get; set; }

     /*   public QR(string width, string height, string text)
        {
            Width = width;
            Height = height;
            Text = text;
        }*/

        public QR()
        {
            width = 200;
            height = 200;
        }
/*
        public Bitmap GenerateQR()
        {
            var bw = new ZXing.BarcodeWriter();
            var encOptions = new ZXing.Common.EncodingOptions()
            {
                Width = width,
                Height = height,
                Margin = 0
            };
            bw.Options = encOptions;
            bw.Format = ZXing.BarcodeFormat.QR_CODE;
            var result = new Bitmap(bw.Write("hello world"));

            return result;
        }*/

        public Image GenerateQR()
        {
            Zen.Barcode.CodeQrBarcodeDraw qr = Zen.Barcode.BarcodeDrawFactory.CodeQr;

            var result = qr.Draw("hello", 200);

            //result.Save(@"C:\Users\luisr\Desktop\QR\test.jpg");

            //result.Save(@"test.jpg");

            

            return result;
        }
    }
}