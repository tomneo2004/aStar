using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using NP.aStarPathfinding;
using NP.aStarPathfindingEditor;

namespace NP.aStarPathfindingEditor{

	internal struct GridDrawer{
	
		Vector2 _center;
		Color _color;
		float _size;

		public GridDrawer(Vector2 center, Color color, float size){
		
			_center = center;
			_color = color;
			_size = size;
		}

		public void Draw(){

			Handles.color = _color;
			Handles.DrawWireCube (_center, new Vector3 (_size, _size, 0.0f));
		}
	}

	[CanEditMultipleObjects]
	[CustomEditor(typeof(GridGraphGenerator))]
	public class GridGraphEditor : GraphEditor {

		protected SerializedProperty p_NodeWidth;
		protected SerializedProperty p_NodeHeight;
		protected SerializedProperty p_NodeSize;

		bool centerFoldout = false;
		bool showUnwalkableGrid = true;

		protected Color unwalkableGridColor = Color.red;

		List<GridDrawer> walkableGrids = new List<GridDrawer>();
		List<GridDrawer> unwalkableGrids = new List<GridDrawer>();

		public virtual void OnEnable(){

			p_NodeWidth = serializedObject.FindProperty ("nodeWidth");
			p_NodeHeight = serializedObject.FindProperty ("nodeHeight");
			p_NodeSize = serializedObject.FindProperty ("nodeSize");

		}

		protected override void DrawProperties (){

			p_NodeWidth.intValue = EditorGUILayout.IntField (new GUIContent("Node Width",
			"Number of nodes in horizontal"), p_NodeWidth.intValue);
			p_NodeHeight.intValue = EditorGUILayout.IntField (new GUIContent("Node Height",
				"Number of nodes in vertical"), p_NodeHeight.intValue);
			p_NodeSize.floatValue = EditorGUILayout.FloatField (new GUIContent("Node Size",
				"Size of node"), p_NodeSize.floatValue);

			GridGraphGenerator g = (GridGraphGenerator)target;
			g.obstacleLayer = EditorUtil.LayerMaskField ("Obstacle Layer", g.obstacleLayer, "The layer which will be" +
				" used in collision test and act as obstacle");
		}

		protected override void DrawSceneGUI (){

			base.DrawSceneGUI ();
		}

		protected override void DrawGraphVisual (Graph graph){

			base.DrawGraphVisual (graph);

			walkableGrids.Clear ();
			unwalkableGrids.Clear ();

			GridGraph grid = (GridGraph)graph;

			Vector2 graphTopLeft = new Vector2 (grid.Center.x - grid.HorizontalNode * grid.NodeSize / 2.0f,
				grid.Center.y + grid.VerticalNodes * grid.NodeSize / 2.0f);
			
			IEnumerator ie = grid.AllNodes.GetEnumerator ();
			while (ie.MoveNext ()) {

				GridNode n = (GridNode)ie.Current;

				Color gColor = graphColor;
				if (!n.Walkable)
					gColor = unwalkableGridColor;

				Vector2 nodeCenter = new Vector2 (graphTopLeft.x + n.Column * grid.NodeSize + grid.NodeSize / 2.0f,
					                     graphTopLeft.y - n.Row * grid.NodeSize - grid.NodeSize / 2.0f);

				GridDrawer gd = new GridDrawer (nodeCenter, gColor, grid.NodeSize);

				if (n.Walkable)
					walkableGrids.Add (gd);
				else {
					if(showUnwalkableGrid)
						unwalkableGrids.Add (gd);
				}
			}

			DrawGrid ();
		}

		private void DrawGrid(){

			for (int i = 0; i < walkableGrids.Count; i++)
				walkableGrids [i].Draw ();

			for (int i = 0; i < unwalkableGrids.Count; i++)
				unwalkableGrids [i].Draw ();
		}

		protected override void DrawGraphVisualProperties ()
		{
			base.DrawGraphVisualProperties ();

			bool repaintScene = false;

			bool showUnwalkable = EditorGUILayout.Toggle (new GUIContent ("Show Unwalkable Grid", "Show unwalkable grid" +
			" in grid graph"), showUnwalkableGrid);

			if (showUnwalkable != showUnwalkableGrid) {

				showUnwalkableGrid = showUnwalkable;
				repaintScene = true;
			}

			if (showUnwalkableGrid) {
				EditorGUI.indentLevel += 1;
				unwalkableGridColor = EditorGUILayout.ColorField ("Unwalkable Color", unwalkableGridColor);
				EditorGUI.indentLevel -= 1;
			}

			if (repaintScene)
				SceneView.RepaintAll ();
		}

		protected override void DrawGraphInformation (Graph graph){

			base.DrawGraphInformation (graph);

			GridGraph grid = (GridGraph)graph;

			EditorGUI.indentLevel += 1;
			if (centerFoldout = EditorGUILayout.Foldout (centerFoldout, 
				new GUIContent("Graph Center", "Graph's center"))) {

				EditorGUI.indentLevel += 1;
				EditorGUILayout.LabelField ("X:" + grid.Center.x);
				EditorGUILayout.LabelField ("Y:" + grid.Center.y);
				EditorGUI.indentLevel -= 1;
			}
			EditorGUI.indentLevel -= 1;
		}

	}

}
