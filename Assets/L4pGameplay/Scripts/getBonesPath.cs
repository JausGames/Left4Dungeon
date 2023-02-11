using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getBonesPath : MonoBehaviour {

	public Transform obj;
	public AnimationClip _clip;

	public bool isOn;

	public Transform[] allChildren;
	public bool[] use;

	Transform bone;
	Transform bonex;
	Transform bxhierarch;
	string[] unpath;//negative path

	public int maxdepth = 20;
	public int i;
	int x;

	public string[] path;

	public KeyCode getPaths = KeyCode.P;
	public KeyCode getCurves = KeyCode.Space;

	int k, kx;

	bool getpathx;
	bool getcurvesx;

	[System.Serializable]
	public class CRV
	{
		public AnimationCurve[] curve;
	}
	public CRV[] boneCurves;



	public bool doDebug;
	bool cdefined;

	void Start () {

		if (isOn) {
			
			allChildren = obj.GetComponentsInChildren<Transform> ();

			use = new bool[allChildren.Length];
			for (i = 0; i < allChildren.Length; i++) use [i] = true;
		
			path = new string[allChildren.Length];

			boneCurves = new CRV[allChildren.Length];
		}
	}

	void Update(){

		if (!isOn) return;
		
		if (!cdefined && Time.time > 0.5f) {
			cdefined = true;
			boneCurves [0].curve = new AnimationCurve[7];
			for (i = 1; i < allChildren.Length; i++) boneCurves [i].curve = new AnimationCurve[4];
		}

		if (Input.GetKeyDown (getPaths)) getpathx = true;
		if (Input.GetKeyDown (getCurves)) getcurvesx = true;

		if (getpathx) {
			getpathx = false;

			//get all path for used
			for(k=0; k<allChildren.Length; k++){

				if (use [k]) bone = allChildren [k];

				bonex = SearchHierarchyForBone(obj, bone.name);

				for (i = 0; i < maxdepth; i++) {

					if (i == 0) bxhierarch = bonex;

					if (bxhierarch.parent != null) {
						if (i == 1) bxhierarch = bonex.parent;
						if (i > 1) bxhierarch = bxhierarch.parent;
						if (bxhierarch == obj) x = i;
					}

				}
				unpath = new string[x];

				for (i = 0; i < x; i++) {

					if (i == 0) bxhierarch = bonex;

					if (bxhierarch.parent != null) {
						if (i == 1) bxhierarch = bonex.parent;
						if (i > 1) bxhierarch = bxhierarch.parent;
					}

					unpath [i] = bxhierarch.name;
				}

				path[k] += obj.name;
				for (i = unpath.Length-1; i > -1; i--) path[k] += "/" + unpath [i];
			}
		}

		if (getcurvesx) {
			getcurvesx = false;
			i = 0;

			var curveBindings = UnityEditor.AnimationUtility.GetCurveBindings (_clip);

			for (x = 0; x < path.Length; x++) {
				
				foreach (var curveBinding in curveBindings) {

					if (doDebug) Debug.Log (curveBinding.path + "," + curveBinding.propertyName);

					if(curveBinding.propertyName.Contains ("RootT.x")) boneCurves [0].curve[0] = UnityEditor.AnimationUtility.GetEditorCurve (_clip, curveBinding);
					if(curveBinding.propertyName.Contains ("RootT.y")) boneCurves [0].curve[1] = UnityEditor.AnimationUtility.GetEditorCurve (_clip, curveBinding);
					if(curveBinding.propertyName.Contains ("RootT.z")) boneCurves [0].curve[2] = UnityEditor.AnimationUtility.GetEditorCurve (_clip, curveBinding);
					if(curveBinding.propertyName.Contains ("RootQ.x")) boneCurves [0].curve[3] = UnityEditor.AnimationUtility.GetEditorCurve (_clip, curveBinding);
					if(curveBinding.propertyName.Contains ("RootQ.y")) boneCurves [0].curve[4] = UnityEditor.AnimationUtility.GetEditorCurve (_clip, curveBinding);
					if(curveBinding.propertyName.Contains ("RootQ.z")) boneCurves [0].curve[5] = UnityEditor.AnimationUtility.GetEditorCurve (_clip, curveBinding);
					if(curveBinding.propertyName.Contains ("RootQ.w")) boneCurves [0].curve[6] = UnityEditor.AnimationUtility.GetEditorCurve (_clip, curveBinding);

					if (curveBinding.path == path [x] && !curveBinding.propertyName.Contains ("LocalScale")) {

						boneCurves [x].curve[i] = UnityEditor.AnimationUtility.GetEditorCurve (_clip, curveBinding);

						i++;
						//if (x == 0 && i == 7) i = 0;
						//if (x != 0 && i == 4) i = 0;
						if (i == 4) i = 0;
					}
				}
			}

		}
	}


	public Transform SearchHierarchyForBone(Transform current, string name)   
	{
		// check if the current bone is the bone we're looking for, if so return it
		if (current.name.Contains(name)) return current;

		// search through child bones for the bone we're looking for
		for (int i = 0; i < current.childCount; ++i)
		{
			// the recursive step; repeat the search one step deeper in the hierarchy
			Transform found = SearchHierarchyForBone(current.GetChild(i), name);
			// a transform was returned by the search above that is not null,
			// it must be the bone we're looking for
			if (found != null) return found;
		}

		// bone with name was not found
		return null;
	}
}
