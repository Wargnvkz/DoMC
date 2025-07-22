#pragma warning disable IDE0090
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DoMCLib.Classes;
using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Configuration;

namespace DoMCLib.Tools
{
    public class ImageTools
    {
        public static readonly int Width = 512;
        public static readonly int Height = 512;
        public static readonly int BytePerPixel = 2;
        private static int Size = Width * Height * BytePerPixel;

        //[y,x]
        public static byte[] ImageToArray(short[,] image)
        {
            var len = Buffer.ByteLength(image);
            byte[] baData = new byte[len];
            Buffer.BlockCopy(image, 0, baData, 0, len);
            return baData;
        }
        public static short[] ImageToArray16(short[,] image)
        {
            var len = Buffer.ByteLength(image);
            short[] baData = new short[len / 2];
            Buffer.BlockCopy(image, 0, baData, 0, len);
            return baData;
        }
        public static byte[] ImageToArray(ushort[,] image)
        {
            var len = Buffer.ByteLength(image);
            byte[] baData = new byte[len];
            Buffer.BlockCopy(image, 0, baData, 0, len);
            return baData;
        }
        public static short[,]? ArrayToImage(byte[] data, bool NonStandardImage = false)
        {
            if (data is null || data.Length == 0)
            {
                return null;
            }

            var elementLength = 2;
            short[,] img;
            if (NonStandardImage)
            {
                var dlen = Math.Sqrt(data.Length / elementLength);
                var len = (int)Math.Ceiling(dlen);//Buffer.ByteLength(image);
                                                  //byte[] baData = new byte[len];
                img = new short[len, len];
            }
            else
            {
                img = new short[Height, Width];
            }
            int bytetocopy = img.Length * elementLength;
            Buffer.BlockCopy(data, 0, img, 0, Math.Min(data.Length, bytetocopy));
            return img;
        }
        public static ushort[,] ArrayToUImage(byte[] data, bool NonStandardImage = false)
        {
            var elementLength = 2;
            ushort[,] img;
            if (NonStandardImage)
            {
                var dlen = Math.Sqrt(data.Length / elementLength);
                var len = (int)Math.Ceiling(dlen);//Buffer.ByteLength(image);
                                                  //byte[] baData = new byte[len];
                img = new ushort[len, len];
            }
            else
            {
                img = new ushort[Height, Width];
            }
            int bytetocopy = img.Length * elementLength;
            Buffer.BlockCopy(data, 0, img, 0, bytetocopy);
            return img;
        }

