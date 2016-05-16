using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace ks.kinect.ext
{
    public class BodyVisualManager
    {
        #region Constants

        /// <summary>
        /// The default drawing brush.
        /// </summary>
        static readonly Brush DEFAULT_BRUSH = new SolidColorBrush(Colors.LightCyan);

        /// <summary>
        /// The default circle size.
        /// </summary>
        static readonly double DEFAULT_RADIUS = 15;

        /// <summary>
        /// The default line thickness.
        /// </summary>
        static readonly double DEFAULT_THICKNESS = 8;

        #endregion

        public List<BodyVisualInfo> VisualBodies { get; set; }
        public BodyVisualManager()
        {
            VisualBodies = new List<BodyVisualInfo>();
        }

        public void AddBody(Body body, double jointRadius, Brush jointBrush, double boneThickness, Brush boneBrush)
        {
            if(body == null || !body.IsTracked) return;

            var isCurrentBodyExists = VisualBodies.FirstOrDefault(i => i.TrackingId == body.TrackingId);
            if(isCurrentBodyExists != null) return; //already exists - so return

            //add new body
            var visual = BodyVisualInfo.AddBody(body.TrackingId, body.Joints.Keys, jointRadius, jointBrush, boneThickness, boneBrush);

            //foreach (var ellipse in visual.Joints.Values)
            //{
            //    canvas.Children.Add(ellipse);
            //}

            //foreach (var line in visual.Bones.Values)
            //{
            //    canvas.Children.Add(line);
            //}

            //_bodyVisuals.Add(visual);

        }
    }

    public class BodyVisualInfo
    {
        #region Properties

        /// <summary>
        /// The tracking ID of the corresponding body.
        /// </summary>
        public ulong TrackingId { get; set; }

        /// <summary>
        /// The joints of the body and their corresponding ellipses.
        /// </summary>
        public Dictionary<JointType, Ellipse> Joints { get; set; }

        /// <summary>
        /// The bones of the body and their corresponding lines.
        /// </summary>
        public Dictionary<Tuple<JointType, JointType>, Line> Bones { get; set; }

        #endregion

        #region Constructor
        public BodyVisualInfo()
        {
            Joints = new Dictionary<JointType, Ellipse>();
            Bones = new Dictionary<Tuple<JointType, JointType>, Line>();
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Cleares the joints and bones.
        /// </summary>
        public void Clear()
        {
            Joints.Clear();
            Bones.Clear();
        }

        /// <summary>
        /// Adds the specified joint to the collection.
        /// </summary>
        /// <param name="joint">The joint type.</param>
        /// <param name="radius">The size of the ellipse</param>
        /// <param name="brush">The brush used to fill the ellipse.</param>
        public void AddJoint(JointType joint, double radius, Brush brush)
        {
            Joints.Add(joint, new Ellipse
            {
                Width = radius,
                Height = radius,
                Fill = brush
            });
        }

        /// <summary>
        /// Adds a bone to the collection.
        /// </summary>
        /// <param name="joints">The start and end of the line segment.</param>
        /// <param name="thickness">The thickness of the line.</param>
        /// <param name="brush">The brush used to fill the line.</param>
        public void AddBone(Tuple<JointType, JointType> joints, double thickness, Brush brush)
        {
            Bones.Add(joints, new Line
            {
                StrokeThickness = thickness,
                Stroke = brush
            });
        }



        /// <summary>
        /// Creates a new BodyVisual object with the specified parameters.
        /// </summary>
        /// <param name="trackingId">The tracking ID of the corresponding body.</param>
        /// <param name="joints">The joint types of the body.</param>
        /// <param name="jointRadius">The desired joint size.</param>
        /// <param name="jointBrush">The desired joint brush.</param>
        /// <param name="boneThickness">The desired line thickness.</param>
        /// <param name="boneBrush">The desired line brush.</param>
        /// <returns>A new instance of BodyVisual.</returns>
        public static BodyVisualInfo AddBody(ulong trackingId, IEnumerable<JointType> joints, double jointRadius, Brush jointBrush, double boneThickness, Brush boneBrush)
        {
            var manager = new BodyVisualInfo
            {
                TrackingId = trackingId
            };

            foreach (var joint in joints)
            {
                manager.AddJoint(joint, jointRadius, jointBrush);
            }

            foreach (var bone in Constants.Body_Connections_Points)
            {
                manager.AddBone(bone, boneThickness, boneBrush);
            }

            return manager;
        }


        #endregion

    }
}
