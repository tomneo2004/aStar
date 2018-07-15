using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NP.aStarPathfinding;
using NP.aStarPathfindingAttributes;


namespace NP.aStarPathfindingEditor{

	[CanEditMultipleObjects]
	[CustomEditor(typeof(GraphGenerator), true)]
	public abstract class GraphEditor : Editor {

		public override void OnInspectorGUI (){

			DrawProperties ();
			DrawMethodButton ();

			serializedObject.ApplyModifiedProperties ();
		}

		void OnSceneGUI(){

			DrawSceneGUI ();
		}

		protected abstract void DrawProperties ();
		protected abstract void DrawSceneGUI ();

		private void DrawMethodButton(){
		
			Type t = target.GetType ();

			foreach (MethodInfo method in t.GetMethods(
				BindingFlags.NonPublic 
				| BindingFlags.Public 
				| BindingFlags.Instance)) {

				var attribute = method.GetCustomAttributes (typeof(ExposeMethodInEditor), true);

				if (attribute.Length > 0) {

					if (GUILayout.Button (((ExposeMethodInEditor)attribute [0]).methodName)) {

						((GraphGenerator)target).GenerateGraph ();
					}
				}
			}
		}
	}
}

