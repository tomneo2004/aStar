using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

namespace NP.aStarPathfinding{

	public class Path {

		/**
		 * Reference to node that associate with this path
		 **/
		protected Node _node;

		/**
		 * Path position in world space
		 **/
		protected Vector2 _position;

		/**
		 * Get position of this path
		 **/
		public Vector2 Position{ get{ return _position;}}

		/**
		 * Previous path
		 **/
		protected Path _previous;

		public Path PreviousPath{ 

			get{ return _previous;}

			set{ _previous = value;}
		}

		/**
		 * Next path
		 **/
		protected Path _next;

		public Path NextPath{

			get{ return _next;}

			set{ _next = value;}
		}

		/**
		 * Is this a root path
		 **/
		public bool IsRootPath{ 

			get{ return _previous == null ? true : false;}
		}

		public Path(Vector2 position, Node node){

			_position = position;
			_node = node;
		}
	}
}

