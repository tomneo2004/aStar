using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NP.aStarPathfinding{

	public class Connection : ObjectBase {

		/**
		 * The node this connection connect from
		 **/
		protected Node _from;

		/**
		 * Get the node this connection connect from
		 **/
		public Node From{ get{ return _from;}}

		/**
		 * The node this connection connect to
		 **/
		protected Node _to;

		/**
		 * Get the node this connection connect to
		 **/
		public Node To{ get{ return _to;}}

		/**
		 * The cost of connection from "from node" to "to node"
		 **/
		protected int _cost;

		/**
		 * The cost of connection from "from node" to "to node"
		 * 
		 * Get cost of connection
		 * 
		 * Set cost of connection
		 **/
		public int cost{

			get{ return _cost;}

			set { _cost = value;}
		}

		public Connection(Node from, Node to, int cost = 1) : base(){

			_from = from;
			_to = to;
			_cost = cost;
		}

	}
}
