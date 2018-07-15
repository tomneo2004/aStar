using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.aStarPathfinding;
using NP.aStarPathfindingAttributes;

namespace NP.aStarPathfinding{


	public abstract class GraphGenerator : MonoBehaviour {

		[SerializeField]
		protected Graph _graph;
		public Graph Graph{get{ return _graph;}}

		public T GetGraphByType<T>(){

			if (_graph != null) {

				try{
					return (T)Convert.ChangeType(_graph, typeof(T));

				} catch(InvalidCastException){

					return default(T);
				}
			}

			return default(T);
		}

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		[ExposeMethodInEditor("Generate Graph")]
		public abstract void GenerateGraph ();
	}
}

