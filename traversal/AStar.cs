using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbital.structures;
using Priority_Queue;

namespace Orbital.traversal {
    class AStar<T> where T : IGraphNode<T> {
        public Graph<T> Graph { get; set; }
        public Graph<T>.Vertex Start { get; set; }
        public Graph<T>.Vertex End { get; set; }

        public HashSet<Graph<T>.Vertex> Closed { get; set; }
        public SimplePriorityQueue<Graph<T>.Vertex> Open { get; set;}

        public Dictionary<string, string> CameFrom { get; set; }
        public Dictionary<string, double> GScore { get; set; }


        public AStar(Graph<T>.Vertex start, Graph<T>.Vertex end, Graph<T> graph) {
            Graph = graph;
            Start = start;
            End = end;
            Closed = new HashSet<Graph<T>.Vertex>();
            Open = new SimplePriorityQueue<Graph<T>.Vertex>();
            Open.Enqueue(Start, HeuristicEstimate(Start, End));

            CameFrom = new Dictionary<string, string>();
            GScore = new Dictionary<string, double>();
        }

        public LinkedList<string> Search() {
            foreach (var vertex in Graph.Vertices) {
                GScore[vertex.Key] = Double.MaxValue;
            }
            GScore[Start.Key] = 0.0;

            while (Open.Any()) {
                var current = Open.Dequeue();
                if (current.Equals(End))
                    return ReconstructPath(CameFrom, current);
                Closed.Add(current);
                foreach (var neighborEdge in current.Adjacent.Where(edge => !Closed.Contains(edge.Target))) {
                    var neighbor = neighborEdge.Target;
                    double tentativeGScore = GScore[current.Key] + neighborEdge.Weight;
                    if (!Open.Contains(neighbor))
                        Open.Enqueue(neighbor, Double.MaxValue);
                    else if (tentativeGScore >= GScore[neighbor.Key])
                        continue;

                    CameFrom[neighbor.Key] = current.Key;
                    GScore[neighbor.Key] = tentativeGScore;
                    var newFScore = GScore[neighbor.Key] + HeuristicEstimate(neighbor, End);
                    Open.UpdatePriority(neighbor, newFScore);
                }
            }
            // Empty return list marks failure
            return new LinkedList<string>();
        }

        private LinkedList<string> ReconstructPath(Dictionary<string, string> cameFrom, Graph<T>.Vertex current) {
            LinkedList<String> path = new LinkedList<String>();
            path.AddFirst(current.Key);
            var cur = current.Key;
            while (CameFrom.Keys.Contains(cur)) {
                cur = CameFrom[cur];
                path.AddFirst(cur);
            }
            return path;
        }

        public double HeuristicEstimate(Graph<T>.Vertex start, Graph<T>.Vertex end) {
            return start.Data.GetDistance(end.Data);
        }
    }
}
