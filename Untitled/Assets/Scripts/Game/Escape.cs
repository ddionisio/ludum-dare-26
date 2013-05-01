using UnityEngine;
using System.Collections;

public class Escape : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Main.instance.input.AddButtonCall(0, InputAction.Escape, OnInput);
	}

    void OnInput(InputManager.Info dat) {
        if(dat.state == InputManager.State.Pressed)
            Application.Quit();
    }
}
