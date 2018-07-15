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
	public class GraphEditor : Editor {

		bool propertyFoldout = false;
		bool informationFoldout = false;
		bool graphVisualFoldout = false;

		protected Color graphColor = Color.green;

		public override void OnInspectorGUI (){

			if(propertyFoldout = EditorGUILayout.Foldout(propertyFoldout, "Properties"))
				DrawProperties ();

			if (informationFoldout = EditorGUILayout.Foldout (informationFoldout, "Information")) {

				GraphGenerator gg = (GraphGenerator)serializedObject.targetObject;
				if (gg != null && gg.Graph != null)
					DrawGraphInformation (gg.Graph);
				else {
				
					EditorGUILayout.HelpBox ("No graph avaliable", MessageType.Warning);
				}
			}
				

			if (graphVisualFoldout = EditorGUILayout.Foldout (graphVisualFoldout, "Graph visual"))
				DrawGraphVisualProperties ();
			
			DrawMethodButton ();

			serializedObject.ApplyModifiedProperties ();
		}

		void OnSceneGUI(){

			GraphGenerator gg = (GraphGenerator)serializedObject.targetObject;
			if (gg != null && gg.Graph != null)
				DrawGraphVisual (gg.Graph);
			
			DrawSceneGUI ();
		}

		protected virtual void DrawProperties (){
		}
		protected virtual void DrawSceneGUI (){
		}
		protected virtual void DrawGraphVisual (Graph graph){
		}
		protected virtual void DrawGraphInformation (Graph graph){
		}

		protected virtual void DrawGraphVisualProperties(){

			graphColor = EditorGUILayout.ColorField ("VisualColor", graphColor);
		}

		protected virtual void DrawMethodButton(){
		
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

