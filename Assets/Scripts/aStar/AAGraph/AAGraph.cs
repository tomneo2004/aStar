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
		public virtual Path FindPath (Vector2 start, Vector2 end, float agentSize = 1.0f){
			//A* pathfinding
			bool pathFound = false;

			//the last node that is cloesest to end node
			AANode bestEndNode = null;

			AANode startNode = (AANode)FindNode(start);
			startNode.CameFrom = null;

			AANode endNode = (AANode)FindNode (end);
			endNode.CameFrom = null;

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

			//if start node == end node
			if (startNode.Id == endNode.Id) {
				bestEndNode = startNode;
				pathFound = true;
				return ConstructPath (bestEndNode, startNode, agentSize);
			}

			//Use binary heap for open list to improve search performance
			BinaryHeap<AANode> openNodes = new BinaryHeap<AANode>(delegate(AANode parent, AANode child) {
				return parent.F >= child.F;
			});
			List<AANode> closeNodes = new List<AANode> ();

			startNode.G = 0;
			startNode.H = Findheuristic (startNode, endNode);
			openNodes.Add (startNode);

			while (openNodes.Count > 0) {

				AANode currentNode = openNodes.Priority;

				//current node is goal
				if (currentNode.Id == endNode.Id) {

					bestEndNode = currentNode;
					pathFound = true;
					break;
				}

				if (bestEndNode == null) {
				
					bestEndNode = currentNode;

				} else {

					if (Vector2.Distance(currentNode.Center, endNode.Center) 
						<= Vector2.Distance(bestEndNode.Center, endNode.Center))
						bestEndNode = currentNode;
				}

				openNodes.Remove ();
				closeNodes.Add (currentNode);

				List<Connection> conns = currentNode.AllConnections;
				for (int i = 0; i < conns.Count; i++) {

					AANode neighbourNode = (AANode)conns [i].To;

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

					if (neighbourNode.Clearance < agentSize)
						continue;

					bool updateNieghbour = false;
					bool addNeighbour = false;

					//find g score for this neighbour
					float gScore = currentNode.G + conns [i].cost;

					//add neighbour node to open node if it is not
					//exist in open node and update neighbour node
					if (!openNodes.Contain (neighbourNode, delegate(AANode first, AANode second) {
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
			return ConstructPath(bestEndNode, startNode, agentSize);
		}

		protected Path ConstructPath (AANode node, AANode startNode, float agentSize)
		{
			//construct path
			AANode currentNode = node;
			Path currentPath = null;

			Vector2 graphTopLeft = new Vector2 (_center.x - _horizontalNodes * _nodeSize / 2.0f,
				_center.y + _verticalNodes * _nodeSize / 2.0f);

			do {

				int numNode = 1;

				if(agentSize >= _nodeSize)
					numNode = Mathf.CeilToInt(agentSize / _nodeSize);

				Vector2 pathPos = new Vector2(graphTopLeft.x + currentNode.Column * _nodeSize + _nodeSize / 2.0f, 
					graphTopLeft.y - currentNode.Row * _nodeSize - _nodeSize / 2.0f);

				if(currentNode.Id != startNode.Id){

					//find center point of path base on agent size
					pathPos = new Vector2(graphTopLeft.x + currentNode.Column * _nodeSize + numNode * _nodeSize / 2.0f, 
						graphTopLeft.y - currentNode.Row * _nodeSize -  numNode * _nodeSize / 2.0f);
				}

				Path newPath = new Path(pathPos, currentNode);

				if(currentPath != null){
					currentPath.PreviousPath = newPath;
					newPath.NextPath = currentPath;
				}

				currentPath = newPath;

				AANode parent = (AANode)currentNode.CameFrom;

				//make sure parent node is clean up
				currentNode.CameFrom = null;

				currentNode = parent;

			} while(currentNode != null);

			return currentPath;
		}

		public override void GenerateGraph ()
		{
			if (_nodes != null)
				_nodes.Clear ();

			for (int row = 0; row < _verticalNodes; row++) {

				for (int col = 0; col < _horizontalNodes; col++) {

					//create new grid node
					AANode n = new AANode (this, row, col);
					n.DrawColor = _drawColor;
					n.DrawPriority = _drawPriority;

					//add new grid node to grid graph
					AddNode (n);

					//TODO configure new grid node if it is walkable or not
					Vector2 gridOffset = new Vector2 (_center.x - _horizontalNodes * _nodeSize / 2.0f, 
						_center.y + _verticalNodes * _nodeSize / 2.0f);
					Vector2 nodeCenter = new Vector2 (col * _nodeSize + _nodeSize / 2.0f + gridOffset.x,
						-(row * _nodeSize + _nodeSize / 2.0f) + gridOffset.y);

					Collider2D[] results = Physics2D.OverlapBoxAll(nodeCenter, new Vector2(_nodeSize, _nodeSize),
						0.0f, _collisionLayerMask);
					if (results.Length > 0) {

						n.Walkable = false;
						n.DrawColor = Color.red;
						n.DrawPriority = 0;

					} else {

						n.Walkable = true;
					}

					//connect this grid node to neighbour nodes
					AANode[] neighbours = new AANode[8];
					neighbours[0] = (AANode)FindNode(row, col - 1);//Left
					neighbours[1] = (AANode)FindNode (row - 1, col);//Top
					neighbours[2] = (AANode)FindNode (row, col + 1);//Right
					neighbours[3] = (AANode)FindNode (row + 1, col);//Bottom
					neighbours[4] = (AANode)FindNode (row - 1, col - 1);//TopLeft
					neighbours[5] = (AANode)FindNode (row - 1, col + 1);//TopRight
					neighbours[6] = (AANode)FindNode (row + 1, col + 1);//BottomRight
					neighbours[7] = (AANode)FindNode (row + 1, col - 1);//BottomLeft

					for (int i = 0; i < neighbours.Length; i++) {

						AANode neighbourNode = neighbours [i];

						if (neighbourNode != null) {

							//add connection both way
							n.AddConnection (neighbourNode);
							neighbourNode.AddConnection (n);
						}
					}
				}
			}
		}
	}
}

