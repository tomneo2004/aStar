using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;
using NP.DataStructure;

namespace NP.aStarPathfinding{

	public class AAGraph : GridGraph{

		public AAGraph(Vector2 center) : base(center){
		}
	
		/**
		 * Find path with clearance
		 **/
		public virtual Path FindPath (Vector2 start, Vector2 end, int agentSize = 1){
			//A* pathfinding
			GridNode startNode = FindNode(start);
			startNode.CameFrom = null;
			GridNode endNode = FindNode (end);
			GridNode foundNode = null;

			if (startNode == null) {

				#if DEBUG
				Debug.LogError("Start position "+start+" is not in grid");
				#endif
				return null;
			}

			if (endNode == null) {

				#if DEBUG
				Debug.LogError("End position "+end+" is not in grid");
				#endif
				return null;
			}

			//Use binary heap for open list to improve search performance
			BinaryHeap<GridNode> openNodes = new BinaryHeap<GridNode>(delegate(GridNode parent, GridNode child) {
				return parent.F >= child.F;
			});
			List<GridNode> closeNodes = new List<GridNode> ();

			startNode.G = 0;
			startNode.H = Findheuristic (startNode, endNode);
			openNodes.Add (startNode);

			while (openNodes.Count > 0) {

				GridNode currentNode = openNodes.Priority;

				//current node is goal
				if (currentNode.Id == endNode.Id) {

					foundNode = currentNode;
					break;
				}

				openNodes.Remove ();
				closeNodes.Add (currentNode);

				List<Connection> conns = currentNode.AllConnections;
				for (int i = 0; i < conns.Count; i++) {

					GridNode neighbourNode = (GridNode)conns [i].To;

					//if neighbour node is in close node then ignore and continue
					//next neighbour
					bool closeContain = false;
					for (int n = 0; n < closeNodes.Count; n++) {

						if (closeNodes [n].Id == neighbourNode.Id) {
							closeContain = true;
							break;
						}
					}

					if (!neighbourNode.Walkable)
						continue;
					if (closeContain)
						continue;

					int clearance = FindClearance (neighbourNode);

					if (agentSize > clearance)
						continue;

					bool updateNieghbour = false;
					bool addNeighbour = false;

					//find g score for this neighbour
					float gScore = currentNode.G + conns [i].cost;

					//add neighbour node to open node if it is not
					//exist in open node and update neighbour node
					if (!openNodes.Contain (neighbourNode, delegate(GridNode first, GridNode second) {
						return (first.Id == second.Id);
					})) {

						addNeighbour = true;
						updateNieghbour = true;
					}

					//neighbour is alrady in open list
					//new path is better and update neighbour node
					if (gScore < neighbourNode.G)
						updateNieghbour = true;

					//check if neighbour need to be updated
					if (updateNieghbour) {

						neighbourNode.G = gScore;
						neighbourNode.H = Findheuristic (neighbourNode, endNode);
						neighbourNode.CameFrom = currentNode;
					}

					//check if neighbour need to be added to open list
					if (addNeighbour)
						openNodes.Add (neighbourNode);
				}
			}

			//construct path
			if (foundNode == null)
				return null;
			return ConstructPath(foundNode);
		}

		public int FindClearance(GridNode node){

			int clearance = 1;
			int searchRow = node.Row + 1;
			int searchCol = node.Column + 1;

			while (searchRow < _verticalNodes && searchCol < _horizontalNodes) {

				if (!FindNode (searchRow, searchCol).Walkable)
					break;

				bool block = false;

				for (int v = searchRow; v >= node.Row; v--) {

					if (!FindNode (v, searchCol).Walkable) {
						block = true;
						break;
					}
				}

				if (block)
					break;

				for (int h = searchCol; h >= node.Column; h--) {

					if (!FindNode (searchRow, h).Walkable) {
						block = true;
						break;
					}
				}

				if (block)
					break;

				clearance++;
				searchRow++;
				searchCol++;
			}

			return clearance;

		}
	}
}

