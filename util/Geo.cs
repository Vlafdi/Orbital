using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Orbital.util {
    static class Geo {
        /// <summary>
        /// Radius of the Earth in km
        /// </summary>
        public static readonly int Radius = 6371;

        public static Vector3D LatLongToCoords(double latitude, double longitude, double altitude) {
            double LatRad = Math.PI * latitude / 180.0;
            double LongRad = Math.PI * longitude / 180.0;

            double r = Radius + altitude;
            double x = r * Math.Cos(LatRad) * Math.Cos(LongRad);
            double y = r * Math.Cos(LatRad) * Math.Sin(LongRad);
            double z = r * Math.Sin(LatRad);
            return new Vector3D(x, y, z);
        }
    }
}
