/***
 * 數位影像處理課程 HW1
 * 任何問題請上作業網頁或洽詢助教
 * http://imzack.github.io/Digital-Image-Processing/
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace ImageProcess
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private DisplayImage displayImage;
        public delegate void EventHandler(object sender, ImageSource e);
        public event EventHandler showImage;

        private int height;
        private int width;
        private int stride;
        private byte[] pixels;
        private WriteableBitmap wb;
        private WriteableBitmap wbRotate;

        private int mosaicBlockSize = 32;
        private int rotateAngleDegree = 45;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PICS (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            if (dlg.ShowDialog() == true)
            {
                inputImage.Source = new BitmapImage(new Uri(dlg.FileName));

                // Load
                wb = new WriteableBitmap(inputImage.Source as BitmapSource);
                width = wb.PixelWidth;
                height = wb.PixelHeight;
                stride = width * wb.Format.BitsPerPixel / 8;
                pixels = new byte[height * stride];
                wb.CopyPixels(pixels, stride, 0);
            }

            processBox.IsEnabled = true;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPEG|*.jpg";
            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;

                var encoder = new JpegBitmapEncoder();
                BitmapFrame frame = BitmapFrame.Create(wb);
                encoder.Frames.Add(frame);
                using (var stream = File.Create(filename))
                {
                    encoder.Save(stream);
                }
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void grayBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] temp = (byte[])pixels.Clone();

            /**
             * 轉灰階演算法
             **/
            for (int i = 0; i < height * stride; i++)
            {
                byte gray = (byte)((temp[i] + temp[i + 1] + temp[i + 2]) / 3);
                temp[i] = gray;// Blue
                temp[i + 1] = gray;// Green 
                temp[i + 2] = gray;// Red
                // pixels[i+3] = Alpha
                i+=3;
            }
            //////////////////////////////////////////////////////////////////////
            wb.WritePixels(new Int32Rect(0, 0, width, height), temp, stride, 0);
            outputImage.Source = wb;
        }

        private void mirrorBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] temp = (byte[])pixels.Clone();

            /**
             * 鏡像演算法
             **/


            //////////////////////////////////////////////////////////////////////

            wb.WritePixels(new Int32Rect(0, 0, width, height), temp, stride, 0);
            outputImage.Source = wb;
        }

        public static void Swap(ref byte foo, ref byte bar)
        {
            byte tmp = foo;
            foo = bar;
            bar = tmp;
        }

        private void mosaicBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] temp = (byte[])pixels.Clone();

            /**
             * 馬賽克演算法
             **/



            //////////////////////////////////////////////////////////////////////

            wb.WritePixels(new Int32Rect(0, 0, width, height), temp, stride, 0);
            outputImage.Source = wb;
        }

        private void rotateBtn_Click(object sender, RoutedEventArgs e)
        {
            /**
             * 旋轉
             **/


            //////////////////////////////////////////////////////////////////////
        }

        private void mosaicBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mosaicBlockSize = (int)e.NewValue;
            mosaicBtn.Content = "Mosaic\r\n(" + mosaicBlockSize + "x" + mosaicBlockSize + ")";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mosaicBtn.Content = "Mosaic\r\n(" + mosaicBlockSize + "x" + mosaicBlockSize + ")";
            mosaicBlock.Value = mosaicBlockSize;
            displayImage = new DisplayImage();
            showImage += new EventHandler(displayImage.updateImage);
        }

        private void inputImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (displayImage == null)
            {
                displayImage = new DisplayImage();
                showImage += new EventHandler(displayImage.updateImage);
            }
            showImage(this, inputImage.Source);
            displayImage.Show();
        }

        private void outputImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (displayImage == null)
            {
                displayImage = new DisplayImage();
                showImage += new EventHandler(displayImage.updateImage);
            }
            showImage(this, outputImage.Source);
            displayImage.Show();
        }

        private void rotateDegree_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rotateAngleDegree = (int) e.NewValue;
            rotateBtn.Content = "Rotate\r\n(" + rotateAngleDegree + ")";
        }
    }
}
