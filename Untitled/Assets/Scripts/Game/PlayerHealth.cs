using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    public enum State {
        None,
        RegenWait,
        Regen,
        Dead
    }

    public delegate void OnHit(PlayerHealth playerHealth);

    public float maxHealth = 100.0f;

    public float maxTileRes = 1024;
    public float minTileRes = 4;

    public float regenDelay = 1.0f;
    public float regenRate = 5.0f; //per second

    public event OnHit hitCallback;

    private float mCurHealth = 0.0f;

    private float mCurTime;

    private State mCurState = State.None;

    private M8.ImageEffects.Tile mTiler;

    public float curHealth { get { return mCurHealth; } }
    public State curState { get { return mCurState; } }

    public void ResetStats() {
        mCurHealth = maxHealth;

        SetState(State.None);
    }

    public void Hit(float amt) {
        mCurHealth -= amt;
        if(mCurHealth <= 0.0f) {
            mCurHealth = 0.0f;

            SetState(State.Dead);
        }
        else {
            SetState(State.RegenWait);
        }

        if(hitCallback != null)
            hitCallback(this);
    }

    void OnDestroy() {
        ResetStats();

        hitCallback = null;
    }

    void Awake() {
        ResetStats();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        switch(mCurState) {
            case State.None:
                break;

            case State.RegenWait:
                mCurTime += Time.deltaTime;
                if(mCurTime >= regenDelay) {
                    SetState(State.Regen);
                }
                break;

            case State.Regen:
                mCurHealth += regenRate * Time.deltaTime;
                if(mCurHealth >= maxHealth) {
                    mCurHealth = maxHealth;
                    SetState(State.None);
                }
                else if(mTiler != null) {
                    mTiler.numTiles = minTileRes + (maxTileRes - minTileRes) * (mCurHealth / maxHealth);
                }
                break;
        }
    }

    private void SetState(State newState) {
        //undo previous stuff

        //new stuff
        mCurState = newState;

        if(mCurState == State.None) {
            if(mTiler != null) {
                mTiler.enabled = false;
                mTiler = null;
            }
        }
        else if(mTiler == null) {
            CameraController cc = CameraController.instance;
            if(cc != null) {
                mTiler = cc.mainCamera.GetComponent<M8.ImageEffects.Tile>();
                mTiler.enabled = true;
            }
        }
                
        switch(newState) {
            case State.RegenWait:
                if(mTiler != null) {
                    mTiler.numTiles = minTileRes + (maxTileRes - minTileRes) * (mCurHealth / maxHealth);
                }

                mCurTime = 0.0f;
                break;

            case State.Regen:
                break;

            case State.Dead:
                if(mTiler != null) {
                    mTiler.numTiles = minTileRes;
                }
                break;
        }
    }
}
