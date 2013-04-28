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

    private bool mInputEnabled = false;

    private bool mActionActive = false;

    private Vector2 mBodyOfsStart;
    private Vector2 mBodyOfsEnd;
    private float mBodyCurTime;

    private WaitForFixedUpdate mWaitUpdate;
    private WaitForSeconds mWaitDelay;

    private Player mPlayer;

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

    public void ResetState() {
        inputEnabled = false;
        curActSensor = null;
        mActionActive = false;

        circleBody.localPosition = Vector3.zero;
        mBodyOfsStart = Vector2.zero;
        mBodyOfsEnd = Vector2.zero;

        actionObject.SetActive(false);

        StopAllCoroutines();
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
    }
    
    protected override void FixedUpdate() {
        if(mInputEnabled) {
            InputManager input = Main.instance.input;

            float moveX = input.GetAxis(0, InputAction.MoveHorizontal);
            float moveY = input.GetAxis(0, InputAction.MoveVertical);

            if(moveX != 0.0f || moveY != 0.0f) {
                body.AddForce(moveX * force, moveY * force, 0.0f);
            }

            mBodyOfsStart = circleBody.localPosition;
            mBodyOfsEnd = new Vector2(moveX * bodyOfsLength, moveY * bodyOfsLength);
            mBodyCurTime = 0.0f;
        }

        base.FixedUpdate();
    }

    void OnInputAction(InputManager.Info data) {
        if(data.state == InputManager.State.Pressed) {
            if(!mActionActive) {
                if(curActSensor != null) {
                    curActSensor.Action(this);
                }

                mActionActive = true;
                StartCoroutine(DoActionIntensify());
            }
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
