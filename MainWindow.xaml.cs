//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.CoordinateMappingBasics
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Size of the RGB pixel in the bitmap
        /// </summary>
        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for depth/color/body index frames
        /// </summary>
        private MultiSourceFrameReader multiFrameSourceReader = null;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap bitmap = null;

        /// <summary>
        /// The size in bytes of the bitmap back buffer
        /// </summary>
        private uint bitmapBackBufferSize = 0;

        /// <summary>
        /// Intermediate storage for the color to depth mapping
        /// </summary>
        private DepthSpacePoint[] colorMappedToDepthPoints = null;

        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText = null;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            this.kinectSensor = KinectSensor.GetDefault();

            this.multiFrameSourceReader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color | FrameSourceTypes.BodyIndex);

            this.multiFrameSourceReader.MultiSourceFrameArrived += this.Reader_MultiSourceFrameArrived;

            this.coordinateMapper = this.kinectSensor.CoordinateMapper;

            FrameDescription depthFrameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

            int depthWidth = depthFrameDescription.Width;
            int depthHeight = depthFrameDescription.Height;

            FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.FrameDescription;

            int colorWidth = colorFrameDescription.Width;
            int colorHeight = colorFrameDescription.Height;

            this.colorMappedToDepthPoints = new DepthSpacePoint[colorWidth * colorHeight];

            this.bitmap = new WriteableBitmap(colorWidth, colorHeight, 96.0, 96.0, PixelFormats.Bgra32, null);
            
            // Calculate the WriteableBitmap back buffer size
            this.bitmapBackBufferSize = (uint)((this.bitmap.BackBufferStride * (this.bitmap.PixelHeight - 1)) + (this.bitmap.PixelWidth * this.bytesPerPixel));
                                   
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            //this.kinectSensor.Open();

            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            this.DataContext = this;

            this.InitializeComponent();
            this.fileStructure();
        }

        string myPhotos, path, pathUI, pathIni, pathWaiting, pathSuccess, pathDefault, pathBackground;

        BitmapImage Image1 = new BitmapImage();
        BitmapImage Image2 = new BitmapImage();
        BitmapImage Image3 = new BitmapImage();
        BitmapImage Image4 = new BitmapImage();
        BitmapImage Image5 = new BitmapImage();
        BitmapImage Image6 = new BitmapImage();

        public void fileStructure()
        {
            myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            path = Path.Combine(myPhotos, "kinectBackground");
            pathUI = Path.Combine(path, "UI");
            pathIni = Path.Combine(path, "Initial");
            pathWaiting = Path.Combine(path, "Waiting");
            pathSuccess = Path.Combine(path, "Success");
            pathDefault = Path.Combine(path, "Default");
            pathBackground = Path.Combine(path, "Background");
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(pathUI);
            Directory.CreateDirectory(pathIni);
            Directory.CreateDirectory(pathWaiting);
            Directory.CreateDirectory(pathSuccess);
            Directory.CreateDirectory(pathDefault);
            Directory.CreateDirectory(pathBackground);
            Thread.Sleep(200);
            try
            {
                BitmapImage welcome = new BitmapImage();
                welcome.BeginInit();
                welcome.UriSource = new Uri(pathUI + "\\welcome.png", UriKind.Absolute);
                welcome.EndInit();
                welcomeImg.Source = welcome;
                BitmapImage thankyou = new BitmapImage();
                thankyou.BeginInit();
                thankyou.UriSource = new Uri(pathUI + "\\thankyou.png", UriKind.Absolute);
                thankyou.EndInit();
                thankyouImg.Source = thankyou;
                //BitmapImage Image1 = new BitmapImage();
                Image1.BeginInit();
                Image1.UriSource = new Uri(pathBackground + "\\img1.png", UriKind.Absolute);
                Image1.EndInit();
                img1.Source = Image1;
                //BitmapImage Image2 = new BitmapImage();
                Image2.BeginInit();
                Image2.UriSource = new Uri(pathBackground + "\\img2.png", UriKind.Absolute);
                Image2.EndInit();
                img2.Source = Image2;
                //BitmapImage Image3 = new BitmapImage();
                Image3.BeginInit();
                Image3.UriSource = new Uri(pathBackground + "\\img3.png", UriKind.Absolute);
                Image3.EndInit();
                img3.Source = Image3;
                //BitmapImage Image4 = new BitmapImage();
                Image4.BeginInit();
                Image4.UriSource = new Uri(pathBackground + "\\img4.png", UriKind.Absolute);
                Image4.EndInit();
                img4.Source = Image4;
                //BitmapImage Image5 = new BitmapImage();
                Image5.BeginInit();
                Image5.UriSource = new Uri(pathBackground + "\\img5.png", UriKind.Absolute);
                Image5.EndInit();
                img5.Source = Image5;
                //BitmapImage Image6 = new BitmapImage();
                Image6.BeginInit();
                Image6.UriSource = new Uri(pathBackground + "\\img6.png", UriKind.Absolute);
                Image6.EndInit();
                img6.Source = Image6;

            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void UI_handler()
        {
            if (welcomeUI.IsVisible)
            {
                welcomeUI.Visibility = Visibility.Hidden;
                thankyouUI.Visibility = Visibility.Hidden;
                background_select.Visibility = Visibility.Visible;
            }
            else if(MainScreen.IsVisible)
            {
                welcomeUI.Visibility = Visibility.Hidden;
                thankyouUI.Visibility = Visibility.Hidden;
                background_select.Visibility = Visibility.Hidden;
                Screenshot_btn.Visibility = Visibility.Visible;
                Back_btn.Visibility = Visibility.Visible;

            }
        }

        private void start_btn(object sender, RoutedEventArgs e)
        {
            UI_handler();
            //welcomeUI.Visibility = Visibility.Hidden;
            //thankyouUI.Visibility = Visibility.Hidden;
            //background_select.Visibility = Visibility.Visible;
        }

        private void bck_select_Onclick(object sender, RoutedEventArgs e)
        {
            Image backgroundSelect = (Image)sender;
            MessageBox.Show(backgroundSelect.Name.ToString());
            MainScreen.Visibility = Visibility.Visible;
            UI_handler();
            this.kinectSensor.Open();
            if(sender.Equals(this.img2))
            {
                BackgroundIMG.Source = Image2;
            }
            else if(sender.Equals(this.img1))
            {
                BackgroundIMG.Source = Image1;
            }
            else if (sender.Equals(this.img3))
            {
                BackgroundIMG.Source = Image3;
            }
            else if (sender.Equals(this.img4))
            {
                BackgroundIMG.Source = Image4;
            }
            else if (sender.Equals(this.img5))
            {
                BackgroundIMG.Source = Image5;
            }
            else if (sender.Equals(this.img6))
            {
                BackgroundIMG.Source = Image6;
            }
           

        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            Back_btn.Visibility = Visibility.Hidden;
            welcomeUI.Visibility = Visibility.Hidden;
            thankyouUI.Visibility = Visibility.Hidden;
            background_select.Visibility = Visibility.Visible;
            Screenshot_btn.Visibility = Visibility.Hidden;
            MainScreen.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return this.bitmap;
            }
        }

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;

                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.multiFrameSourceReader != null)
            {
                // MultiSourceFrameReder is IDisposable
                this.multiFrameSourceReader.Dispose();
                this.multiFrameSourceReader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }
         private void kinect_sensor_handle(bool kinect_open)
         {
            if(kinect_open == true)
            {
                this.kinectSensor.Open();
            }
            if(this.kinectSensor != null && kinect_open == false)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
         }

        /// <summary>
        /// Handles the user clicking on the screenshot button
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a render target to which we'll render our composite image
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)CompositeImage.ActualWidth, (int)CompositeImage.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(CompositeImage);
                dc.DrawRectangle(brush, null, new Rect(new Point(), new Size(CompositeImage.ActualWidth, CompositeImage.ActualHeight)));
            }

            renderBitmap.Render(dv);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            string time = System.DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

            string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            string path = Path.Combine(myPhotos, "KinectScreenshot-CoordinateMapping-" + time + ".png");

            // Write the new file to disk
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(fs);
                }

                this.StatusText = string.Format(Properties.Resources.SavedScreenshotStatusTextFormat, path);
            }
            catch (IOException)
            {
                this.StatusText = string.Format(Properties.Resources.FailedScreenshotStatusTextFormat, path);
            }
        }

        /// <summary>
        /// Handles the depth/color/body index frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            int depthWidth = 0;
            int depthHeight = 0;
                    
            DepthFrame depthFrame = null;
            ColorFrame colorFrame = null;
            BodyIndexFrame bodyIndexFrame = null;
            bool isBitmapLocked = false;

            MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();           

            // If the Frame has expired by the time we process this event, return.
            if (multiSourceFrame == null)
            {
                return;
            }

            // We use a try/finally to ensure that we clean up before we exit the function.  
            // This includes calling Dispose on any Frame objects that we may have and unlocking the bitmap back buffer.
            try
            {                
                depthFrame = multiSourceFrame.DepthFrameReference.AcquireFrame();
                colorFrame = multiSourceFrame.ColorFrameReference.AcquireFrame();
                bodyIndexFrame = multiSourceFrame.BodyIndexFrameReference.AcquireFrame();

                // If any frame has expired by the time we process this event, return.
                // The "finally" statement will Dispose any that are not null.
                if ((depthFrame == null) || (colorFrame == null) || (bodyIndexFrame == null))
                {
                    return;
                }

                // Process Depth
                FrameDescription depthFrameDescription = depthFrame.FrameDescription;

                depthWidth = depthFrameDescription.Width;
                depthHeight = depthFrameDescription.Height;

                // Access the depth frame data directly via LockImageBuffer to avoid making a copy
                using (KinectBuffer depthFrameData = depthFrame.LockImageBuffer())
                {
                    this.coordinateMapper.MapColorFrameToDepthSpaceUsingIntPtr(
                        depthFrameData.UnderlyingBuffer,
                        depthFrameData.Size,
                        this.colorMappedToDepthPoints);
                }

                // We're done with the DepthFrame 
                depthFrame.Dispose();
                depthFrame = null;

                // Process Color

                // Lock the bitmap for writing
                this.bitmap.Lock();
                isBitmapLocked = true;

                colorFrame.CopyConvertedFrameDataToIntPtr(this.bitmap.BackBuffer, this.bitmapBackBufferSize, ColorImageFormat.Bgra);

                // We're done with the ColorFrame 
                colorFrame.Dispose();
                colorFrame = null;

                // We'll access the body index data directly to avoid a copy
                using (KinectBuffer bodyIndexData = bodyIndexFrame.LockImageBuffer())
                {
                    unsafe
                    {
                        byte* bodyIndexDataPointer = (byte*)bodyIndexData.UnderlyingBuffer;

                        int colorMappedToDepthPointCount = this.colorMappedToDepthPoints.Length;

                        fixed (DepthSpacePoint* colorMappedToDepthPointsPointer = this.colorMappedToDepthPoints)
                        {
                            // Treat the color data as 4-byte pixels
                            uint* bitmapPixelsPointer = (uint*)this.bitmap.BackBuffer;

                            // Loop over each row and column of the color image
                            // Zero out any pixels that don't correspond to a body index
                            for (int colorIndex = 0; colorIndex < colorMappedToDepthPointCount; ++colorIndex)
                            {
                                float colorMappedToDepthX = colorMappedToDepthPointsPointer[colorIndex].X;
                                float colorMappedToDepthY = colorMappedToDepthPointsPointer[colorIndex].Y;

                                // The sentinel value is -inf, -inf, meaning that no depth pixel corresponds to this color pixel.
                                if (!float.IsNegativeInfinity(colorMappedToDepthX) &&
                                    !float.IsNegativeInfinity(colorMappedToDepthY))
                                {
                                    // Make sure the depth pixel maps to a valid point in color space
                                    int depthX = (int)(colorMappedToDepthX + 0.5f);
                                    int depthY = (int)(colorMappedToDepthY + 0.5f);

                                    // If the point is not valid, there is no body index there.
                                    if ((depthX >= 0) && (depthX < depthWidth) && (depthY >= 0) && (depthY < depthHeight))
                                    {
                                        int depthIndex = (depthY * depthWidth) + depthX;

                                        // If we are tracking a body for the current pixel, do not zero out the pixel
                                        if (bodyIndexDataPointer[depthIndex] != 0xff)
                                        {
                                            continue;
                                        }
                                    }
                                }

                                bitmapPixelsPointer[colorIndex] = 0;
                            }
                        }

                        this.bitmap.AddDirtyRect(new Int32Rect(0, 0, this.bitmap.PixelWidth, this.bitmap.PixelHeight));
                    }
                }
            }
            finally
            {
                if (isBitmapLocked)
                {
                    this.bitmap.Unlock();
                }

                if (depthFrame != null)
                {
                    depthFrame.Dispose();
                }

                if (colorFrame != null)
                {
                    colorFrame.Dispose();
                }

                if (bodyIndexFrame != null)
                {
                    bodyIndexFrame.Dispose();
                }
            }
        }

        /// <summary>
        /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.SensorNotAvailableStatusText;
        }
    }
}
