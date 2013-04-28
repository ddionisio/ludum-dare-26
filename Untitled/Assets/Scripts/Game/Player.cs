using UnityEngine;
using System.Collections;

public class Player : UnitBaseEntity {
    public float force;
   
    private PlayerController mController;
    private PlayerHealth mHealth;
    private FlockFilter mFlockFilter;

    public override int flockId { get { return mFlockFilter.id; } }
    
    public override void SpawnFinish() {
        //enable input
        mController.inputEnabled = true;
    }

    protected override void OnDespawned() {
        //disable input
        mController.ResetState();

        mHealth.ResetStats();

        base.OnDespawned();
    }

    protected override void OnDestroy() {
        //disable input
        mController.ResetState();

        base.OnDestroy();
    }

    protected override void Awake() {
        base.Awake();

        mController = GetComponent<PlayerController>();
        mHealth = GetComponent<PlayerHealth>();
        mFlockFilter = GetComponent<FlockFilter>();

        mHealth.hitCallback += OnHit;
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();

        CameraController.instance.attachTo = transform;
    }

    protected override void SpawnStart() {
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        //Debug.Log("hit: " + hit.collider.gameObject.name);
    }

    void OnHit(PlayerHealth playerHealth) {
        if(playerHealth.curHealth <= 0.0f) {
            Debug.Log("Game Over!");
        }
    }
}
