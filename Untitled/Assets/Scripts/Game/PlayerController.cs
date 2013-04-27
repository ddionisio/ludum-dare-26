using UnityEngine;
using System.Collections;

public class PlayerController : MotionBase {

    public float force;

    private bool mInputEnabled = false;

    [System.NonSerialized]
    public PlayerActSensor curActSensor;

    public bool inputEnabled {
        get { return mInputEnabled; }
        set {
            if(mInputEnabled != value) {
                InputManager input = Main.instance != null ? Main.instance.input : null;

                mInputEnabled = value;

                if(value) {
                    input.AddButtonCall(0, InputAction.Action, OnInputAction);
                }
                else {
                    input.RemoveButtonCall(0, InputAction.Action, OnInputAction);
                }
            }
        }
    }

    void OnDisable() {
        curActSensor = null;
    }

    protected override void Awake() {
        base.Awake();

    }

    protected override void FixedUpdate() {
        if(mInputEnabled) {
            InputManager input = Main.instance.input;

            float moveX = input.GetAxis(0, InputAction.MoveHorizontal);
            float moveY = input.GetAxis(0, InputAction.MoveVertical);

            if(moveX != 0.0f || moveY != 0.0f) {
                body.AddForce(moveX * force, moveY * force, 0.0f);
            }
        }

        base.FixedUpdate();
    }

    void OnInputAction(InputManager.Info data) {
        if(data.state == InputManager.State.Pressed) {
            if(curActSensor != null) {
                curActSensor.Action();
            }
        }
    }
}
