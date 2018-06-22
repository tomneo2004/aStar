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
			_connections = new List<Connection> ();
		}

		/**
		 * Add connection from this node to another node
		 * 
		 * Specify connection type to undirect or direct. Default is Undirect
		 **/
		public Connection AddConnection(Node toNode, ConnectionType connType = ConnectionType.Undirect){

			if (toNode == null) {
				#if DEBUG
				Debug.LogError("Unable to add connection. Given node is null");
				#endif

				return null;
			}

			//do not add connection if it is already exist
			Connection conn = null;
			conn = FindConnection (toNode);

			if (conn != null) {
				#if DEBUG
				Debug.LogWarning("Connection is exist, we do not add connection");
				#endif

				return conn;
			}

			//create new connection to another node
			conn = new Connection(this, toNode);

			//add to connection list
			_connections.Add(conn);

			//if connection is undirect
			if (connType == ConnectionType.Undirect) {

				//tell another node to add connection to this node but only one way
				toNode.AddConnection (this, ConnectionType.Direct);
			}

			return conn;
		}

		/**
		 * Find connection from this node to another node
		 **/
		public Connection FindConnection(Node toNode){

			Connection found = null;

			IEnumerator ie = _connections.GetEnumerator ();
			while (ie.MoveNext ()) {

				//found
				if (((Connection)ie.Current).To.Id == toNode.Id) {

					found = (Connection)ie.Current;
					break;
				}
			}

			return found;
		}

		/**
		 * Remove connection from this node to another node
		 * 
		 * Specify connection type to undirect or direct. Default is Direct(one way)
		 **/
		public void RemoveConnection(Node toNode, ConnectionType connType = ConnectionType.Direct){

			int connIndex = -1;
			int currIndex = 0;

			IEnumerator ie = _connections.GetEnumerator ();
			while (ie.MoveNext ()) {

				//found
				if (((Connection)ie.Current).To.Id == toNode.Id) {

					connIndex = currIndex;
					break;
				}

				currIndex++;
			}

			//has connection to another node, remove it otherwise do nothing
			if (connIndex >= 0) {
				
				_connections.RemoveAt (connIndex);

				if (connType == ConnectionType.Undirect) {

					//tell another node to remove connection to this node
					toNode.RemoveConnection (this, ConnectionType.Direct);
				}

				return;
			}

			#if DEBUG
			Debug.LogWarning("There is no connection to another node, we have nothing to remove");
			#endif
				
		}

	}
}
