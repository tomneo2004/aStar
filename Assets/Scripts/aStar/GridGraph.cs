using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;

public class GridGraph : Graph {

	/**
	 * Number of node in horizontal
	 **/
	protected int _horizontalNodes = 0;

	/**
	 * Get number of node in horizontal
	 * 
	 * Set number of node in horizontal
	 **/
	public int HorizontalNode{

		get{

			return _horizontalNodes;
		}

		set{

			_horizontalNodes = value;
		}
	}

	/**
	 * Number of node in vertical
	 **/
	protected int _verticalNodes = 0;

	/**
	 * Get number of node in vertical
	 * 
	 * Set number of node in vertical
	 **/
	public int VerticalNodes{

		get{ 
		
			return _verticalNodes;
		}

		set{

			_verticalNodes = value;
		}
	}

	/**
	 * Size of node in graph
	 **/
	protected float _nodeSize = 5;

	/**
	 * Get size of node
	 * 
	 * Set size of node
	 **/
	public float NodeSize{
	
		get{ 
		
			return _nodeSize;
		}

		set{
		
			_nodeSize = value;
		}
	}

	public GridGraph () : base (){

	}

	public override void GenerateGraph ()
	{
		for (int row = 0; row < _horizontalNodes; row++) {

			for (int col = 0; col < _verticalNodes; col++) {

				//create new grid node
				GridNode n = new GridNode (this, row, col);

				//add new grid node to grid graph
				AddNode (n);

				//TODO configure new grid node if it is walkable or not

				//TODO connect this grid node to neighbour nodes
			}
		}
	}
}
