using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NP.aStarPathfindingAttributes{

	[AttributeUsage(AttributeTargets.Method)]
	public class ExposeMethodInEditor : Attribute{

		/**
		 * Method display name
		 **/
		public readonly string methodName;

		public ExposeMethodInEditor(string name){

			methodName = name;
		}
	}
}
