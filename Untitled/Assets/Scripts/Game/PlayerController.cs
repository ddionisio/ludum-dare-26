using UnityEngine;
using System.Collections;

public class PlayerController : MotionBase {

    public float force;

    private bool mInputEnabled = false;

    public bool inputEnabled {
        get { return mInputEnabled; }
        set {
            if(mInputEnabled != value) {
                mInputEnabled = value;

                if(value) {
                }
                else {
                }
            }
        }
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
}
