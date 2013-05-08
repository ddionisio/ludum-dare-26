using UnityEngine;
using System.Collections;

public class PlayerController : MotionBase {
    public Light lightBody;
    public Transform circleBody;

    public float force;

    public float intensifyDelay;

    public float intensityStayDelay;

    public float intensifyMin = 0.5f;
    public float intensifyMax = 8.0f;

    public GameObject actionObject;

    public float bodyOfsLength = 1.0f;
    public float bodyOfsDelay = 0.25f;

    public float actionBoostForce;
    public float actionBoostDelay;

    public float idleThreshold = 1.0f;

    public float helpPopUpDelay = 3.0f;
    public string helpPopUpShowAnim = "show";
    public string helpPopUpHideAnim = "hide";

    private bool mInputEnabled = false;

    private bool mActionActive = false;

    private Vector2 mBodyOfsStart;
    private Vector2 mBodyOfsEnd;
    private float mBodyCurTime;

    private WaitForFixedUpdate mWaitUpdate;
    private WaitForSeconds mWaitDelay;

    private Player mPlayer;

    private Vector2 mCurMove;

    private Vector2 mLastIdlePos;
    private bool mIdleCheck;

    private GameObject mHelpPopUp;
    private bool mHelpPopUpActive = false;
    private bool mHelpPopUpShowDelay = false;

    [System.NonSerialized]
    public PlayerActSensor curActSensor;

    public Player player { get { return mPlayer; } }

    public bool actionActive { get { return mActionActive; } }

    public bool inputEnabled {
        get { return mInputEnabled; }
        set {
            if(mInputEnabled != value) {
                InputManager input = Main.instance != null ? Main.instance.input : null;

                mInputEnabled = value;

                if(input != null) {
                    if(value) {
                        input.AddButtonCall(0, InputAction.Action, OnInputAction);
                    }
                    else {
                        input.RemoveButtonCall(0, InputAction.Action, OnInputAction);
                    }
                }
            }
        }
    }

    public void SpawnStart() {
        if(mHelpPopUp == null)
            mHelpPopUp = GameObject.FindGameObjectWithTag("HelpPopUp");

        HelpPopUpShow();
    }

    public void ResetState() {
        inputEnabled = false;
        curActSensor = null;
        mActionActive = false;
        mIdleCheck = false;

        circleBody.localPosition = Vector3.zero;
        mBodyOfsStart = Vector2.zero;
        mBodyOfsEnd = Vector2.zero;

        actionObject.SetActive(false);

        HelpPopUpCancel();
                
        StopAllCoroutines();
    }

    void OnScenePause() {
        inputEnabled = false;

        HelpPopUpCancel();
    }

    void OnSceneResume() {
        inputEnabled = true;    

        HelpPopUpShow();
    }

    void OnDisable() {
        curActSensor = null;
    }

    protected override void Awake() {
        base.Awake();

        mPlayer = GetComponent<Player>();

        actionObject.SetActive(false);

        mWaitUpdate = new WaitForFixedUpdate();
        mWaitDelay = new WaitForSeconds(intensityStayDelay);
    }

    void Update() {
        if(mBodyOfsStart != mBodyOfsEnd) {
            mBodyCurTime += Time.deltaTime;
            if(mBodyCurTime >= bodyOfsDelay) {
                circleBody.localPosition = mBodyOfsEnd;
                mBodyOfsStart = mBodyOfsEnd;
            }
            else {
                circleBody.localPosition = Vector2.Lerp(mBodyOfsStart, mBodyOfsEnd, mBodyCurTime / bodyOfsDelay);
            }
        }

        if(mIdleCheck) {
            Vector2 delta = transform.position;
            delta -= mLastIdlePos;
            if(delta.sqrMagnitude >= idleThreshold * idleThreshold) {
                mPlayer.health.idleRegen = false;
                mIdleCheck = false;
                //Debug.Log("resume regular regen");
            }
        }
    }

    void HelpPopUpShow() {
        if(!mHelpPopUpActive && mHelpPopUp != null) {
            //Debug.Log("show");
            mHelpPopUpActive = true;
            mHelpPopUp.SetActive(true);

            if(mHelpPopUp.animation != null && !string.IsNullOrEmpty(helpPopUpShowAnim) && !mHelpPopUp.animation.IsPlaying(helpPopUpShowAnim))
                mHelpPopUp.animation.Play(helpPopUpShowAnim, AnimationPlayMode.Queue);
        }
    }

    void HelpPopUpShowDelay() {
        if(!mHelpPopUpActive && !mHelpPopUpShowDelay) {
            //Debug.Log("show delay");
            StartCoroutine("HelpPopUpShowDelayIt");
        }
    }

