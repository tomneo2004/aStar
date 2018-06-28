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
		protected Graph _graph;

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

		/**
		 * Walkable boolean for this node
		 **/
		protected bool _walkable = true;

		/**
		 * Get walkable for this node
		 * 
		 * Set walkable for this node
		 **/
		public bool Walkable{

			get{ 
			
				return _walkable;
			}

			set{
			
				_walkable = value;
			}
		}

		public Node(Graph graph) : base(){

			_graph = graph;
			_connections = new List<Connection> ();
		}

		/**
		 * Add connection from this node to another node
		 **/
		public virtual Connection AddConnection(Node toNode){

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
			/*
			if (connType == ConnectionType.Undirect) {

				//tell another node to add connection to this node but only one way
				toNode.AddConnection (this, ConnectionType.Direct);
			}
			*/

			return conn;
		}

		/**
		 * Find connection from this node to another node
		 **/
		public virtual Connection FindConnection(Node toNode){

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
		 * 
		 * Undirect will remove connection from this node to another node and also incoming connection
		 * from another node.
		 * 
		 * Direct will only remove connection from this node to another node.
		 **/
		public virtual void RemoveConnection(Node toNode, ConnectionType connType = ConnectionType.Direct){

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

		/**
		 * Remove all connection from this node to other node.
		 * As well as, all incoming connection from other node.
		 * 
		 * This is an convenient for remove all connections that are
		 * related to this node
		 **/
		public virtual void RemoveAllConnections(){

			Connection[] conns = _connections.ToArray ();

			IEnumerator ie = conns.GetEnumerator ();
			while (ie.MoveNext ()) {
			
				RemoveConnection (((Connection)ie.Current).To, ConnectionType.Undirect);
			}

			#if DEBUG
			if(_connections.Count > 0)
				Debug.LogError("Connections is not remove completely, fix this");
			#endif
		}
	}
}
