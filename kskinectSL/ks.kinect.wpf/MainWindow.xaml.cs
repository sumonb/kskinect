using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Kinect;
//using System.Windows.Media.Imaging;
using ks.kinect.ext;
using LightBuzz.Vitruvius;

namespace ks.kinect.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly KinectSensor _sensor;
        IList<Body> _bodies;

        // Reads Color frame data
        private readonly ColorFrameReader _colorReader = null;
        private readonly InfraredFrameReader _infraredReader = null;
        private readonly DepthFrameReader _depthReader = null;
        private readonly BodyFrameReader _bodyReader = null;


        private Visualization _mode = Visualization.Color;

        public MainWindow()
        {
            InitializeComponent();

            _sensor = KinectSensor.GetDefault();
            if (_sensor == null) return;
            _sensor.Open();

            _colorReader = _sensor.ColorFrameSource.OpenReader();
            _colorReader.FrameArrived += ColorReader_FrameArrived;

            _infraredReader = _sensor.InfraredFrameSource.OpenReader();
            _infraredReader.FrameArrived += InfraredReader_FrameArrived;

            _depthReader = _sensor.DepthFrameSource.OpenReader();
            _depthReader.FrameArrived += DepthReader_FrameArrived;

            _bodyReader = _sensor.BodyFrameSource.OpenReader();
            _bodyReader.FrameArrived += BodyReader_FrameArrived;

            
        }

        void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame == null) return;
                canvas.Children.Clear();

                var bodies = frame.Bodies();
                var bodyCount = bodies.TrackedBodyCountKS();
                if (bodyCount > 0)
                {
                    lbl.Content = "who is here";
                }
                else
                {
                    lbl.Content = "";
                }
                foreach (var currentBody in bodies)
                {
                    if (currentBody.IsTracked == false) continue;

                        foreach (var currentJoint in currentBody.TrackedJoints())
                        {
                            var currentPosition = currentJoint.Position.ToPoint(_mode);

                            // Draw
                            Ellipse ellipse = new Ellipse
                            {
                                Fill = Brushes.Red,
                                Width = 30,
                                Height = 30
                            };
                            Canvas.SetLeft(ellipse, currentPosition.X - ellipse.Width / 2);
                            Canvas.SetTop(ellipse, currentPosition.Y - ellipse.Height / 2);

                            canvas.Children.Add(ellipse);
                        }
                }
            }
        }

        void DepthReader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            if (_mode == Visualization.Depth)
            {
                using (var frame = e.FrameReference.AcquireFrame())
                {
                    if (frame == null) return;
                    camera.Source = frame.ToBitmap();
                }
            }
        }



        void ColorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (_mode == Visualization.Color)
            {
                using (var frame = e.FrameReference.AcquireFrame())
                {
                    if (frame == null) return;
                    camera.Source = frame.ToBitmap();
                }
            }

        }

        void InfraredReader_FrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            if (_mode == Visualization.Infrared)
            {
                using (var frame = e.FrameReference.AcquireFrame())
                {
                    if (frame == null) return;
                    camera.Source = frame.ToBitmap();
                }
            }
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            if (_colorReader != null)
            {
                _colorReader.Dispose();
            }

            if (_infraredReader != null)
            {
                _infraredReader.Dispose();
            }

            if (_depthReader != null)
            {
                _depthReader.Dispose();
            }

            if (_bodyReader != null)
            {
                _bodyReader.Dispose();

            }
            if (_sensor != null)
            {
                _sensor.Close();
            }
        }
    }
}