    void HelpPopUpHide() {
        if(mHelpPopUpActive) {
            if(mHelpPopUp.animation != null && !string.IsNullOrEmpty(helpPopUpHideAnim)) {
                if(!mHelpPopUp.animation.IsPlaying(helpPopUpHideAnim)) {
                    mHelpPopUp.animation.Play(helpPopUpHideAnim, AnimationPlayMode.Queue);
                }
            }
            else if(mHelpPopUp.activeSelf) {
                mHelpPopUp.SetActive(false);
            }

            //Debug.Log("hide deactive");

            mHelpPopUpActive = false;
        }

        if(mHelpPopUpShowDelay) {
            //Debug.Log("hide remove show delay");

            mHelpPopUpShowDelay = false;
            StopCoroutine("HelpPopUpShowDelayIt");
        }
    }

    void HelpPopUpCancel() {
        mHelpPopUpActive = false;

        if(mHelpPopUp != null)
            mHelpPopUp.SetActive(false);

        if(mHelpPopUpShowDelay) {
            mHelpPopUpShowDelay = false;
            StopCoroutine("HelpPopUpShowDelayIt");
        }
    }
    
    protected override void FixedUpdate() {
        if(mInputEnabled) {
            InputManager input = Main.instance.input;

            mCurMove.x = input.GetAxis(0, InputAction.MoveHorizontal);
            mCurMove.y = input.GetAxis(0, InputAction.MoveVertical);

            if(mCurMove.x != 0.0f || mCurMove.y != 0.0f) {
                body.AddForce(mCurMove * force);

                HelpPopUpHide();
            }
            else {
                if(!mIdleCheck) {
                    mPlayer.health.idleRegen = true;
                    mLastIdlePos = transform.position;
                    mIdleCheck = true;

                    //Debug.Log("set to idle regen");
                }

                HelpPopUpShowDelay();
            }

            mBodyOfsStart = circleBody.localPosition;
            mBodyOfsEnd = mCurMove * bodyOfsLength;
            mBodyCurTime = 0.0f;

            if(input.IsDown(0, InputAction.Action) && curActSensor == null && !mActionActive) {
                StopCoroutine("DoBoost");
                                
                mActionActive = true;
                StartCoroutine(DoActionIntensify());

                if(actionBoostDelay > 0.0f && actionBoostForce > 0.0f)
                    StartCoroutine("DoBoost");

                HelpPopUpHide();
            }
        }

        base.FixedUpdate();
    }

    void OnInputAction(InputManager.Info data) {
        if(data.state == InputManager.State.Pressed) {
            if(curActSensor != null) {
                curActSensor.Action(this);
            }
            else {
                if(SoundPlayerGlobal.instance != null)
                    SoundPlayerGlobal.instance.Play("act");
            }

            HelpPopUpHide();
        }
    }

    IEnumerator HelpPopUpShowDelayIt() {
        if(mHelpPopUp != null) {
            mHelpPopUpShowDelay = true;

            while(mHelpPopUp.animation != null && mHelpPopUp.animation.isPlaying)
                yield return new WaitForFixedUpdate();

            yield return new WaitForSeconds(helpPopUpDelay);

            HelpPopUpShow();

            mHelpPopUpShowDelay = false;
        }
        else {
            yield break;
        }
    }

    IEnumerator DoBoost() {
        float curTime = 0.0f;

        //mBoostDelay
        while(curTime < actionBoostDelay) {
            //boost
            if(mCurMove != Vector2.zero) {
                body.AddForce(mCurMove * actionBoostForce);
            }

            curTime += Time.fixedDeltaTime;
            yield return mWaitUpdate;
        }
    }
    
    IEnumerator DoActionIntensify() {
        float curTime = 0.0f;

        actionObject.SetActive(true);
                
        //intensify light
        while(curTime < intensifyDelay) {
            float t = curTime / intensifyDelay;

            lightBody.intensity = intensifyMin + t * (intensifyMax - intensifyMin);

            curTime += Time.fixedDeltaTime;
            yield return mWaitUpdate;
        }

        lightBody.intensity = intensifyMax;

        //wait a bit
        yield return mWaitDelay;

        actionObject.SetActive(false);

        //detensify
        curTime = 0.0f;

        while(curTime < intensifyDelay) {
            float t = curTime / intensifyDelay;

            lightBody.intensity = intensifyMax + t * (intensifyMin - intensifyMax);

            curTime += Time.fixedDeltaTime;
            yield return mWaitUpdate;
        }

        lightBody.intensity = intensifyMin;

        mActionActive = false;

        yield break;
    }
}
