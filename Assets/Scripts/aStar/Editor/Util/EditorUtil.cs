using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class EditorUtil {

	//contain index of layer
	static List<int> layerNumbers = new List<int> ();
	public static LayerMask LayerMaskField(string label, LayerMask layerMask){
	
		string[] layers = InternalEditorUtility.layers;

		layerNumbers.Clear ();

		for (int i = 0; i < layers.Length; i++)
			layerNumbers.Add (LayerMask.NameToLayer (layers [i]));


		//selected mask
		int maskWithoutEmpty = 0;
		for (int i = 0; i < layerNumbers.Count; i++) {

			//convert selected layer index to integer 
			if ((layerMask.value & (1 << layerNumbers [i])) > 0)
				maskWithoutEmpty |= 1 << i;
		}

		maskWithoutEmpty = EditorGUILayout.MaskField (label, maskWithoutEmpty, layers);

		int mask = 0;
		for (int i = 0; i < layerNumbers.Count; i++) {

			//convert selected mask to layer mask
			if ((maskWithoutEmpty & (1 << i)) > 0)
				mask |= 1 << layerNumbers [i];
		}

		layerMask.value = mask;

		return layerMask;
	}
}
