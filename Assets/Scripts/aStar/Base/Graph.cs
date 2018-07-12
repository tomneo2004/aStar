using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

namespace NP.aStarPathfinding{

	public abstract class Graph : ObjectBase, IGizmoGraphDrawable {

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

		protected Color _drawColor;
		public Color DrawColor{

			get{ return _drawColor;}
			set{ _drawColor = value;}
		}

		protected int _drawPriority = 1;
		public int DrawPriority{

			get{ return _drawPriority;}
			set{ _drawPriority = value;}
		}

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
		 * Remove all nodes from graph
		 **/
		public virtual void RemoveAllNodes(){

			if (_nodes != null && _nodes.Count > 0) {

				for (int i = 0; i < _nodes.Count; i++)
					_nodes [i].RemoveAllConnections ();

				_nodes.Clear ();
			}
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
		 * Find path
		 **/
		public abstract Path FindPath (Vector2 start, Vector2 end);

		/**
		 * //TODO provide more heuristic calculation
		 * Find heuristic value
		 **/
		protected abstract float Findheuristic (Node startNode, Node endNode);

		/**
		 * Construct path
		 * 
		 * Node is the last node a* found in path
		 **/
		protected abstract Path ConstructPath (Node node);

		/**
		 * Generate graph
		 * 
		 * Subclass must override
		 **/
		public abstract void GenerateGraph ();

		public virtual void DrawGraphGizmo (){

			Gizmos.color = _drawColor;
		}
	}
}
