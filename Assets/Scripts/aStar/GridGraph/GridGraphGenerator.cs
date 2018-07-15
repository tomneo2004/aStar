using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

public class GridGraphGenerator : GraphGenerator {

	public GridGraph GridGraph{ get{ return GetGraphByType<GridGraph> ();}}

	public int nodeWidth = 5;
	public int nodeHeight = 5;
	public float nodeSize = 5;
	public LayerMask obstacleLayer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void GenerateGraph ()
	{

	}
}
