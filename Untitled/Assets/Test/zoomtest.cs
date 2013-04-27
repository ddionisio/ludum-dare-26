using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class zoomtest : MonoBehaviour {
    tk2dCamera cam;

    public float zoom = 1.0f;

	// Use this for initialization
	void Start () {
        cam = GetComponent<tk2dCamera>();
	}
	
	// Update is called once per frame
	void Update () {
        cam.zoomScale = zoom;
	}
}
