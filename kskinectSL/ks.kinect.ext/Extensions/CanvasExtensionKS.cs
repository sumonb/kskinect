using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LightBuzz.Vitruvius;
using Microsoft.Kinect;

namespace ks.kinect.ext
{
    public static class CanvasExtensionKs
    {
        public static void MarkBodyJointKS(this Canvas canvas, Visualization visualizationMode, IEnumerable<Body> bodies, SolidColorBrush jointBrushColor)
        {
            foreach (var currentBody in bodies)
            {
                if (currentBody.IsTracked == false) continue;

                foreach (var currentJoint in currentBody.TrackedJoints())
                {
                    var currentPosition = currentJoint.Position.ToPoint(visualizationMode);

                    // Draw
                    var ellipse = new Ellipse
                    {
                        Fill = jointBrushColor,
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
}
