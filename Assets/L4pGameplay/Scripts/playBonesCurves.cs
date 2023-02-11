using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class playBonesCurves : MonoBehaviour {

	public getBonesPath getBonespath;
	public Transform root;

	public bool justRoot;
	public Vector3 rootOffset;

	[System.Serializable] 
	public class Play
	{ 

		public KeyCode start = KeyCode.A;
		public KeyCode stop = KeyCode.S;
		public KeyCode cont = KeyCode.D;
		public bool onStart;
	}
	public Play play = new Play();

	public bool syncClip = true;

	[System.Serializable] 
	public class Sync
	{ 

		public Animator animator;
		public string state;
	}
	public Sync sync = new Sync();


	int i,x;

	public float timescale = 0.5f;
	public float time;
	public bool timecount;
	float starttime;

	[System.Serializable] 
	public class Display
	{ 

		public Text time;
	}
	public Display display = new Display();


	void Start () {
		if (play.onStart) {

			timecount = true;
			starttime = Time.time;
		}
	}
	

	void Update () {


		if (Input.GetKeyDown (play.start)) {
			timecount = true;
			starttime = Time.time;
			Time.timeScale = timescale;

			if (syncClip) {
				sync.animator.speed = timescale;
				sync.animator.CrossFadeInFixedTime (sync.state, 0);
			}
		}

		if (Input.GetKeyDown (play.stop)) {
			timecount = false;
			Time.timeScale = 0;
		}

		if (Input.GetKeyDown (play.cont)) {
			timecount = true;
			Time.timeScale = timescale;
		}

		if (timecount) time = Time.time - starttime;



		if (display.time) display.time.text = "" + time;

		//if (playIn == a.update) {


		//}
	}


	void LateUpdate(){

		if (timecount) {

			for (i = 0; i < getBonespath.allChildren.Length; i++) {

				if (i == 0) {

					root.position = new Vector3 (
						getBonespath.boneCurves [0].curve [0].Evaluate (time),
						getBonespath.boneCurves [0].curve [1].Evaluate (time),
						getBonespath.boneCurves [0].curve [2].Evaluate (time)
					);

					root.localRotation = new Quaternion (
						getBonespath.boneCurves [0].curve [3].Evaluate (time),
						getBonespath.boneCurves [0].curve [4].Evaluate (time),
						getBonespath.boneCurves [0].curve [5].Evaluate (time),
						getBonespath.boneCurves [0].curve [6].Evaluate (time)
					);
				}

				if (i != 0 && !justRoot) {
					getBonespath.allChildren [i].localRotation = new Quaternion (
						getBonespath.boneCurves [i].curve [0].Evaluate (time),
						getBonespath.boneCurves [i].curve [1].Evaluate (time),
						getBonespath.boneCurves [i].curve [2].Evaluate (time),
						getBonespath.boneCurves [i].curve [3].Evaluate (time)
					);
				}

			}
				
		}

		root.rotation = Quaternion.Euler (rootOffset);
	}

}
