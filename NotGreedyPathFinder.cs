using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
	public class PathData
	{
		public State State { get; }
		public Point Start { get; }
		public int Energy { get; }
		public int Goal { get; }
		public List<Point> Targets { get; }
		public List<Point> Path { get; }
		public HashSet<Point> Visited { get; }

		public PathData(State state, Point start, int energy, int goal, List<Point> targets, List<Point> path, HashSet<Point> visited)
		{
			State = state;
			Start = start;
			Energy = energy;
			Goal = goal;
			Targets = targets;
			Path = path;
			Visited = visited;
		}
	}
	public class NotGreedyPathFinder : IPathFinder
	{
		private List<Point> bestPath;
		private int bestGoal;
		private DijkstraPathFinder dijkstra = new DijkstraPathFinder();

		public List<Point> FindPathToCompleteGoal(State state)
		{
			bestPath = new List<Point>();
			bestGoal = 0;
			var dictionary = FillAllPaths(state);

			var stack = new Stack<PathData> ();

			stack.Push(new PathData(state, state.Position, state.Energy, 0, state.Chests.ToList(), new List<Point>(), new HashSet<Point>()));
			while (stack.Count != 0)
			{
				var step = stack.Pop();
				if (step.Goal > bestGoal)
				{
					bestGoal = step.Goal;
					bestPath = step.Path;
				}
				if (step.Goal == state.Chests.Count) break;

				foreach (var way in dictionary[step.Start].Where(p => !step.Visited.Contains(p.End)))
				{
					var e = step.Energy - way.Cost;
					if (e >= 0)
					{
						step.Targets.Remove(way.End);

						var p = step.Path.ToList();
						p.AddRange(way.Path.Skip(1));

						var v = step.Visited.ToHashSet<Point>();
						v.Add(way.End);

						stack.Push(new PathData(state, way.End, e, step.Goal + 1, step.Targets, p, v));
					}
				}
			}



			//FindOptimalPathRecursive(state, state.Position, state.Energy, 0, state.Chests.ToList(),
			//	new List<Point>(), dictionary, new HashSet<Point>());
			return bestPath;
		}

		private Dictionary<Point, List<PathWithCost>> FillAllPaths(State state)
		{
			var dictionary = new Dictionary<Point, List<PathWithCost>>();
			dictionary[state.Position] = dijkstra.GetPathsByDijkstra(state, state.Position, state.Chests).ToList();
			foreach (var t in state.Chests)
				dictionary[t] = dijkstra.GetPathsByDijkstra(state, t, state.Chests).ToList();
			return dictionary;
		}

		//public void FindOptimalPathRecursive(State state, Point start, int energy, int goal, List<Point> targets,
		//	List<Point> path, Dictionary<Point, List<PathWithCost>> dictionary, HashSet<Point> visited)
		//{
		//	if (goal > bestGoal)
		//	{
		//		bestGoal = goal;
		//		bestPath = path;
		//	}
		//	if (goal == state.Chests.Count) return;

		//	foreach (var way in dictionary[start].Where(p => !visited.Contains(p.End)))
		//	{
		//		var e = energy- way.Cost;
		//		if (e >= 0)
		//		{
		//			targets.Remove(way.End);

		//			var p = path.ToList();
		//			p.AddRange(way.Path.Skip(1));

		//			var v = visited.ToHashSet<Point>();
		//			v.Add(way.End);

		//			FindOptimalPathRecursive(state, way.End, e, goal + 1, targets, p, dictionary, v);
		//		}
		//	}
		//}
	}
}