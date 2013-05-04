using UnityEngine;
using System.Collections;

public class UIOptions : UIController {

    protected override void OnActive(bool active) {
    }

    protected override void OnOpen() {
        Main.instance.sceneManager.Pause();
    }

    protected override void OnClose() {
        Main.instance.sceneManager.Resume();
    }

    // Use this for initialization
    void Start() {

    }
}
