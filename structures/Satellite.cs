using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Orbital.util;

namespace Orbital.structures {

    class Satellite : IGraphNode<Satellite> {

        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        //public bool HasLineOfSight(Vector3D coord) {
        //    Vector3D x0 = new Vector3D(0.0f, 0.0f, 0.0f);
        //    Vector3D x1 = this.GetCoord();
        //    Vector3D x2 = coord;

        //    double denominator = Vector3D.CrossProduct(Vector3D.Subtract(x0, x1), Vector3D.Subtract(x0, x2)).Length;
        //    double numerator = Vector3D.Subtract(x2, x1).Length;
        //    double distance = denominator / numerator;
        //    return distance >= Geo.Radius;
        //}

        /// <summary>
        /// To determine whether there is line of sight between satellites, find out
        /// if the the minimum distance between the line defined by the satellite coordinates
        /// and the center of the Earth is shorter than the radius.
        /// </summary>
        /// <param name="sat">Satellite to measure distance with</param>
        /// <returns></returns>
        public bool HasLineOfSight(Vector3D coord) {
            double distance = GetDistanceFromOrigo(coord);
            return distance >= Geo.Radius;
        }

        public bool HasLineOfSight(Satellite sat) {
            return HasLineOfSight(sat.GetCoord());
        }

        /// <summary>
        /// Attempt to get a minimum distance between origo and a line segment defined by satellite coordinates
        /// ant the coordinate given as a parameter
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public double GetDistanceFromOrigo(Vector3D coord) {
            Vector3D x0 = new Vector3D(0.0f, 0.0f, 0.0f);
            Vector3D x1 = this.GetCoord();
            Vector3D x2 = coord;

            var x1x2 = Vector3D.Subtract(x2, x1);
            var x1x0 = Vector3D.Subtract(x0, x1);

            double distance = 0.0;

            if (Vector3D.DotProduct(x1x0, x1x2) <= 0.0) {
                distance = x1x0.Length;
            }
            else {
                Vector3D x2x0 = Vector3D.Subtract(x0, x2);
                if (Vector3D.DotProduct(x2x0, x1x2) >= 0.0) {
                    distance = x2x0.Length;
                }
                else {
                    distance = Vector3D.CrossProduct(x1x2, x1x0).Length / x1x2.Length;
                }
            }
            return distance;
        }
        /// <summary>
        /// Euclidean distance between satellites by cartesian coordinates
        /// </summary>
        /// <param name="sat"></param>
        /// <returns></returns>
        public double GetDistance(Satellite sat) {
            return GetDistance(sat.GetCoord());
        }   

        /// <summary>
        /// Euclidean distance between the satellite and a coordinate
        /// </summary>
        /// <param name="sat"></param>
        /// <returns></returns>
        public double GetDistance(Vector3D coord) {

            Vector3D a = this.GetCoord();
            Vector3D b = coord;
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
        }


        /// <summary>
        /// Convert lat & long to radians and get a corresponding cartesian coordinate position 
        /// </summary>
        /// <returns></returns>
        public Vector3D GetCoord() {
            return Geo.LatLongToCoords(Latitude, Longitude, Altitude);
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;
            Satellite satObj = obj as Satellite;
            if ((Object)satObj == null)
                return false;
            return Name == satObj.Name;
        }

        public override int GetHashCode() {
            return Name.GetHashCode();
        }
    }
}
