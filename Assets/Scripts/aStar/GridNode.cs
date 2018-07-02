using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

namespace NP.aStarPathfinding{

	public class GridNode : Node {

		/**
	 * Row of this node in grid
	 **/
		int _row = 0;

		/**
	 * Get row of this node in grid
	 **/
		public int Row{ get{ return _row;}}

		/**
	 * Column of this node in grid
	 **/
		int _column = 0;

		/**
	 * Get column of this node in grid
	 **/
		public int Column{ get{ return _column;}}

		/**
	 * Get the reference to grid graph this node belong to
	 **/
		public GridGraph GridGraphRef{ 

			get{

				//check if graph is GridGraph
				if (_graph is GridGraph)
					return (GridGraph)_graph;

				#if DEBUG
				Debug.LogError("GridNode's graph is not a GridGraph, fix me");
				#endif
				return null;
			}
		}

		public GridNode(GridGraph gridGraph, int row, int column) : base(gridGraph){

			_row = row;
			_column = column;
		}

		public override void DrawNodeGizmo (Vector2 center, float size)
		{
			base.DrawNodeGizmo (center, size);

			Gizmos.DrawWireCube (center, new Vector3 (size, size, size));
		}
	}
}

