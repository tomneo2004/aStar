using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NP.aStarPathfinding;
using NP.aStarPathfindingEditor;

namespace NP.aStarPathfindingEditor{

	[CanEditMultipleObjects]
	[CustomEditor(typeof(AAGraphGenerator))]
	public class AAGraphEditor : GridGraphEditor {


		public override void OnEnable(){

			base.OnEnable ();
		}


		protected override void DrawProperties ()
		{
			base.DrawProperties ();
		}
	}
}

