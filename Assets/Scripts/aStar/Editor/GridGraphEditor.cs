using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using NP.aStarPathfinding;
using NP.aStarPathfindingEditor;

namespace NP.aStarPathfindingEditor{

	[CanEditMultipleObjects]
	[CustomEditor(typeof(GridGraphGenerator))]
	public class GridGraphEditor : GraphEditor {

		protected SerializedProperty p_NodeWidth;
		protected SerializedProperty p_NodeHeight;
		protected SerializedProperty p_NodeSize;
		protected SerializedProperty p_ObstacleLayer;

		public virtual void OnEnable(){

			p_NodeWidth = serializedObject.FindProperty ("nodeWidth");
			p_NodeHeight = serializedObject.FindProperty ("nodeHeight");
			p_NodeSize = serializedObject.FindProperty ("nodeSize");
			p_ObstacleLayer = serializedObject.FindProperty ("obstacleLayer");
		}

		protected override void DrawProperties (){

			p_NodeWidth.intValue = EditorGUILayout.IntField ("NodeWidth", p_NodeWidth.intValue);
			p_NodeHeight.intValue = EditorGUILayout.IntField ("NodeHeight", p_NodeHeight.intValue);
			p_NodeSize.floatValue = EditorGUILayout.FloatField ("NodeSize", p_NodeSize.floatValue);
			p_ObstacleLayer.intValue = EditorGUILayout.MaskField ("ObstacleLayer", 
				p_ObstacleLayer.intValue, 
				InternalEditorUtility.layers);
		}

		protected override void DrawSceneGUI ()
		{
			
		}

	}

}
