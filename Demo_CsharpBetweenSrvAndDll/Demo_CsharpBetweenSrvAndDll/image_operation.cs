using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Demo_CsharpBetweenSrvAndDll
{
    public class image_operation
    {

        public Bitmap gBMP = null;

        public void inversed_bmp_gray(Bitmap In_Bmp, Bitmap Out_Bmp)
        {
            //input image
            BitmapData BmpData = In_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Height), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);

            System.IntPtr ScanG = BmpData.Scan0;
            int image_range = BmpData.Stride;
            int w = BmpData.Width;
            int h = BmpData.Height;
            int offset = image_range - w;

            //output image
            BitmapData OutBmpData = Out_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Height), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);
            System.IntPtr ScanOut = OutBmpData.Scan0;

            unsafe
            {
                byte* g = (byte*)(void*)ScanG;
                byte* g_out = (byte*)(void*)ScanOut;

                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        g_out[i + (j * w)] = (byte)(255 - g[i + (j * w)]);
                    }
                }
            }

            In_Bmp.UnlockBits(BmpData);
            Out_Bmp.UnlockBits(OutBmpData);
        }

        private void ImgProessingADPThres(Bitmap In_Bmp, Bitmap Out_Bmp, int filter_size)
        {

            uint avg;
            uint p0, p1, p2;
            uint p3, p4, p5;
            uint p6, p7, p8;
            uint p9, p10, p11;
            uint p12, p13, p14;
            uint p15, p16, p17;
            uint p18, p19, p20;
            uint p21, p22, p23, p24;
            uint pc;

            //input image
            BitmapData BmpData = In_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Width), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);

            System.IntPtr ScanG = BmpData.Scan0;
            int image_range = BmpData.Stride;
            int w = BmpData.Width;
            int h = BmpData.Height;
            int offset = image_range - w;
            int start_point;

            //output image
            BitmapData OutBmpData = Out_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Width), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);
            System.IntPtr ScanOut = OutBmpData.Scan0;

            if (filter_size == 9)
            {
                start_point = 1;
            }
            else //filter_size ==25
            {
                start_point = 2;
            }

            unsafe
            {
                byte* g = (byte*)(void*)ScanG;
                byte* g_out = (byte*)(void*)ScanOut;

                for (int j = start_point; j < (h - start_point); j++)
                {
                    for (int i = start_point; i < (w - start_point); i++)
                    {
                        if (filter_size == 9)
                        {
                            p0 = g[(i - 1) + (j * w) - 1 * w];
                            p1 = g[(i - 0) + (j * w) - 1 * w];
                            p2 = g[(i + 1) + (j * w) - 1 * w];

                            p3 = g[(i - 1) + (j * w) - 0 * w];
                            p4 = g[(i - 0) + (j * w) - 0 * w];
                            p5 = g[(i + 1) + (j * w) - 0 * w];

                            p6 = g[(i - 1) + (j * w) + 1 * w];
                            p7 = g[(i - 0) + (j * w) + 1 * w];
                            p8 = g[(i + 1) + (j * w) + 1 * w];

                            avg = (p0 + p1 + p2 + p3 + 0 + p5 + p6 + p7 + p8) / 8;
                            pc = p4;
                        }
                        else
                        {
                            p0 = g[(i - 2) + (j * w) - 2 * w];
                            p1 = g[(i - 1) + (j * w) - 2 * w];
                            p2 = g[(i - 0) + (j * w) - 2 * w];
                            p3 = g[(i + 1) + (j * w) - 2 * w];
                            p4 = g[(i + 2) + (j * w) - 2 * w];

                            p5 = g[(i - 2) + (j * w) - 1 * w];
                            p6 = g[(i - 1) + (j * w) - 1 * w];
                            p7 = g[(i - 0) + (j * w) - 1 * w];
                            p8 = g[(i + 1) + (j * w) - 1 * w];
                            p9 = g[(i + 2) + (j * w) - 1 * w];

                            p10 = g[(i - 2) + (j * w) - 0 * w];
                            p11 = g[(i - 1) + (j * w) - 0 * w];
                            p12 = g[(i - 0) + (j * w) - 0 * w];
                            p13 = g[(i + 1) + (j * w) - 0 * w];
                            p14 = g[(i + 2) + (j * w) - 0 * w];

                            p15 = g[(i - 2) + (j * w) + 1 * w];
                            p16 = g[(i - 1) + (j * w) + 1 * w];
                            p17 = g[(i - 0) + (j * w) + 1 * w];
                            p18 = g[(i + 1) + (j * w) + 1 * w];
                            p19 = g[(i + 2) + (j * w) + 1 * w];

                            p20 = g[(i - 2) + (j * w) + 2 * w];
                            p21 = g[(i - 1) + (j * w) + 2 * w];
                            p22 = g[(i - 0) + (j * w) + 2 * w];
                            p23 = g[(i + 1) + (j * w) + 2 * w];
                            p24 = g[(i + 2) + (j * w) + 2 * w];

                            avg = (p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + 0 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24) / 24;
                            pc = p12;
                        }

                        if (pc >= avg)
                        {
                            g_out[i + (j * w)] = 255;
                        }
                        else
                        {
                            g_out[i + (j * w)] = 0;
                        }
                    }
                }
            }

            In_Bmp.UnlockBits(BmpData);
            Out_Bmp.UnlockBits(OutBmpData);
        }

        private void ImgProessingADPThres_Advanced(Bitmap In_Bmp, Bitmap Out_Bmp, int radius)
        {

            uint avg;
            uint pc;

            //input image
            BitmapData BmpData = In_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Width), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);

            System.IntPtr ScanG = BmpData.Scan0;
            int image_range = BmpData.Stride;
            int w = BmpData.Width;
            int h = BmpData.Height;
            int offset = image_range - w;
            int start_point;

            //output image
            BitmapData OutBmpData = Out_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Width), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);
            System.IntPtr ScanOut = OutBmpData.Scan0;

            start_point = radius;
            unsafe
            {
                byte* g = (byte*)(void*)ScanG;
                byte* g_out = (byte*)(void*)ScanOut;

                for (int j = start_point; j < (h - start_point); j++)
                {
                    for (int i = start_point; i < (w - start_point); i++)
                    {
                        int sum = 0, add = 0;
                        for (int y = (-radius); y < (radius + 1); y++)
                        {
                            for (int x = (-radius); x < (radius + 1); x++)
                            {
                                sum = sum + g[(i + x) + (j * w) + y * w];
                                add++;
                            }
                        }
                        pc = g[(i + 0) + (j * w) + 0 * w];

                        avg = (uint)((sum - (int)pc) / (add - 1));



                        if (pc >= avg)
                        {
                            g_out[i + (j * w)] = 255;
                        }
                        else
                        {
                            g_out[i + (j * w)] = 0;
                        }
                    }
                }
            }

            In_Bmp.UnlockBits(BmpData);
            Out_Bmp.UnlockBits(OutBmpData);
        }

        private void ImgProessingFixThres(Bitmap In_Bmp, Bitmap Out_Bmp, Int16 threshold)
        {

            uint avg;
            uint p0, p1, p2;
            uint p3, p4, p5;
            uint p6, p7, p8;

            //input image
            BitmapData BmpData = In_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Width), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);

            System.IntPtr ScanG = BmpData.Scan0;
            int image_range = BmpData.Stride;
            int w = BmpData.Width;
            int h = BmpData.Height;
            int offset = image_range - w;

            //output image
            BitmapData OutBmpData = Out_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Width), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);
            System.IntPtr ScanOut = OutBmpData.Scan0;

            unsafe
            {
                byte* g = (byte*)(void*)ScanG;
                byte* g_out = (byte*)(void*)ScanOut;

                for (int j = 1; j < (h - 1); j++)
                {
                    for (int i = 1; i < (w - 1); i++)
                    {
                        p0 = g[(i - 1) + (j * w) - 1 * w];
                        p1 = g[(i - 0) + (j * w) - 1 * w];
                        p2 = g[(i + 1) + (j * w) - 1 * w];

                        p3 = g[(i - 1) + (j * w) - 0 * w];
                        p4 = g[(i - 0) + (j * w) - 0 * w];
                        p5 = g[(i + 1) + (j * w) - 0 * w];

                        p6 = g[(i - 1) + (j * w) + 1 * w];
                        p7 = g[(i - 0) + (j * w) + 1 * w];
                        p8 = g[(i + 1) + (j * w) + 1 * w];

                        avg = (p0 + p1 + p2 + p3 + 0 + p5 + p6 + p7 + p8) / 8;
                        if (p4 >= threshold)
                        {
                            g_out[i + (j * w)] = 255;
                        }
                        else
                        {
                            g_out[i + (j * w)] = 0;
                        }
                    }
                }
            }

            In_Bmp.UnlockBits(BmpData);
            Out_Bmp.UnlockBits(OutBmpData);
        }

        private void cal_histogram(Bitmap In_Bmp, int[] histogram)
        {
            byte pc;
            //input image
            BitmapData BmpData = In_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Width), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);

            System.IntPtr ScanG = BmpData.Scan0;
            int image_range = BmpData.Stride;
            int w = BmpData.Width;
            int h = BmpData.Height;
            int offset = image_range - w;

            for (int k = 0; k < 256; k++)
            {
                histogram[k] = 0;
            }

            unsafe
            {
                byte* g = (byte*)(void*)ScanG;

                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < image_range; i++)
                    {
                        pc = g[i + (j * w)];
                        if (pc != 255)
                        {
                            histogram[pc]++;
                        }


                    }
                }
            }

            In_Bmp.UnlockBits(BmpData);

        }

        public byte[] get_bmp_raw(Bitmap In_Bmp)
        {
            //input image
            BitmapData BmpData = In_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Height), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);

            System.IntPtr SrcScanG = BmpData.Scan0;
            int image_range = BmpData.Stride;
            int w = BmpData.Width;
            int h = BmpData.Height;
            byte[] raw = new byte[w * h];

            unsafe
            {
                byte* g = (byte*)(void*)SrcScanG;

                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        raw[i + (j * w)] = (byte) g[i + (j * image_range)];
                    }
                }
            }

            In_Bmp.UnlockBits(BmpData);

            return raw;
        }

        public Bitmap raw_to_bmp(int width, int height, byte[] raw)
        {
            Bitmap In_Bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            //new color table
            ColorPalette monoPalette = In_Bmp.Palette;
            Color[] entries = monoPalette.Entries;
            for (int i = 0; i < 256; i++)
            {
                entries[i] = Color.FromArgb(0xff, i, i, i);
            }
            In_Bmp.Palette = monoPalette;

            //input image
            BitmapData BmpData = In_Bmp.LockBits(new Rectangle(0, 0, In_Bmp.Width, In_Bmp.Height), ImageLockMode.ReadWrite, In_Bmp.PixelFormat);

            System.IntPtr ScanG = BmpData.Scan0;
            //int image_range = BmpData.Stride;
            int w = BmpData.Width;
            int h = BmpData.Height;

            unsafe
            {
                byte* g = (byte*)(void*)ScanG;

                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        g[i + (j * w)] = raw[i + (j * w)];
                    }
                }
            }

            In_Bmp.UnlockBits(BmpData);

            return In_Bmp;
        }

    }
}
