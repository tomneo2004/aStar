using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

namespace NP.aStarPathfinding{

	public class AANode : GridNode {

		public float Clearance{get{ return CalculateClearance ();}}

		public AANode(AAGraph aAGraph, int row, int column) : base(aAGraph, row, column){
		}

		float CalculateClearance(){
		
			AAGraph aAGraph = (AAGraph)_graph;
			float clearance = aAGraph.NodeSize;
			int searchRow = _row + 1;
			int searchCol = _column + 1;

			while (searchRow < aAGraph.VerticalNodes && searchCol < aAGraph.HorizontalNode) {

				if (!aAGraph.FindNode (searchRow, searchCol).Walkable)
					break;

				bool block = false;

				for (int v = searchRow; v >= _row; v--) {

					if (!aAGraph.FindNode (v, searchCol).Walkable) {
						block = true;
						break;
					}
				}

				if (block)
					break;

				for (int h = searchCol; h >= _column; h--) {

					if (!aAGraph.FindNode (searchRow, h).Walkable) {
						block = true;
						break;
					}
				}

				if (block)
					break;

				clearance += aAGraph.NodeSize;
				searchRow++;
				searchCol++;
			}

			return clearance;
		}
	}
}

