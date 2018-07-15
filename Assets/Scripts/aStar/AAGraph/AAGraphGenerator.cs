using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

public class AAGraphGenerator : GridGraphGenerator {

	public AAGraph AAGraph{ get{ return GetGraphByType<AAGraph> ();}}

	void Start(){
	}

	// Update is called once per frame
	void Update () {
		
	}
		
	public override void GenerateGraph ()
	{
		_graph = new AAGraph (new Vector2(transform.position.x, transform.position.y));
		AAGraph tempGraph = (AAGraph)_graph;

		tempGraph.HorizontalNode = nodeWidth;
		tempGraph.VerticalNodes = nodeHeight;
		tempGraph.NodeSize = nodeSize;
		tempGraph.collisionLayerMask = obstacleLayer.value;

		tempGraph.GenerateGraph ();
	}
}
