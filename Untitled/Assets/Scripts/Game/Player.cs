using UnityEngine;
using System.Collections;

public class Player : UnitBaseEntity {
   
    private bool mInputEnabled = false;

    public override int flockId { get { return 1; } }
    
    public override void SpawnFinish() {
        //enable input
    }

    protected override void OnDespawned() {
        //disable input

        base.OnDespawned();
    }

    protected override void OnDestroy() {
        //disable input

        base.OnDestroy();
    }

    protected override void Awake() {
        base.Awake();

    }

    // Use this for initialization
    protected override void Start() {
        base.Start();

    }

    protected override void SpawnStart() {
    }

    // Update is called once per frame
    void Update() {
        //float dt = Time.deltaTime;

        //InputManager input = Main.instance.input;

        //Vector3 delta = Vector3.zero;

        //delta.x = speed * input.GetAxis(0, InputAction.MoveHorizontal) * dt;
        //delta.y = fallSpeed * dt;

        //mCharCtrl.Move(delta);
    }

    void OnInputJump(InputManager.Info data) {
    }

    void OnInputAction(InputManager.Info data) {
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        //Debug.Log("hit: " + hit.collider.gameObject.name);
    }

    private void InputEnable(bool yes) {
        if(yes) {
        }
        else {
        }

        mInputEnabled = yes;
    }
}
