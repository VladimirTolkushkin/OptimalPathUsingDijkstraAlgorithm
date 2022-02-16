using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var result = new List<Point>() { };
            if (state.Goal > state.Chests.Count) return result;
            var start = state.Position;
            var goal = state.Goal;
            var targets = state.Chests;
            while (targets.Count != 0)
            {
                var dijkstra = new DijkstraPathFinder();
                var path = dijkstra.GetPathsByDijkstra(state, start, targets).FirstOrDefault();
                if (path == null || goal == 0) break;
                state.Energy -= path.Cost;
                if (state.Energy < 0) break;

                result.AddRange(path.Path.Skip(1));
                targets.Remove(path.End);
                start = path.End;
                goal--;
            }

            return result;
        }
    }
}