using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;
using NP.DataStructure;

namespace NP.aStarPathfinding{
	
	public class GridGraph : Graph {

		/**
	 * Number of node in horizontal
	 **/
		protected int _horizontalNodes = 0;

		/**
	 * Get number of node in horizontal
	 * 
	 * Set number of node in horizontal
	 **/
		public int HorizontalNode{

			get{

				return _horizontalNodes;
			}

			set{

				_horizontalNodes = value;
			}
		}

		/**
	 * Number of node in vertical
	 **/
		protected int _verticalNodes = 0;

		/**
	 * Get number of node in vertical
	 * 
	 * Set number of node in vertical
	 **/
		public int VerticalNodes{

			get{ 

				return _verticalNodes;
			}

			set{

				_verticalNodes = value;
			}
		}

		/**
	 * Size of node in graph
	 **/
		protected float _nodeSize = 5;

		/**
	 * Get size of node
	 * 
	 * Set size of node
	 **/
		public float NodeSize{

			get{ 

				return _nodeSize;
			}

			set{

				_nodeSize = value;
			}
		}

		/**
		 * Grid graph center
		 **/
		protected Vector2 _center;

		/**
		 * Get center of grid graph
		 * 
		 * Set center of grid graph
		 **/
		public Vector2 Center{

			get{ 

				return _center;
			}

			set{ 
			
				_center = value;
			}
		}

		/**
		 * The LayerMask used to check if obstacle in grid node
		 **/
		protected int _collisionLayerMask;

		/**
		 * The LayerMask used to check if obstacle in grid node
		 * 
		 * Get LayerMask
		 * 
		 * Set LayerMask
		 **/
		public int collisionLayerMask{
		
			get{ return _collisionLayerMask;}

			set{ 
			
				_collisionLayerMask = value;
			}
		}

		public GridGraph (Vector2 center) : base (){

			_center = center;
		}

		/**
	 * Find node by given position in world space
	 **/
		public GridNode FindNode(Vector2 position){

			//Make sure position within grid boundary
			float gridHalfWidth = _horizontalNodes * _nodeSize / 2.0f;
			float gridHalfHeight = _verticalNodes * _nodeSize / 2.0f;
			if (position.x < _center.x - gridHalfWidth || position.x > _center.x + gridHalfWidth
			    || position.y > _center.y + gridHalfHeight || position.y < _center.y - gridHalfHeight) {

				#if DEBUG
				Debug.LogWarning("Given position "+position+" is not within grid boundary");
				#endif
				return null;
			}

			//Convert position in world space into row and column
			Vector2 convertPos = position - _center;//use (0,0) as origin position

			int row = Mathf.FloorToInt ((gridHalfHeight - convertPos.y)/_nodeSize);
			row = row >= _verticalNodes ? _verticalNodes - 1 : row;

			int column = Mathf.FloorToInt ((gridHalfWidth + convertPos.x)/_nodeSize);
			column = column >= _horizontalNodes ? _horizontalNodes - 1 : column;

			return FindNode(row, column);
		}

		/**
	 * Find node by given row and column 
	 **/
		public GridNode FindNode(int row, int col){

			if (row < 0 || row >= _verticalNodes)
				return null;

			if (col < 0 || col >= _horizontalNodes)
				return null;

			if (_nodes.Count > 0) {

				IEnumerator ie = AllNodes.GetEnumerator ();
				while (ie.MoveNext ()) {

					if ((((GridNode)ie.Current).Row == row) && (((GridNode)ie.Current).Column == col))
						return (GridNode)ie.Current;
				}
			}

			#if DEBUG
			Debug.LogWarning("Unable to find grid node, but given row and column is in grid graph. row:"+row +"column:"+col);
			#endif
			return null;
		}

		//TODO has issue
		public override Path FindPath (Vector2 start, Vector2 end)
		{
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

			BinaryHeap<GridNode> openNodes = new BinaryHeap<GridNode>(delegate(GridNode parent, GridNode child) {
				return parent.F >= parent.F;
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

					bool update = false;

					//find g score for this neighbour
					float gScore = currentNode.G + conns [i].cost;

					//add neighbour node to open node if it is not
					//exist in open node and update neighbour node
					if (!openNodes.Contain (neighbourNode, delegate(GridNode first, GridNode second) {
						return (first.Id == second.Id);
					})) {

						openNodes.Add (neighbourNode);
						update = true;
					}

					//new path is better and update neighbour node
					if (gScore < neighbourNode.G)
						update = true;

					if (update) {

						neighbourNode.G = gScore;
						neighbourNode.H = Findheuristic (neighbourNode, endNode);
						neighbourNode.CameFrom = currentNode;
					}
				}
			}

			//construct path
			if (foundNode == null)
				return null;
			return ConstructPath(foundNode);
		}

