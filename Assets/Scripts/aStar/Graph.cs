using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

namespace NP.aStarPathfinding{

	public abstract class Graph : ObjectBase {

		/**
		 * All nodes in this graph
		 **/
		protected List<Node> _nodes = null;

		/**
		 * Return all nodes in graph
		 * 
		 * Return null if there is no nodes
		 **/
		public List<Node> AllNodes{ get{ return _nodes;}}

		public Graph() : base(){

			_nodes = new List<Node> ();
		}

		/**
		 * Add node to graph
		 **/
		public virtual void AddNode(Node node){

			Node n = FindNode (node.Id);

			if (n == null) {
				_nodes.Add (node);
				return;
			}

			#if DEBUG
			Debug.LogWarning("Node is exist in graph, we do not add node");
			#endif
		}


		/**
		 * Remove node from graph
		 **/
		public virtual void RemoveNode(Node node){

			if (node.NodeGraph != this || (FindNode(node.Id) == null)) {
				#if DEBUG
				Debug.LogWarning("Node does not exist in graph, we do nothing");
				#endif

				return;
			}

			node.RemoveAllConnections ();

			_nodes.Remove (node);
		}

		/**
		 * Find node by node's id
		 **/
		public virtual Node FindNode(Guid nodeId){

			IEnumerator ie = _nodes.GetEnumerator ();
			while (ie.MoveNext ()) {
			
				if (((Node)ie.Current).Id == nodeId)
					return (Node)ie.Current;
			}

			return null;
		}

		/**
		 * Generate graph
		 * 
		 * Subclass must override
		 **/
		public abstract void GenerateGraph ();
	}
}
