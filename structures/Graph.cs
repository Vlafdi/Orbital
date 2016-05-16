using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital.structures
{
    class Graph<T> where T : IGraphNode<T> {

        public HashSet<Vertex> Vertices { get; set; }
        
        public Graph() {
            Vertices = new HashSet<Vertex>();
        }

        public void addNode(Vertex node) {
            Vertices.Add(node);
        }

        public Vertex getVertex(string key) {
            foreach (Vertex vertex in Vertices) {
                if (vertex.Key == key)
                    return vertex;
            }
            throw new ArgumentException("Graph does not contain the given vertex!");
        }

        public class Vertex {
            public string Key { get; set; }
            public List<Edge> Adjacent { get; set; }

            public T Data { get; set; }

            public Vertex() {
                Adjacent = new List<Edge>();
            }

            public IEnumerable<Vertex> Neighbors() {
                var neighbors = new List<Vertex>();
                foreach (var edge in Adjacent) {
                    neighbors.Add(edge.Target);
                }
                return neighbors;
            }
            /// <summary>
            /// Helper method to create an edge between this vertex and the target
            /// </summary>
            /// <param name="vertex"></param>
            public void CreateEdge(Vertex vertex) {
                if (this.AreAdjacent(vertex))
                    throw new ArgumentException("An edge already exists between these vertices!");

                double distance = this.Data.GetDistance(vertex.Data);
                Adjacent.Add(new Edge(this, vertex) { Weight = distance });
            }

            /// <summary>
            /// Helper method to determine whether to vertices are adjacent
            /// </summary>
            /// <param name="vertex"></param>
            /// <returns></returns>
            public bool AreAdjacent(Vertex vertex) {
                return Adjacent.Any(edge => edge.Source.Equals(this) && edge.Target.Equals(vertex));
            }
        }

        public class Edge {
            public double Weight { get; set; }
            public Vertex Source { get; set; }
            public Vertex Target { get; set; }

            public Edge(Vertex src, Vertex trg) {
                Source = src;
                Target = trg;
            }
        }
    }
}
