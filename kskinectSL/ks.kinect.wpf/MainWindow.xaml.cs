﻿using System;
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
using System.Windows.Media.Imaging;
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


        private Constants.CameraMode _mode = Constants.CameraMode.Color;

        public MainWindow()
        {
            InitializeComponent();

            _sensor = KinectSensor.GetDefault();
            if (_sensor == null) return;


            _colorReader.ColorFrameSource.OpenReader();
            _colorReader.FrameArrived += ColorReader_FrameArrived;

            _infraredReader.InfraredFrameSource.OpenReader();
            _infraredReader.FrameArrived += InfraredReader_FrameArrived;

            _depthReader.DepthFrameSource.OpenReader();
            _depthReader.FrameArrived += DepthReader_FrameArrived;
        }

        void DepthReader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            if (_mode == Constants.CameraMode.Depth)
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
            if (_mode == Constants.CameraMode.Color)
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
            if (_mode == Constants.CameraMode.Infrared)
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

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }
    }
}
