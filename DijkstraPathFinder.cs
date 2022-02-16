using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{
	public class DijkstraData
	{
		public Point Previous { get; set; }
		public int Price { get; set; }

		public DijkstraData(Point previous, int price)
		{
			Previous = previous;
			Price = price;
		}
	}

	public class DijkstraPathFinder
	{
		public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
		{
			var unreachable–oint = new Point(-1, -1);
			var targetsList = targets.ToList();
			var toVisit = new HashSet<Point> { start };
			var visisted = new HashSet<Point>();
			var track = new Dictionary<Point, DijkstraData> { [start] = new DijkstraData(unreachable–oint, 0) };

			while (true)
			{
				Point toOpen = GetPointToOpen(unreachable–oint, toVisit, track);

				if (targetsList.Count == 0 || toVisit.Count == 0) break;
				if (toOpen == unreachable–oint) yield return null;
				if (targetsList.Contains(toOpen))
				{
					var path = GetPath(unreachable–oint, track, toOpen);
					yield return new PathWithCost(track[toOpen].Price, path.ToArray());
					targetsList.Remove(toOpen);
				};

				var toMove = GetPointsToMove(toOpen);

				FillPath(state, toVisit, visisted, track, toOpen, toMove);
			}
		}

		private static void FillPath(State state, HashSet<Point> toVisit, HashSet<Point> visisted,
			Dictionary<Point, DijkstraData> track, Point toOpen, Point[] toMove)
		{
			foreach (var p in toMove)
			{
				if (state.InsideMap(p) && !state.IsWallAt(p))
				{
					if (!visisted.Contains(p)) toVisit.Add(p);
					var currentPrice = track[toOpen].Price + state.CellCost[p.X, p.Y];
					if (!track.ContainsKey(p) || track[p].Price > currentPrice)
					{
						track[p] = new DijkstraData(toOpen, currentPrice);
					}
				}
			}
			toVisit.Remove(toOpen);
			visisted.Add(toOpen);
		}

		private static Point GetPointToOpen(Point unreachable–oint, HashSet<Point> toVisit,
			Dictionary<Point, DijkstraData> track)
		{
			var toOpen = unreachable–oint;
			var bestPrice = int.MaxValue;
			foreach (var p in toVisit)
			{
				if (track.ContainsKey(p) && track[p].Price < bestPrice)
				{
					bestPrice = track[p].Price;
					toOpen = p;
				}
			}

			return toOpen;
		}

		private static List<Point> GetPath(Point unreachablePoint, Dictionary<Point, DijkstraData> track, Point toOpen)
		{
			var result = new List<Point>();
			var end = toOpen;
			while (end != unreachablePoint)
			{
				result.Add(end);
				end = track[end].Previous;
			}
			result.Reverse();
			return result;
		}

		private static Point[] GetPointsToMove(Point toOpen)
		{
			var toMove = new Point[4];
			toMove[0] = new Point(toOpen.X - 1, toOpen.Y);
			toMove[1] = new Point(toOpen.X + 1, toOpen.Y);
			toMove[2] = new Point(toOpen.X, toOpen.Y + 1);
			toMove[3] = new Point(toOpen.X, toOpen.Y - 1);
			return toMove;
		}
	}
}