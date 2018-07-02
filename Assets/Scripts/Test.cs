using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

public class Test : MonoBehaviour {

	public int nodeWidth = 5;
	public int nodeHeight = 5;
	public float nodeSize = 5;

	public Color drawColor = Color.green;

	GridNode pickedNode;

	GridGraph grid;

	// Use this for initialization
	void Start () {

		grid = new GridGraph (new Vector2(transform.position.x, transform.position.y));
		grid.DrawColor = drawColor;
		grid.HorizontalNode = nodeWidth;
		grid.VerticalNodes = nodeHeight;
		grid.NodeSize = nodeSize;

		grid.GenerateGraph ();

	}
	
	// Update is called once per frame
	void Update () {

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
	}

	void OnDrawGizmos(){

		if (grid != null) {
			grid.DrawColor = drawColor;
			grid.DrawGraphGizmo ();
		}
	}
}
