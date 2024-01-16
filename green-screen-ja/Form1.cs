using green_screen_ja.Helpers;
using green_screen_ja.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;


namespace green_screen_ja
{
    public partial class Form1 : Form
    {

        [DllImport(@"C:\projekty\green-screen-ja\x64\Debug\CppLib.dll")]
        public static extern unsafe void removeGreenScreenCPP(byte* pixelArray, byte* colorRgbBytes, int size);

        [DllImport(@"C:\projekty\green-screen-ja\x64\Debug\JAAsm.dll")]
        public static extern unsafe void removeGreenScreenASM(byte* pixelArray, byte* colorRgbBytes, int size);

        private ImageStore imageStore;

        private bool isASM = true;

        public Form1()
        {
            InitializeComponent();
        }

        private byte[] toPixels(Bitmap inputBitmap)
        {
            int width = inputBitmap.Width;
            int height = inputBitmap.Height;
            int arraySize = width * height * 4;

            byte[] pixelArray = new byte[arraySize];

            int pixelIndex = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pixelArray[pixelIndex] = inputBitmap.GetPixel(j, i).A;
                    pixelArray[pixelIndex + 1] = inputBitmap.GetPixel(j, i).R;
                    pixelArray[pixelIndex + 2] = inputBitmap.GetPixel(j, i).G;
                    pixelArray[pixelIndex + 3] = inputBitmap.GetPixel(j, i).B;

                    pixelIndex += 4;
                }
            }
            return pixelArray;
        }

        private byte[] getRgbColorBytes()
        {
           
                byte[] colorRgbBytes = new byte[3];
                colorRgbBytes[0] = 62;
                colorRgbBytes[1] = 255;
                colorRgbBytes[2] = 40;

                return colorRgbBytes;
      
        }

        private Bitmap toOutputBitmap(byte[] pixelArray, int width, int height)
        {
            Bitmap outputBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            int pixelIndex = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var pixelColor = Color.FromArgb(pixelArray[pixelIndex], pixelArray[pixelIndex + 1], pixelArray[pixelIndex + 2], pixelArray[pixelIndex + 3]);
                    outputBitmap.SetPixel(j, i, pixelColor);
                    pixelIndex += 4;
                }
            }
            return outputBitmap;
        }


        private void RunCppDll(byte[] pixelArray, byte[] colorToRemoveRgb, int size)
        {
            unsafe
            {
                fixed (byte* colorToRemoveRgbPtr = &colorToRemoveRgb[0])
                {
                    fixed (byte* pixelArrayPtr = &pixelArray[0])
                    {
                        removeGreenScreenCPP(pixelArrayPtr, colorToRemoveRgbPtr, size);
                    }
                }
            }
        }

        private void RunASMDll(byte[] pixelArray, byte[] colorToRemoveRgb, int size)
        {
            unsafe
            {
                fixed (byte* colorToRemoveRgbPtr = &colorToRemoveRgb[0])
                {
                    fixed (byte* pixelArrayPtr = &pixelArray[0])
                    {
                        removeGreenScreenASM(pixelArrayPtr, colorToRemoveRgbPtr, size);
                    }
                }
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            isASM = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (imageStore == null)
            {
                MessageBox.Show("You have to choose file first", "Warning");
            } else
            {
                imageStore.Pixels = this.toPixels(imageStore.InputImage);
                byte[] arrayRgbColorBytes = this.getRgbColorBytes(); // ToDo args

                int treads = Int32.Parse(labelThreadsTrackBarValue.Text);
                //int treads = Int32.Parse("1");

                List<byte[]> arrayList = ThreadsManager.SplitPixelArray(imageStore.Pixels, treads);

                if (isASM)
                {
                    ThreadsManager.RunThreads(new Action<byte[], byte[], int>(this.RunASMDll),
                           arrayList, arrayRgbColorBytes, treads);
                }
                else
                {
                    ThreadsManager.RunThreads(new Action<byte[], byte[], int>(this.RunCppDll),
                           arrayList, arrayRgbColorBytes, treads);
                }

                imageStore.Pixels = ThreadsManager.MergeArray(arrayList);

                imageStore.OutputImage = this.toOutputBitmap(imageStore.Pixels, imageStore.GetInputWidth(), imageStore.GetInputHeight());
                MemoryStream memoryStream = new MemoryStream();
                imageStore.OutputImage.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;
                outputImage.Image = imageStore.OutputImage;

                stopWatch.Stop();
                labelTime.Text = stopWatch.Elapsed.ToString();
            }
        }

        private void fileSelectButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.bmp;*.jpg;*.png;|JPG|*.jpg|BMP|*.bmp|PNG|*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            openFileDialog.ShowDialog();

            if (String.IsNullOrEmpty(openFileDialog.FileName))
            {
                MessageBox.Show("No file chosen!", "Warning");
            }
            else
            {
                sourceImage.ImageLocation = openFileDialog.FileName;

                imageStore = new ImageStore { InputImage = new Bitmap(openFileDialog.FileName) };
               
            }
        }

        private void threadsTrackBar_Scroll(object sender, EventArgs e)
        {
            labelThreadsTrackBarValue.Text = Math.Pow(2, threadsTrackBar.Value).ToString();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            isASM = false;
        }
    }
}
