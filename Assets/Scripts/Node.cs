using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NP.aStarPathfinding{

	public enum ConnectionType{
	
		/**
		 * Connection is from A->B and B->A both way
		 **/
		Undirect,

		/**
		 * Connection is from A->B only one way
		 **/
		Direct
	}

	public class Node : ObjectBase {

		/**
		 * The graph this node belong to
		 **/
		Graph _graph;

		/**
		 * Get the graph this node belong to
		 **/
		public Graph NodeGraph{ get{ return _graph;}}

		/**
		 * All connections in this node
		 **/
		List<Connection> _connections = null;

		/**
		 * Get all connections in this node
		 **/
		public List<Connection> AllConnections{ get{ return _connections;}}

		public Node(Graph graph) : base(){

			_graph = graph;
		}

	}
}
