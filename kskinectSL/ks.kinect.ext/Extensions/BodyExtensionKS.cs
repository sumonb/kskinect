using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;


namespace ks.kinect.ext
{
    public static class BodyExtensionKS
    {
        public static IList<Body> TrackedBody(this IEnumerable<Body> bodies)
        {
            IList<Body> returnBodies = new List<Body>();
            if (bodies != null)
            {
                return bodies.Where(i => i.IsTracked == true).ToList();
            }
            return null;
        }

        public static int TrackedBodyCount(this IEnumerable<Body> bodies)
        {
            IList<Body> returnBodies = new List<Body>();
            if (bodies != null)
            {
                return bodies.Count(i => i.IsTracked == true);
            }
            return 0;
        }

    }
}
