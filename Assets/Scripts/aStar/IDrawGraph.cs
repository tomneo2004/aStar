using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NP.aStarPathfinding{

	public interface IGizmoDrawable{
	
		int DrawPriority{ get;set;}
		Color DrawColor{ get;set;}
	}

	public interface IGizmoGraphDrawable : IGizmoDrawable{


		void DrawGraphGizmo();
	}

	public interface IGizmoNodeDrawable : IGizmoDrawable{
	

		void DrawNodeGizmo (Vector2 center, float size);
	}
}

