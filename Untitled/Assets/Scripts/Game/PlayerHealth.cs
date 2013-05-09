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

    public float minTileDeathRes = 4;
    public float minTileDeathDelay = 1.0f;

    public float regenDelay = 1.0f;
    public float regenRate = 5.0f; //per second
    public float regenRateIdle = 0.1f;

    public float hurtDelay = 1.0f;

    public event OnHit hitCallback;

    private float mCurHealth = 0.0f;

    private float mCurTime;

    private State mCurState = State.None;

    private M8.ImageEffects.Tile mTiler;

    private UIHealth mHealthUI;

    private int mNumHits = 0;

    [System.NonSerialized]
    public bool idleRegen = false;

    public float curHealth { get { return mCurHealth; } }
    public State curState { get { return mCurState; } }
    public int numHits { get { return mNumHits; } }

    public void ResetStats() {
        mCurHealth = maxHealth;

        SetState(State.None);

        idleRegen = false;

        mNumHits = 0;
    }

    public void Hit(float amt) {
        if(amt != 0.0f) {
            mCurHealth -= amt;

            if(mCurHealth <= 0.0f) {
                mCurHealth = 0.0f;

                if(mCurState != State.Dead) {
                    SetState(State.Dead);

                    if(hitCallback != null)
                        hitCallback(this);
                }
            }
            else {
                SetState(State.RegenWait);

                if(hitCallback != null)
                    hitCallback(this);

                if(SoundPlayerGlobal.instance != null)
                    SoundPlayerGlobal.instance.Play("hurt");

                mNumHits++;
            }

            ApplyHealthText();
        }
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
        //bad
        if(mHealthUI == null) {
            if(UIModalManager.instance != null) {
                mHealthUI = UIModalManager.instance.transform.parent.GetComponentInChildren<UIHealth>();
                if(mHealthUI != null) {
                    mHealthUI.Attach(transform);
                    mHealthUI.gameObject.SetActive(mCurHealth < maxHealth);
                }
            }
        }

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
                mCurHealth += (idleRegen ? regenRateIdle : regenRate) * Time.deltaTime;
                if(mCurHealth >= maxHealth) {
                    mCurHealth = maxHealth;
                    SetState(State.None);
                }
                else {
                    if(mTiler != null) {
                        mTiler.numTiles = minTileRes + (maxTileRes - minTileRes) * (mCurHealth / maxHealth);
                    }

                    ApplyHealthText();
                }
                break;

            case State.Dead:
                if(mTiler != null) {
                    mCurTime += Time.deltaTime;
                    if(mCurTime < minTileDeathDelay) {
                        mTiler.numTiles = minTileRes + (mCurTime / minTileDeathDelay) * (minTileDeathRes - minTileRes);
                    }
                    else {
                        mTiler.numTiles = minTileDeathRes;
                        mTiler = null;
                    }
                }
                break;
        }
    }

    private void ApplyHealthText() {
        if(mHealthUI != null) {
            mHealthUI.label.text = Mathf.RoundToInt((mCurHealth / maxHealth) * 100.0f).ToString() + "%";
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
            }
        }

        if(mHealthUI != null) {
            mHealthUI.gameObject.SetActive(mCurHealth < maxHealth);
        }
                
        switch(newState) {
            case State.RegenWait:
                if(mTiler != null) {
                    mTiler.numTiles = minTileRes + (maxTileRes - minTileRes) * (mCurHealth / maxHealth);
                }

                mCurTime = 0.0f;

                StopAllCoroutines();
                StartCoroutine(HurtMe());
                break;

            case State.Regen:
                break;

            case State.Dead:
                StopAllCoroutines();

                if(mTiler != null) {
                    mTiler.enabled = true;
                    mTiler.numTiles = minTileRes;
                }

                mCurTime = 0.0f;
                break;
        }
    }

    IEnumerator HurtMe() {
        if(mTiler != null) {
            mTiler.enabled = true;

            float curTime = 0.0f;

            while(curTime < hurtDelay) {
                curTime += Time.fixedDeltaTime;

                float t = Mathf.Sin(M8.MathUtil.TwoPI * (curTime / hurtDelay));

                mTiler.numTiles = maxTileRes + t * (minTileRes - maxTileRes);

                yield return new WaitForFixedUpdate();
            }

            mTiler.enabled = false;
        }

        yield break;
    }
}