		/**
		 * //TODO will be changed
		 **/
		protected override float Findheuristic (Node startNode, Node endNode)
		{
			GridNode gStartNode = (GridNode)startNode;
			GridNode gEndNode = (GridNode)endNode;

			Vector2 graphTopLeft = new Vector2 (_center.x - _horizontalNodes * _nodeSize / 2.0f,
				                       _center.y + _verticalNodes * _nodeSize / 2.0f);

			Vector2 startPos = new Vector2 (graphTopLeft.x + gStartNode.Column * _nodeSize + _nodeSize / 2.0f, 
				graphTopLeft.y - gStartNode.Row * _nodeSize - _nodeSize / 2.0f);

			Vector2 endPos = new Vector2 (graphTopLeft.x + gEndNode.Column * _nodeSize + _nodeSize / 2.0f, 
				graphTopLeft.y - gEndNode.Row * _nodeSize - _nodeSize / 2.0f);

			float h = Mathf.Abs (startPos.x - endPos.x) + Mathf.Abs (startPos.y - endPos.y);

			return h;
		}

		protected override Path ConstructPath (Node node)
		{
			//construct path
			GridNode currentNode = (GridNode)node;
			Path currentPath = null;

			Vector2 graphTopLeft = new Vector2 (_center.x - _horizontalNodes * _nodeSize / 2.0f,
				_center.y + _verticalNodes * _nodeSize / 2.0f);
			
			do {

				Vector2 pathPos = new Vector2(graphTopLeft.x + currentNode.Column * _nodeSize + _nodeSize / 2.0f, 
					graphTopLeft.y - currentNode.Row * _nodeSize - _nodeSize / 2.0f);

				Path newPath = new Path(pathPos, currentNode);

				if(currentPath != null){
					currentPath.PreviousPath = newPath;
					newPath.NextPath = currentPath;
				}

				currentPath = newPath;

				GridNode parent = (GridNode)currentNode.CameFrom;

				//make sure parent node is clean up
				currentNode.CameFrom = null;

				currentNode = parent;

			} while(currentNode != null);

			return currentPath;
		}

		public override void GenerateGraph ()
		{
			base.GenerateGraph ();

			for (int row = 0; row < _verticalNodes; row++) {

				for (int col = 0; col < _horizontalNodes; col++) {

					//create new grid node
					GridNode n = new GridNode (this, row, col);
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
					GridNode[] neighbours = new GridNode[8];
					neighbours[0] = FindNode(row, col - 1);//Left
					neighbours[1] = FindNode (row - 1, col);//Top
					neighbours[2] = FindNode (row, col + 1);//Right
					neighbours[3] = FindNode (row + 1, col);//Bottom
					neighbours[4] = FindNode (row - 1, col - 1);//TopLeft
					neighbours[5] = FindNode (row - 1, col + 1);//TopRight
					neighbours[6] = FindNode (row + 1, col + 1);//BottomRight
					neighbours[7] = FindNode (row + 1, col - 1);//BottomLeft

					for (int i = 0; i < neighbours.Length; i++) {

						GridNode neighbourNode = neighbours [i];

						if (neighbourNode != null) {

							//add connection both way
							n.AddConnection (neighbourNode);
							neighbourNode.AddConnection (n);
						}
					}
				}
			}
		}

		public override void DrawGraphGizmo ()
		{
			base.DrawGraphGizmo ();

			Vector2 gridOffset = new Vector2 (_center.x - _horizontalNodes * _nodeSize / 2.0f, 
				_center.y + _verticalNodes * _nodeSize / 2.0f);

			List<Node> nodes = new List<Node> (_nodes);
			nodes.Sort (((x, y) => -x.DrawPriority.CompareTo (y.DrawPriority)));

			for (int i = 0; i < nodes.Count; i++) {
			
				GridNode n = (GridNode)nodes [i];
				Vector2 nodeCenter = new Vector2 (n.Column * _nodeSize + _nodeSize / 2.0f + gridOffset.x,
					-(n.Row * _nodeSize + _nodeSize / 2.0f) + gridOffset.y);

				n.DrawNodeGizmo (nodeCenter, _nodeSize);

			}
		}
	}

}

