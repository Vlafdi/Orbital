using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Orbital.structures {
    class SatelliteGraphFactory {
        public static Graph<Satellite> GetSatelliteGraph(String[][] inputData) {
            Graph<Satellite> graph = new Graph<Satellite>();
            for (int i = 1; i < inputData.Length - 1; i++) {
                string satName = inputData[i][0];
                Satellite satellite = new Satellite() {
                    Name = satName,
                    Latitude = Convert.ToDouble(inputData[i][1], CultureInfo.InvariantCulture),
                    Longitude = Convert.ToDouble(inputData[i][2], CultureInfo.InvariantCulture),
                    Altitude = Convert.ToDouble(inputData[i][3], CultureInfo.InvariantCulture)
                };
                var vertex = new Graph<Satellite>.Vertex() { Key = satName, Data = satellite };
                graph.addNode(vertex);
            }
            
            foreach(Graph<Satellite>.Vertex a in graph.Vertices) {
                foreach (Graph<Satellite>.Vertex b in graph.Vertices) {
                    if (a.Key != b.Key) {
                        if (a.Data.HasLineOfSight(b.Data) && !a.AreAdjacent(b)) {
                            a.CreateEdge(b);
                            b.CreateEdge(a);
                        }
                    }
                }
            }

            return graph;
        }

        /// <summary>
        /// Gets a closest visible satellite from the given coordinate in the given satelitte graph
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static Satellite GetClosestVisibleSatellite(Vector3D coord, Graph<Satellite> g) {
            Satellite closest = null;
            double minDistance = Double.MaxValue;
            foreach (Graph<Satellite>.Vertex vertex in g.Vertices) {
                Satellite sat = vertex.Data;
                double distance = sat.GetDistance(coord);
                if (sat.HasLineOfSight(coord) && distance < minDistance) {
                    closest = sat;
                    minDistance = distance;
                }
            }
            if (closest == null)
                throw new ArgumentException("No visible satellites found!");

            return closest;
        }

        public static Graph<Satellite>.Vertex GetVertexBySatellite(Satellite sat, Graph<Satellite> g) {
            foreach (Graph<Satellite>.Vertex vertex in g.Vertices) {
                if (vertex.Key == sat.Name)
                    return vertex;
            }
            throw new ArgumentException("Graph does not contain the given satellite!");
        }
    }
}
