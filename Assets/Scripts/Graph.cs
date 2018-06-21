using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

namespace NP.aStarPathfinding{

	public class Graph : ObjectBase {

		/**
		 * All nodes in this graph
		 **/
		List<Node> _nodes = null;

		/**
		 * Return all nodes in graph
		 * 
		 * Return null if there is no nodes
		 **/
		public List<Node> AllNodes{ get{ return _nodes;}}

		public Graph() : base(){
			
		}

	}
}
