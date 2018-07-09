using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

public class Test : MonoBehaviour {

	public int nodeWidth = 5;
	public int nodeHeight = 5;
	public float nodeSize = 5;
	public LayerMask obstacleLayer;

	public Color drawColor = Color.green;
	public TextMesh fScoreText;
	List<TextMesh> allFScoreText = new List<TextMesh> ();

	public int agentSize = 1;

	//GridNode pickedNode;

	GridGraph grid;

	Vector2 startPos;
	Vector2 goalPos;
	Path path;

	// Use this for initialization
	void Start () {

//		grid = new GridGraph (new Vector2(transform.position.x, transform.position.y));
		grid = new AAGraph (new Vector2(transform.position.x, transform.position.y));
		grid.DrawColor = drawColor;
		grid.HorizontalNode = nodeWidth;
		grid.VerticalNodes = nodeHeight;
		grid.NodeSize = nodeSize;
		grid.collisionLayerMask = obstacleLayer.value;

		grid.GenerateGraph ();

		for (int i = 0; i < grid.AllNodes.Count; i++) {

			Node n = grid.AllNodes [i];

			TextMesh tm = Instantiate (fScoreText);
			tm.text = n.F.ToString ();
			tm.gameObject.SetActive (false);

			allFScoreText.Add (tm);
		}

	}
	
	// Update is called once per frame
	void Update () {
		/*
		Vector2 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		GridNode n = grid.FindNode (worldPos);
		if (n != pickedNode) {

			if (pickedNode != null) {
				pickedNode.DrawColor = grid.DrawColor;
				pickedNode.DrawPriority = grid.DrawPriority;
			}

			if (n != null) {
				n.DrawColor = Color.red;
				n.DrawPriority = 0;
			}


			pickedNode = n;
		}
		*/

		if (Input.GetMouseButtonDown (0)) {
		
			startPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			FindPath ();
		}

		if (Input.GetMouseButtonDown (1)) {

			goalPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			FindPath ();
		}
	}

	void FindPath(){

		if (startPos == null || goalPos == null)
			return;

		path = ((AAGraph)grid).FindPath (startPos, goalPos, agentSize);
	}

	void OnDrawGizmos(){

		if (grid != null) {
			grid.DrawColor = drawColor;
			grid.DrawGraphGizmo ();
		}

		Vector3 cubeSize = new Vector3(0.3f,0.3f,0.3f);
		if (startPos != null) {

			Gizmos.color = Color.yellow;
			Gizmos.DrawCube (startPos, cubeSize);
		}

		if (goalPos != null) {

			Gizmos.color = Color.red;
			Gizmos.DrawCube (goalPos, cubeSize);
		}

		if (path != null) {

			Path currentPath = path;

			Gizmos.color = Color.yellow;

			while (currentPath.NextPath != null) {

				Gizmos.DrawLine (currentPath.Position, currentPath.NextPath.Position);
				currentPath = currentPath.NextPath;
			}

		}

		if (grid != null && grid.AllNodes != null) {

			Vector2 graphTopLeft = new Vector2 (grid.Center.x - grid.HorizontalNode * grid.NodeSize / 2.0f,
				grid.Center.y + grid.VerticalNodes * grid.NodeSize / 2.0f);
			
			for (int i = 0; i < grid.AllNodes.Count; i++) {

				GridNode n = (GridNode)grid.AllNodes [i];

				Vector2 pos = new Vector2 (graphTopLeft.x + n.Column * grid.NodeSize + grid.NodeSize / 2.0f, 
					              graphTopLeft.y - n.Row * grid.NodeSize - grid.NodeSize / 2.0f);

				TextMesh tm = allFScoreText [i];
				tm.gameObject.SetActive (true);
//				tm.text = n.F.ToString ();
				tm.text = ((AAGraph)grid).FindClearance(n).ToString();
				tm.transform.position = pos;
			}
		}
			
	}
}