        public static short[,]? ImageCopy(short[,] image)
        {
            if (image == null) return null;
            var len = Buffer.ByteLength(image);
            short[,] newImg = new short[image.GetLength(0), image.GetLength(1)];
            Buffer.BlockCopy(image, 0, newImg, 0, len);
            return newImg;
        }
        public static byte[] Compress(byte[] data)
        {
            if (data == null) return new byte[0];
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionMode.Compress))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }

        public static Bitmap? DrawImage(short[,] image, bool invert = false,
            Point? BadPoint = null, Rectangle? CheckArea = null)
        {
            if (image == null)
                return null;

            var bmp = new Bitmap(512, 512);

            if (!invert)
            {
                var max = image.Cast<short>().Max(l => l);
                var min = image.Cast<short>().Min(l => l);
                if (max == min) if (min > 0) min = (short)(min - 1); else { max = (short)(max + 1); }

                if (min > 0) min = 0;
                var maxmax = Math.Max(max, -min);

                //min = max;
                for (int x = 0; x < 512; x++)
                {
                    for (int y = 0; y < 512; y++)
                    {
                        var v = (short)image[y, x];
                        /*byte r = 0;
                        if (v >= 0) r = (byte)(v * 255 / max);
                        byte b = 0;
                        if (v < 0) b = (byte)((-v) * 255 / min);
                        var sb = new SolidBrush(Color.FromArgb(r, 0, b));*/
                        var pv = v * 255 / maxmax;
                        var nv = (-v) * 255 / maxmax;
                        byte r = 0;
                        byte g = 0;
                        byte b = 0;
                        if (v >= 0)
                        {
                            r = (byte)pv;
                            g = 0;// r;
                            b = 0;// r;
                        }
                        else
                        {
                            r = 0;
                            g = (byte)((-v) * 255 / maxmax);
                            b = 0;
                        }
                        bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                        //g.FillRectangle(sb, x, y, 1, 1);
                    }

                }
            }
            else
            {

                var max = image.Cast<short>().Max(l => l);
                var min = image.Cast<short>().Min(l => l);
                if (max == min) if (min > 0) min = (short)(min - 1); else { max = (short)(max + 1); }

                if (min > 0) min = 0;
                max = (short)Math.Max(max, -min);
                min = max;
                for (int x = 0; x < 512; x++)
                {
                    for (int y = 0; y < 512; y++)
                    {
                        var v = (short)image[y, x];
                        /*byte r = 0;
                        if (v >= 0) r = (byte)(v * 255 / max);
                        byte b = 0;
                        if (v < 0) b = (byte)((-v) * 255 / min);
                        var sb = new SolidBrush(Color.FromArgb(r, 0, b));*/
                        var pv = v * 255 / max;
                        var nv = (-v) * 255 / min;
                        byte r = 0;
                        byte g = 0;
                        byte b = 0;
                        if (v >= 0)
                        {
                            r = (byte)(255 - (byte)pv);
                            g = 0;
                            b = 0;
                        }
                        else
                        {
                            r = 0;
                            g = (byte)(255 - (byte)((-v) * 255 / min));
                            b = 0;
                        }
                        bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                        //g.FillRectangle(sb, x, y, 1, 1);
                    }

                }
            }
            {
                var g = Graphics.FromImage(bmp);
                if (BadPoint.HasValue)
                {
                    var pen = new Pen(Color.Magenta);
                    g.DrawEllipse(pen, BadPoint.Value.X - 10, BadPoint.Value.Y - 10, 20, 20);
                }
                if (CheckArea.HasValue)
                {
                    var pen = new Pen(Color.LightGray);
                    g.DrawRectangle(pen, CheckArea.Value);

                }
            }
            return bmp;

        }

        public static short[,] CalculateAverage(short[][,] images, short noImageIfAverageLessThan)
        {
            var standard = new short[512, 512];
            if (images == null || images.Length == 0)
                return standard;
            bool[] hasImage = new bool[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                hasImage[i] = Average(images[i]) > noImageIfAverageLessThan;
            }
            var imagesWithData = Enumerable.Range(0, images.Length).Where(i => hasImage[i]).ToList();
            var standardsN = imagesWithData.Count;
            for (int x = 0; x < 512; x++)
            {
                for (int y = 0; y < 512; y++)
                {
                    var vals = new short[standardsN];
                    for (int s = 0; s < standardsN; s++)
                    {
                        vals[s] = (short)(images[imagesWithData[s]]?[y, x] ?? 0);
                    }
                    double avg = (vals.Sum(v => (double)v) / standardsN);
                    if (avg > short.MaxValue) avg = short.MaxValue;
                    if (avg < short.MinValue) avg = short.MinValue;

                    standard[y, x] = (short)avg;

                }

            }

            return standard;

        }

        public static double[] CalculateDeviation(short[][,] images, int StartLine, int EndLine)
        {
            var standardsN = images.Length;
            if (images == null || images.Length == 0)
                return null;
            var devsocket = new double[standardsN];
            var dev = new double[512];
            for (int x = 0; x < 512; x++)
            {
                double avg = 0;
                double avg2 = 0;
                for (int y = 0; y < 512; y++)
                {
                    for (int s = 0; s < standardsN; s++)
                    {
                        var p = (short)images[s][y, x];
                        avg += p;
                        avg2 += p * p;
                    }

                }
                dev[x] = avg2 / 512 - avg / 512 * avg / 512;
            }
            return dev;
        }

        /*public static double CalculateTotalDeviation(short[][,] images, int StartFrame, int EndFrame)
        {
            var standardsN = images.Length;
            if (images == null || images.Length == 0)
                return 0;
            var devsocket = new double[standardsN];
            //var dev = new double[512];
            double avg = 0;
            double avg2 = 0;
            for (int x = 0; x < 512; x++)
            {
                for (int y = StartFrame; y < EndFrame + 1; y++)
                {
                    for (int s = 0; s < standardsN; s++)
                    {
                        var p = (short)images[s][y, x];
                        avg += p;
                        avg2 += p * p;
                    }

                }
            }
            var dev = Math.Sqrt(avg2 / 512 - avg / 512 * avg / 512);
            return dev;
        }*/
        public static Deviation CalculateDeviationFull(short[][,] images, Rectangle? rect = null)
        {
            Rectangle r;
            if (rect == null)
                r = new Rectangle(0, 0, 511, 511);
            else
            {
                var X = rect.Value.X;
                var Y = rect.Value.Y;
                var right = rect.Value.Right;
                var bottom = rect.Value.Bottom;
                X = X < 0 ? 0 : X;
                Y = Y < 0 ? 0 : Y;
                right = right >= 512 ? 511 : right;
                bottom = bottom >= 512 ? 511 : bottom;
                r = new Rectangle(X, Y, right - X, bottom - Y);
            }


            if (images?.Any(i => i == null) ?? true)
            {
                return new Deviation() { TotalDeviation = 0, TotalAverage = 0, SocketAverage = new double[images.Length] };
            }
            var standardsN = images.Length;

            var dev = new Deviation();
            dev.SocketDeviation = new double[standardsN];
            dev.SocketAverage = new double[standardsN];
            dev.SocketAverageSquared = new double[standardsN];
            dev.SocketAverageByLines = new double[standardsN][];
            dev.SocketAverageByLinesSquared = new double[standardsN][];
            dev.Max = new short[standardsN];
            dev.Min = new short[standardsN];


            for (int i = 0; i < standardsN; i++) dev.SocketAverageByLines[i] = new double[512];
            for (int i = 0; i < standardsN; i++) dev.SocketAverageByLinesSquared[i] = new double[512];
            dev.SocketDeviationByLines = new double[standardsN][];
            for (int i = 0; i < standardsN; i++) dev.SocketDeviationByLines[i] = new double[512];

            if (images == null || images.Length == 0)
                return dev;
            var devsocket = new double[standardsN];
            //var dev = new double[512];
            for (int s = 0; s < standardsN; s++)
            {
                dev.Max[s] = short.MinValue;
                dev.Min[s] = short.MaxValue;

                for (int x = r.X; x <= r.Right; x++)
                {
                    for (int y = r.Y; y < r.Bottom; y++)
                    {
                        var p = images[s][y, x];
                        dev.SocketAverageByLines[s][x] += p;
                        dev.SocketAverageByLinesSquared[s][x] += p * p;
                        if (dev.Max[s] < p) dev.Max[s] = p;
                        if (dev.Min[s] > p) dev.Min[s] = p;
                    }
                    dev.SocketAverageByLines[s][x] = dev.SocketAverageByLines[s][x] / 512;
                    dev.SocketAverageByLinesSquared[s][x] = dev.SocketAverageByLinesSquared[s][x] / 512;
                    dev.SocketDeviationByLines[s][x] = Math.Sqrt(dev.SocketAverageByLinesSquared[s][x] - dev.SocketAverageByLines[s][x] * dev.SocketAverageByLines[s][x]);

                    dev.SocketAverage[s] += dev.SocketAverageByLines[s][x];
                    dev.SocketAverageSquared[s] += dev.SocketAverageByLinesSquared[s][x];
                }
                dev.SocketAverage[s] = dev.SocketAverage[s] / 512;
                dev.SocketAverageSquared[s] = dev.SocketAverageSquared[s] / 512;
                dev.SocketDeviation[s] = Math.Sqrt(dev.SocketAverageSquared[s] - dev.SocketAverage[s] * dev.SocketAverage[s]);
                dev.TotalAverage += dev.SocketAverage[s];
                dev.TotalAverageSquared += dev.SocketAverageSquared[s];
            }
            dev.TotalAverage = dev.TotalAverage / standardsN;
            dev.TotalAverageSquared = dev.TotalAverageSquared / standardsN;
            dev.TotalDeviation = Math.Sqrt(dev.TotalAverageSquared - dev.TotalAverage * dev.TotalAverage);
            return dev;
        }



        public static short[,] GetDifference(short[,] standard, short[,] currentimage, Rectangle? rect = null)
        {
            var dif = new short[512, 512];
            if (standard == null || currentimage == null) return dif;

            {
                for (int x = rect?.Left ?? 0; x <= (rect?.Right ?? 511); x++)
                {
                    if (x < 0 || x > 511) continue;
                    for (int y = rect?.Top ?? 0; y <= (rect?.Bottom ?? 511); y++)
                    {
                        if (y < 0 || y > 511) continue;
                        dif[y, x] = (short)(standard[y, x] - currentimage[y, x]);
                    }
                }
                return dif;
            }
        }

        public static bool IsErrorSocketImage(short[,] difference, short[,] image, short[,] standard, int firstline, int lastline, double maxError)
        {
            for (int x = 0; x < 512; x++)
            {
                for (int y = 0; y < 512; y++)
                {
                    if (Math.Abs(difference[y, x]) > maxError) return true;
                }
            }
            return false;
        }
        public static bool IsBadSocketImage(short[,] difference, Rectangle rect, double maxError)
        {
            var processedImg = ImageTools.DeviationByLine(difference, 10);
            for (int x = rect.X; x < rect.X + rect.Width; x++)
            {
                for (int y = rect.Y; y < rect.Y + rect.Height; y++)
                {
                    if (Math.Abs(processedImg[y, x]) > maxError) return true;
                }
            }
            return false;
        }

        public static short[,] GetNewStandard(short[,] standard, short[,] image, double k)
        {
            var dif = new short[512, 512];
            if (standard == null) return dif;
            if (image == null) return standard;
            for (int x = 0; x < 512; x++)
            {
                for (int y = 0; y < 512; y++)
                {
                    dif[y, x] = (short)(image[y, x] * (1 - k) + standard[y, x] * k);
                }
            }
            return dif;
        }

        public static string ToBase64(byte[] data)
        {
            return System.Convert.ToBase64String(data);

        }
        public static byte[] FromBase64(string base64)
        {
            return System.Convert.FromBase64String(base64);
        }

        public static Complex[] Furje(short[] line)
        {
            var dst = new Complex[256];
            for (int n = 1; n < 256; n++)
            {
                for (int x = 0; x < 512; x++)
                {
                    dst[n] += line[x] * new Complex(Math.Cos(n * x / 512.0 * 2 * Math.PI), Math.Sin(n * x / 512.0 * 2 * Math.PI));
                }
            }
            return dst;
        }

        public static short[,] BaseFilter(short[,] src, double n, double k)
        {
            //if (k < 0 || k > 1) return src;
            var dst = new short[512, 512];
            for (int y = 0; y < 512; y++)
            {
                var koef = new Complex(0, 0);
                for (int x = 0; x < 512; x++)
                {
                    var xc = n * x / 512.0 * 2 * Math.PI;
                    koef += src[y, x] * new Complex(Math.Cos(xc), Math.Sin(xc));
                }
                koef = -koef / 256 * k;
                for (int x = 0; x < 512; x++)
                {
                    var xc = n * x / 512.0 * 2 * Math.PI;
                    var v = (koef.Real * Math.Cos(xc) + koef.Imaginary * Math.Sin(xc));
                    var d = src[y, x] + v;
                    dst[y, x] = d < short.MinValue ? short.MinValue : d > short.MaxValue ? short.MaxValue : (short)d;
                }
            }
            return dst;
        }

        public static short[,] Gradient(short[,] src)
        {
            var grad = new short[512, 512];
            if (src == null) return grad;
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 510; y++)
                {
                    grad[y, x] = (short)(/*(src[y, x + 1] - src[y, x]) / 2 +*/ (src[y + 1, x] - src[y, x])/*/2*/);
                }
            }
            return grad;
        }
        public static short[,] GradientAbs(short[,] src)
        {
            var grad = new short[512, 512];
            if (src == null) return grad;
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 510; y++)
                {
                    grad[y, x] = (short)(/*(src[y, x + 1] - src[y, x]) / 2 +*/ Math.Abs(src[y + 1, x] - src[y, x]) / 2);
                }
            }
            return grad;
        }
        public static short[,] DeviationByLine(short[,] src, int windowSize)
        {
            var deviation = new short[512, 512];
            if (src == null) return deviation;
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {
                    /*var xstart = x - windowSize;
                    if (xstart < 0) xstart = 0;
                    var xend = x + windowSize;
                    if (xend >511) xend = 511;*/

                    var ystart = y - windowSize;
                    if (ystart < 0) ystart = 0;
                    var yend = y + windowSize;
                    if (yend > 511) yend = 511;

                    long sum = 0, sum2 = 0;

                    int n = 0;
                    //for (int cx = xstart; x <= xend; x++)
                    //{
                    for (int cy = ystart; cy <= yend; cy++)
                    {
                        var v = src[cy, x];
                        sum += v;
                        sum2 += v * v;
                        n++;
                    }
                    //}
                    var avg = sum / n;
                    var avg2 = sum2 / n;
                    var dev = avg2 - avg * avg;
                    if (dev < 0) dev = 0;
                    deviation[y, x] = (short)Math.Sqrt(dev);
                }
            }
            return deviation;
        }

        public static short[,] DeviationWindow(short[,] src, int windowSize)
        {
            var deviation = new short[512, 512];
            if (src == null) return deviation;
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {
                    var xstart = x - windowSize;
                    if (xstart < 0) xstart = 0;
                    var xend = x + windowSize;
                    if (xend > 511) xend = 511;

                    var ystart = y - windowSize;
                    if (ystart < 0) ystart = 0;
                    var yend = y + windowSize;
                    if (yend > 511) yend = 511;

                    long sum = 0, sum2 = 0;

                    int n = 0;
                    for (int cx = xstart; cx <= xend; cx++)
                    {
                        for (int cy = ystart; cy <= yend; cy++)
                        {
                            var v = src[cy, cx];
                            sum += v;
                            sum2 += v * v;
                            n++;
                        }
                    }
                    var avg = sum / n;
                    var avg2 = sum2 / n;
                    var dev = avg2 - avg * avg;
                    if (dev < 0) dev = 0;
                    deviation[y, x] = (short)Math.Sqrt(dev);
                }
            }
            return deviation;
        }
        public static short[,] DeviationOnDeviationByLine(short[,] src, int windowSize)
        {
            var deviation = new short[512, 512];
            if (src == null) return deviation;
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {
                    /*var xstart = x - windowSize;
                    if (xstart < 0) xstart = 0;
                    var xend = x + windowSize;
                    if (xend >511) xend = 511;*/

                    var ystart = y - windowSize;
                    if (ystart < 0) ystart = 0;
                    var yend = y + windowSize;
                    if (yend > 511) yend = 511;

                    long sum = 0, sum2 = 0;

                    int n = 0;
                    //for (int cx = xstart; x <= xend; x++)
                    //{
                    for (int cy = ystart; cy <= yend; cy++)
                    {
                        var v = src[cy, x];
                        sum += v;
                        sum2 += v * v;
                        n++;
                    }
                    //}
                    var avg = sum / n;
                    var avg2 = sum2 / n;
                    var dev = avg2 - avg * avg;
                    if (dev < 0) dev = 0;
                    var sigma = (short)Math.Sqrt(dev);
                    deviation[y, x] = (short)((src[y, x] - avg) / (sigma + 1) * 1000);
                }
            }
            return deviation;
        }
        public static short[,] DeviationOnDeviation(short[,] src, int windowSize)
        {
            var deviation = new short[512, 512];
            if (src == null) return deviation;
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {
                    var xstart = x - windowSize;
                    if (xstart < 0) xstart = 0;
                    var xend = x + windowSize;
                    if (xend > 511) xend = 511;

                    var ystart = y - windowSize;
                    if (ystart < 0) ystart = 0;
                    var yend = y + windowSize;
                    if (yend > 511) yend = 511;

                    long sum = 0, sum2 = 0;

                    int n = 0;
                    for (int cx = xstart; cx <= xend; cx++)
                    {
                        for (int cy = ystart; cy <= yend; cy++)
                        {
                            var v = src[cy, cx];
                            sum += v;
                            sum2 += v * v;
                            n++;
                        }
                    }
                    var avg = sum / n;
                    var avg2 = sum2 / n;
                    var dev = avg2 - avg * avg;
                    if (dev < 0) dev = 0;
                    var sigma = (short)Math.Sqrt(dev);
                    deviation[y, x] = (short)((src[y, x] - avg) / (sigma + 1) * 1000);
                }
            }
            return deviation;
        }

        public static short[,] NormalizeByFibers(short[,] img, Rectangle region)
        {
            var norm = new short[512, 512];
            var sum = new double[512];
            var sum2 = new double[512];
            var dev = new double[512];
            for (int x = region.X; x < region.X + region.Width; x++)
            {
                for (int y = region.Y; y < region.Y + region.Height; y++)
                {
                    var v = img[y, x];
                    sum[x] += v;
                    sum2[x] += v * v;
                }
                sum[x] /= 512;
                sum2[x] /= 512;
                dev[x] = Math.Sqrt(sum2[x] - sum[x] * sum[x]);
            }
            var maxsum = sum.Max();
            double avg = 0, avg2 = 0, avgdev;
            for (int x = 0; x <= 511; x++)
            {
                var v = sum[x];
                avg += v;
                avg2 += v * v;
            }
            avg /= 512;
            avg2 /= 512;
            avgdev = Math.Sqrt(avg2 - avg * avg);

            double k = 2;

            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {
                    if ((x < region.X || x >= (region.X + region.Width)) && (y < region.Y || y >= (region.Y + region.Height)))
                        norm[y, x] = img[y, x];
                    if (Math.Abs(avg - sum[x]) > k * avgdev)
                        norm[y, x] = img[y, x];
                    else
                        norm[y, x] = (short)(img[y, x] / sum[x] * maxsum);
                }
            }
            return norm;
        }
        public static short[,] NormalizeVertical(short[,] img)
        {
            var norm = new short[512, 512];
            var sum = new double[512];
            var sum2 = new double[512];
            var dev = new double[512];
            for (int y = 0; y <= 511; y++)
            {
                for (int x = 0; x <= 511; x++)
                {
                    var v = img[y, x];
                    sum[y] += v;
                    sum2[y] += v * v;
                }
                sum[y] /= 512;
                sum2[y] /= 512;
                dev[y] = Math.Sqrt(sum2[y] - sum[y] * sum[y]);
            }
            var maxsum = sum.Max();
            double avg = 0, avg2 = 0, avgdev;
            for (int y = 0; y <= 511; y++)
            {
                var v = sum[y];
                avg += v;
                avg2 += v * v;
            }
            avg /= 512;
            avg2 /= 512;
            avgdev = Math.Sqrt(avg2 - avg * avg);

            //double k = 4;

            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {
                    //if (Math.Abs(avg - sum[y]) > k * avgdev)
                    //  norm[y, x] = img[y, x];
                    //else
                    norm[y, x] = (short)(img[y, x] / sum[y] * maxsum);
                }
            }
            return norm;
        }
        public static short[,] FilterDeviation(short[,] img)
        {
            var filtered = new short[512, 512];
            for (int x = 1; x <= 510; x++)
            {
                for (int y = 1; y <= 510; y++)
                {


                    var n = 0;
                    double sum = 0;
                    double sum2 = 0;
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            var sx = x + dx;
                            if (sx < 0 || sx > 511) continue;
                            var sy = y + dy;
                            if (sy < 0 || sy > 511) continue;
                            var v = img[sy, sx];
                            sum += v;
                            sum2 += v * v;
                            n++;
                        }
                    }
                    sum2 /= n;
                    sum /= n;
                    var dev = Math.Sqrt(sum2 - sum * sum);
                    double k = 1.3;
                    if (Math.Abs(sum - img[y, x]) > dev * k)
                        filtered[y, x] = (short)(sum - img[y, x] / 9d);
                    else
                        filtered[y, x] = img[y, x];
                }
            }
            return filtered;
        }

        public static short[,] FilterDeviation(short[,] src, int windowSize)
        {
            var filtered = new short[512, 512];
            if (src == null) return filtered;
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {
                    var xstart = x - windowSize;
                    if (xstart < 0) xstart = 0;
                    var xend = x + windowSize;
                    if (xend > 511) xend = 511;

                    var ystart = y - windowSize;
                    if (ystart < 0) ystart = 0;
                    var yend = y + windowSize;
                    if (yend > 511) yend = 511;

                    long sum = 0, sum2 = 0;

                    int n = 0;
                    for (int cx = xstart; cx <= xend; cx++)
                    {
                        for (int cy = ystart; cy <= yend; cy++)
                        {
                            var v = src[cy, cx];
                            sum += v;
                            sum2 += v * v;
                            n++;
                        }
                    }
                    var avg = sum / n;
                    var avg2 = sum2 / n;
                    var dev = avg2 - avg * avg;
                    if (dev < 0) dev = 0;
                    var sigma = (short)Math.Sqrt(dev);
                    double k = 1.3;
                    if (Math.Abs(avg - src[y, x]) > sigma * k)
                        filtered[y, x] = (short)avg;// (short)(sum - avg);
                    else
                        filtered[y, x] = src[y, x];
                    //filtered[y, x] = (short)((src[y, x] - avg) / (sigma + 1) * 1000);
                }
            }
            return filtered;
        }

        public static short[,] Blur(short[,] img, int N)
        {
            var dc = N / 2;
            var filtered = new short[512, 512];
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {


                    var n = 0;
                    double sum = 0;
                    double sum2 = 0;
                    for (int dx = -dc; dx <= dc; dx++)
                    {
                        for (int dy = -dc; dy <= dc; dy++)
                        {
                            var sx = x + dx;
                            if (sx < 0 || sx > 511) continue;
                            var sy = y + dy;
                            if (sy < 0 || sy > 511) continue;
                            var v = img[sy, sx];
                            sum += v;
                            sum2 += v * v;
                            n++;
                        }
                    }
                    /*
                    sum2 /= n;
                    sum /= n;
                    var dev = Math.Sqrt(sum2 - sum * sum);
                    double k = 1.3;
                    if (Math.Abs(sum - img[y, x]) > dev * k)
                        filtered[y, x] = (short)(sum - img[y, x] / 9d);
                    else
                        filtered[y, x] = img[y, x];*/
                    filtered[y, x] = (short)(sum / n);
                }
            }
            return filtered;
        }
        public static short[,] BlurV(short[,] img, int N)
        {
            var dc = N / 2;
            var filtered = new short[512, 512];
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {


                    var n = 0;
                    double sum = 0;
                    //double sum2 = 0;
                    for (int dy = -dc; dy <= dc; dy++)
                    {
                        //var sx = x + dx;
                        //if (sx < 0 || sx > 511) continue;
                        var sy = y + dy;
                        if (sy < 0 || sy > 511) continue;
                        var v = img[sy, x];
                        sum += v;
                        n++;
                    }

                    filtered[y, x] = (short)(sum / n);
                }
            }
            return filtered;
        }
        public static short[,] Kalman(short[,] img)
        {
            double _err_measure = 400;  // примерный шум измерений
            double _q = 0.1;   // скорость изменения значений 0.001-1, варьировать самому

            var filtered = new short[512, 512];
            for (int x = 0; x <= 511; x++)
            {
                double _kalman_gain, _current_estimate;
                double _err_estimate = _err_measure;
                double _last_estimate = 0;
                for (int y = 0; y <= 511; y++)
                {
                    _kalman_gain = _err_estimate / (_err_estimate + _err_measure);
                    _current_estimate = _last_estimate + _kalman_gain * (img[y, x] - _last_estimate);
                    _err_estimate = (1.0 - _kalman_gain) * _err_estimate + Math.Abs(_last_estimate - _current_estimate) * _q;
                    _last_estimate = _current_estimate;
                    filtered[y, x] = (short)_current_estimate;
                }
            }
            return filtered;

        }
        public static short[,] Z(short[] f)
        {
            var zspace = new short[512, 512];
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {
                    Complex z = new Complex((x - 256) / 128, (y - 256) / 128);
                    Complex F = new Complex(0, 0);
                    for (int t = 0; t < f.Length; t++)
                    {
                        F += f[t] * Complex.Pow(z, new Complex(-t, 0));
                    }
                    var v = F.Magnitude * 100;
                    zspace[y, x] = (short)(v > 32768 ? 32768 : (v < -32767 ? -32767 : v));
                }
            }
            return zspace;
        }
        public static short[,] Furje(short[,] f, int maxThreads)
        {
            Stopwatch sw = new Stopwatch();
            Complex j = new Complex(0, -1);
            var furje = new short[512, 512];
            Complex[] k = new Complex[512 * 512 * 2];
            Complex[,] cf = new Complex[512, 512];

            for (int i = 0; i < 512 * 512 * 2; i++)
            {
                k[i] = Complex.Pow(Math.E, -j * i / 512d * 2 * Math.PI);
            }
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {
                    cf[y, x] = f[y, x] / 32768.0;
                }
            }

            sw.Start();
            Parallel.For(0, 128, new ParallelOptions() { MaxDegreeOfParallelism = maxThreads }, w0 =>
            //for (int w0 = 0; w0 <= 511; w0++)
            {
                for (int w1 = 0; w1 <= 128; w1++)
                {
                    Complex c = new Complex(0, 0);
                    for (int x = 0; x <= 511; x++)
                    {
                        for (int y = 0; y <= 511; y++)
                        {
                            c += cf[y, x] * k[w0 * x + w1 * y];
                        }
                    }
                    furje[w0, w1] = (short)(c.Magnitude / (2 * Math.PI));
                }
            });
            sw.Stop();
            return furje;
        }

        public static short MaxDeviation(short[,] img, Rectangle? rect = null)
        {
            return MaxDeviation(img, out Point _, rect);
        }
        public static short MaxDeviation(short[,] img, out Point Max, Rectangle? rect = null)
        {
            Rectangle r;
            if (rect == null)
                r = new Rectangle(0, 0, 511, 511);
            else
            {
                var X = rect.Value.X;
                var Y = rect.Value.Y;
                var right = rect.Value.Right;
                var bottom = rect.Value.Bottom;
                X = X < 0 ? 0 : X;
                Y = Y < 0 ? 0 : Y;
                right = right >= 512 ? 511 : right;
                bottom = bottom >= 512 ? 511 : bottom;
                r = new Rectangle(X, Y, right - X, bottom - Y);
            }
            Point pMax = new Point(-1, -1);
            short max = short.MinValue;
            for (int x = r.Left; x <= r.Right; x++)
            {
                for (int y = r.Top; y <= r.Bottom; y++)
                {
                    var abs = Math.Abs(img[y, x]);
                    if (max < abs)
                    {
                        max = abs;
                        pMax = new Point(x, y);
                    }

                }
            }
            Max = pMax;
            return max;
        }
        public static short Average(short[,] img, Rectangle? rect = null)
        {
            if (img == null) return 0;
            Rectangle r;
            if (rect == null)
                r = new Rectangle(0, 0, 511, 511);
            else
            {
                var X = rect.Value.X;
                var Y = rect.Value.Y;
                var right = rect.Value.Right;
                var bottom = rect.Value.Bottom;
                X = X < 0 ? 0 : X;
                Y = Y < 0 ? 0 : Y;
                right = right >= 512 ? 511 : right;
                bottom = bottom >= 512 ? 511 : bottom;
                r = new Rectangle(X, Y, right - X, bottom - Y);
            }
            long sum = 0;
            int n = 0;
            for (int x = r.Left; x <= r.Right; x++)
            {
                for (int y = r.Top; y <= r.Bottom; y++)
                {
                    sum += img[y, x];
                    n++;
                }
            }
            return (short)(sum / n);
        }
        public static ImageProcessResult CheckIfSocketGood(short[,] Current, short[,] StandardImage, ImageProcessParameters ipp)
        {
            var result = new ImageProcessResult();
            result.IsSocketGood = true;
            result.SocketErrorType = ImageErrorType.None;
            if (ipp.Decisions != null)
            {
                for (int i = 0; i < ipp.Decisions.Length; i++)
                {

                    var imgres = ipp.Decisions[i].IsImageGood(StandardImage, Current, ipp, out short[,] resImg, out Point maxCoord);
                    if (!imgres)
                    {
                        result.IsSocketGood = false;
                        result.SocketErrorType |= ipp.Decisions[i].Result == DecisionActionResult.Defect ? ImageErrorType.Defect : ImageErrorType.Average;
                    }
                    result.MaxDeviationPoint = maxCoord;
                    if (i == 0) result.ResultImage = resImg;

                }
                return result;
            }
            return result;

        }

        public static short[] GetVLine(short[,] img, int line)
        {
            if (img != null)
            {
                var aline = new short[img.GetLength(0)];
                for (int i = 0; i < aline.Length; i++)
                {
                    aline[i] = img[i, line];
                }
                return aline;
            }
            return null;
        }
        public static short[] GetHLine(short[,] img, int line)
        {
            if (img != null)
            {
                var aline = new short[img.GetLength(0)];
                for (int i = 0; i < aline.Length; i++)
                {
                    aline[i] = img[line, i];
                }
                return aline;
            }
            return null;
        }

        public static short[,] Multiply(short[,] img, double v)
        {
            if (img == null) return null;
            var res = new short[img.GetLength(0), img.GetLength(1)];
            for (int x = 0; x <= 511; x++)
            {
                for (int y = 0; y <= 511; y++)
                {
                    //if (Math.Abs(avg - sum[y]) > k * avgdev)
                    //  norm[y, x] = img[y, x];
                    //else
                    res[y, x] = (short)(img[y, x] * v);
                }
            }
            return res;
        }

        public static short[,] NormalizeByFibers(short[,] img, Rectangle region, short ToMax)
        {
            var res = new short[img.GetLength(0), img.GetLength(1)];
            for (int x = region.X; x < region.X + region.Width; x++)
            {
                short max = short.MinValue;
                for (int y = region.Y; y < region.Y + region.Height; y++)
                {
                    if (max < img[y, x]) max = img[y, x];
                }
                var v = (double)ToMax / max;
                for (int y = region.Y; y < region.Y + region.Height; y++)
                {
                    res[y, x] = (short)(img[y, x] * v);
                }
            }
            return res;
        }
        public static short[,] Denoise(short[,] img)
        {
            return BlurV(img, 5);
            /*var res = img.Clone() as short[,];
            for (var iter = 0; iter < 10; iter++)
            {
                double MaxA = 0d;
                double MaxK = 0d;
                for (int x = 0; x < 1; x++)
                {
                    double a = 0, b = 0;
                    var N = 512d;
                    for (double k = 1; k < N; k++)
                    //double k = N / 2;
                    {
                        for (int y = 0; y < 512; y++)
                        {
                            var wx = Math.PI * 2 * k * y / N;
                            a += res[y, x] * Math.Cos(wx);
                            b += res[y, x] * Math.Sin(wx);
                        }
                        var A = Math.Sqrt(a * a + b * b);
                        if (MaxA < A)
                        {
                            MaxA = A;
                            MaxK = k;
                        }
                    }
                }
                for (int x = 0; x < 511; x++)
                {
                    double a = 0, b = 0;
                    var N = 512d;
                    //for (double k = N - 1; k < N; k++)
                    double k = MaxK;
                    {
                        for (int y = 0; y < 512; y++)
                        {
                            var wx = Math.PI * 2 * k * y / N;
                            a += img[y, x] * Math.Cos(wx);
                            b += img[y, x] * Math.Sin(wx);
                        }
                        var A = Math.Sqrt(a * a + b * b);
                        var f = Math.Asin(a / A);
                        for (int y = 0; y < 512; y++)
                        {
                            var wx = Math.PI * 2 * k * y / N;

                            //res[y, x] = (short)(img[y, x] - (a * Math.Cos(wx) + b * Math.Sin(wx)) / N);
                            res[y, x] = (short)(img[y, x] - (A * Math.Sin(wx + f)) / N);
                        }
                    }
                }
            }
            return res;*/
        }
        public static short[] GetColumn(short[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }

        public static short[] GetRow(short[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }
    }

    public class Deviation
    {
        public double[][] SocketDeviationByLines;
        public double[] SocketDeviation;
        public double TotalDeviation;

        public double[][] SocketAverageByLinesSquared;
        public double[][] SocketAverageByLines;
        public double[] SocketAverage;
        public double[] SocketAverageSquared;
        public double TotalAverage;
        public double TotalAverageSquared;

        public short[] Max;
        public short[] Min;


    }


}
