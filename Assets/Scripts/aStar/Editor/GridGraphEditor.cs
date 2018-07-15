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

		bool centerFoldout = false;

		public virtual void OnEnable(){

			p_NodeWidth = serializedObject.FindProperty ("nodeWidth");
			p_NodeHeight = serializedObject.FindProperty ("nodeHeight");
			p_NodeSize = serializedObject.FindProperty ("nodeSize");

		}

		protected override void DrawProperties (){

			p_NodeWidth.intValue = EditorGUILayout.IntField ("NodeWidth", p_NodeWidth.intValue);
			p_NodeHeight.intValue = EditorGUILayout.IntField ("NodeHeight", p_NodeHeight.intValue);
			p_NodeSize.floatValue = EditorGUILayout.FloatField ("NodeSize", p_NodeSize.floatValue);

			GridGraphGenerator g = (GridGraphGenerator)target;
			g.obstacleLayer = EditorUtil.LayerMaskField ("ObstacleLayer", g.obstacleLayer);
		}

		protected override void DrawSceneGUI (){

			base.DrawSceneGUI ();
		}

		protected override void DrawGraphVisual (Graph graph){

			base.DrawGraphVisual (graph);

			GridGraph grid = (GridGraph)graph;

			Vector2 graphTopLeft = new Vector2 (grid.Center.x - grid.HorizontalNode * grid.NodeSize / 2.0f,
				grid.Center.y + grid.VerticalNodes * grid.NodeSize / 2.0f);
			
			IEnumerator ie = grid.AllNodes.GetEnumerator ();
			while (ie.MoveNext ()) {

				GridNode n = (GridNode)ie.Current;

				if (n.Walkable)
					Handles.color = graphColor;
				else
					Handles.color = unwalkableGridColor;

				Vector2 nodeCenter = new Vector2 (graphTopLeft.x + n.Column * grid.NodeSize + grid.NodeSize / 2.0f,
					                     graphTopLeft.y - n.Row * grid.NodeSize - grid.NodeSize / 2.0f);

				Handles.DrawWireCube (nodeCenter, new Vector3 (grid.NodeSize, grid.NodeSize, 0.0f));
			}
				
		}

		protected override void DrawGraphInformation (Graph graph){

			base.DrawGraphInformation (graph);

			GridGraph grid = (GridGraph)graph;

			EditorGUI.indentLevel += 1;
			if (centerFoldout = EditorGUILayout.Foldout (centerFoldout, "Graph Center")) {

				EditorGUI.indentLevel += 1;
				EditorGUILayout.LabelField ("X:" + grid.Center.x);
				EditorGUILayout.LabelField ("Y:" + grid.Center.y);
				EditorGUI.indentLevel -= 1;
			}
			EditorGUI.indentLevel -= 1;
		}

	}

}
