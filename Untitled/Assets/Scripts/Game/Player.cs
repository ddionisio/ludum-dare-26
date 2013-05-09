using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HutongGames.PlayMaker;

public class Player : UnitBaseEntity {
    public GameObject musicJukebox; //when no music is available, instantiate this
    public GameObject ui; //when ui is not available, instantiate this

    public float force;

    public float flowerDistance;
   
    private PlayerController mController;
    private PlayerHealth mHealth;
    private FlockFilter mFlockFilter;

    private List<Transform> mFlowers;

    private bool mSpawnFinished = false;

    private bool mInputEnabled = false;

    public override int flockId { get { return mFlockFilter.id; } }

    public bool spawnFinished { get { return mSpawnFinished; } }

    public PlayerController controller { get { return mController; } }
    public PlayerHealth health { get { return mHealth; } }

    public List<Transform> flowers { get { return mFlowers; } }

    private float mPlayStartTime;

    public bool inputEnabled {
        get { return mInputEnabled; }
        set {
            if(mInputEnabled != value) {
                InputManager input = Main.instance != null ? Main.instance.input : null;

                mInputEnabled = value;

                if(input != null) {
                    if(value) {
                        input.AddButtonCall(0, InputAction.Escape, OnInputPause);
                    }
                    else {
                        input.RemoveButtonCall(0, InputAction.Escape, OnInputPause);
                    }
                }
            }
        }
    }

    public int numFlowers {
        get {
            int num = 0;

            float distSq = flowerDistance * flowerDistance;

            Vector2 pos = transform.position;

            foreach(Transform t in mFlowers) {
                Vector2 flowerPos = t.position;
                Vector2 dPos = flowerPos - pos;
                if(dPos.sqrMagnitude <= distSq)
                    num++;
            }

            return num;
        }
    }

    public float curPlayTime {
        get {
            return Time.time - mPlayStartTime;
        }
    }

    public void Exit() {
        if(Application.loadedLevelName == "start") {
            Application.Quit();
        }
        else if(Application.loadedLevelName == "end") {
            Main.instance.sceneManager.LoadScene("start");
        }
        else if(Application.loadedLevelName == "game_ex_end") {
            //TODO: make sure to properly save data and determine which start_ex to load
            Main.instance.sceneManager.LoadLastSceneStack();
        }
        else {
            Main.instance.sceneManager.LoadLastSceneStack();
        }
    }

    public override void SpawnFinish() {
        //enable input
        mController.inputEnabled = true;

        mSpawnFinished = true;

        mPlayStartTime = Time.time;
    }

    protected override void OnDespawned() {
        //disable input
        mController.ResetState();

        mHealth.ResetStats();

        mSpawnFinished = false;

        base.OnDespawned();
    }

    protected override void OnDestroy() {
        //disable input
        mController.ResetState();

        inputEnabled = false;
                
        base.OnDestroy();
    }

    protected override void Awake() {
        base.Awake();

        mFlowers = new List<Transform>(10);

        mController = GetComponent<PlayerController>();
        mHealth = GetComponent<PlayerHealth>();
        mFlockFilter = GetComponent<FlockFilter>();

        mHealth.hitCallback += OnHit;
    }

    // Use this for initialization
    protected override void Start() {
        //check if music is available
        if(MusicManager.instance == null && musicJukebox != null) {
            GameObject.Instantiate(musicJukebox);
        }

        //check if ui is available
        if(UIModalManager.instance == null && ui != null) {
            GameObject.Instantiate(ui);
        }

        base.Start();

        CameraController.instance.attachTo = transform;
        CameraController.instance.SnapToAttach();

        //apply any level start point
        if(PlayerLevelStartPoint.instance != null) {
            PlayerLevelStartPoint.instance.Set(this);
        }
    }

    protected override void SpawnStart() {
        mController.SpawnStart();

        inputEnabled = true;
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        //Debug.Log("hit: " + hit.collider.gameObject.name);
    }

    void OnHit(PlayerHealth playerHealth) {
        if(playerHealth.curHealth <= 0.0f) {
#if true
            //set scene to load after death to current scene to restart
            FsmString fsmSceneString = FSM.FsmVariables.FindFsmString("toScene");
            fsmSceneString.Value = Application.loadedLevelName;
#else
            //default scene to load when killed is game over
            //save current scene
            UserData.instance.SetString(PlayerActChangeScene.playerLastSaveScene, Application.loadedLevelName);
#endif

            FSM.Fsm.Event(EntityEvent.Kill);
        }
    }

    void OnScenePause() {
        inputEnabled = false;
    }

    void OnSceneResume() {
        inputEnabled = true;
    }

    void OnInputPause(InputManager.Info data) {
        if(data.state == InputManager.State.Pressed) {
            UIModalManager.instance.ModalOpen("options");
        }
    }

    void OnDrawGizmosSelected() {
        if(flowerDistance > 0) {
            Gizmos.color = new Color(1, 0.5f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, flowerDistance);
        }
    }
}
