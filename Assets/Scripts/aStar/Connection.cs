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

		public Connection(Node from, Node to) : base(){

			_from = from;
			_to = to;
		}

	}
}
