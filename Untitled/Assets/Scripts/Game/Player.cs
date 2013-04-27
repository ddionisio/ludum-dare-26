using UnityEngine;
using System.Collections;

public class Player : UnitBaseEntity {
    public float force;
   
    private bool mInputEnabled = false;

    private PlayerController mController;
    private PlayerHealth mHealth;

    public override int flockId { get { return 1; } }
    
    public override void SpawnFinish() {
        //enable input
        mController.inputEnabled = true;
    }

    protected override void OnDespawned() {
        //disable input
        mController.inputEnabled = false;

        mHealth.ResetStats();

        base.OnDespawned();
    }

    protected override void OnDestroy() {
        //disable input
        mController.inputEnabled = false;

        base.OnDestroy();
    }

    protected override void Awake() {
        base.Awake();

        mController = GetComponent<PlayerController>();
        mHealth = GetComponent<PlayerHealth>();
        mHealth.hitCallback += OnHit;
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();

        CameraController.instance.attachTo = transform;
    }

    protected override void SpawnStart() {
    }

    void OnInputJump(InputManager.Info data) {
    }

    void OnInputAction(InputManager.Info data) {
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        //Debug.Log("hit: " + hit.collider.gameObject.name);
    }

    void OnHit(PlayerHealth playerHealth) {
        if(playerHealth.curHealth <= 0.0f) {
            Debug.Log("Game Over!");
        }
    }

    private void InputEnable(bool yes) {
        if(yes) {
        }
        else {
        }

        mInputEnabled = yes;
    }
}
