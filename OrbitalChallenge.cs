using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Media.Media3D;
using Orbital.structures;
using Orbital.util;
using Orbital.traversal;

namespace Orbital
{
    class OrbitalChallenge
    {
        static void Main(string[] args)
        {
            string[][] fileData = IO.FileToArray("../../data/sample.txt");
            for (int i = 0; i < fileData.Length; i++) {
                Console.WriteLine(String.Join(", ", fileData[i]));
            }

            // Build a Graph where vertices contain satellites and are connected by edges if they are visible
            // from another, with distances as weight edges
            Graph<Satellite> satelliteGraph = SatelliteGraphFactory.GetSatelliteGraph(fileData);

            var startLat = Convert.ToDouble(fileData[fileData.Length-1][1], CultureInfo.InvariantCulture);
            var startLon = Convert.ToDouble(fileData[fileData.Length-1][2], CultureInfo.InvariantCulture);

            var targLat = Convert.ToDouble(fileData[fileData.Length-1][3], CultureInfo.InvariantCulture);
            var targLon = Convert.ToDouble(fileData[fileData.Length-1][4], CultureInfo.InvariantCulture);

            Vector3D startCoord = Geo.LatLongToCoords(startLat, startLon, 0.0f);
            Vector3D targCoord = Geo.LatLongToCoords(targLat, targLon, 0.0f);

            Console.WriteLine("Trying to find closest satellites...");
            Satellite start = SatelliteGraphFactory.GetClosestVisibleSatellite(startCoord, satelliteGraph);
            Satellite target = SatelliteGraphFactory.GetClosestVisibleSatellite(targCoord, satelliteGraph);

            var startVertex = satelliteGraph.getVertex(start.Name);
            var targVertex = satelliteGraph.getVertex(target.Name);

            var search = new AStar<Satellite>(startVertex, targVertex, satelliteGraph);
            Console.WriteLine(String.Format("Searching for path between satellites [{0}, {1}]", start.Name, target.Name));

            IEnumerable<string> path = search.Search();

            if (!path.Any())
                Console.WriteLine("No viable solution found!");
            else
                Console.WriteLine(String.Format("Optimal path is: [ {0} ]", String.Join(",", path)));

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
