using UnityEngine;
using System.Collections;

public class UIHealth : MonoBehaviour {
    public UILabel label;

    private NGUIAttach mAttach;

    public void Attach(Transform t) {
        mAttach.target = t;
    }

    void Awake() {
        mAttach = GetComponent<NGUIAttach>();
    }
}
