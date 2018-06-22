using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NP.aStarPathfinding{

	public class ObjectBase {

		/**
	 * Unique id.
	 * Used to identify object
	 **/
		protected Guid _id;

		/**
	 * Return unique id of this object
	 **/
		public Guid Id{ get{ return _id;}}

		public ObjectBase(){

			_id = Guid.NewGuid ();
		}
	}
}
