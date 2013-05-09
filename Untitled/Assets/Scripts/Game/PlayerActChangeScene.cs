using UnityEngine;
using System.Collections;

using HutongGames.PlayMaker;

public class PlayerActChangeScene : PlayerActSensor {
    public const string playerGameState = "pgame";
    public const string playerGameExState = "pexgame";
    public const string playerLastSaveScene = "plastscene";

    public const string playerFlowerFormat = "pflower_{0}";
    public const string playerTimeFormat = "ptime_{0}";
    public const string playerNumHitFormat = "phit_{0}";

    public string toScene;
    public bool useLastSavedScene; //set to true in game over screen

    public string waypoint;

    public GameObject toggleObject;

    public bool game_2_complete;
    public bool game_3_complete;
    public bool game_4_complete;

    public int game_ex = -1;
    public bool game_ex_save = false; //set to true in exit of levels

    public GameObject flowerCheckObject; //to show how much flowers were collected in game_ex
    public bool flowerCheckUsePlayerData; //show number from pflower_{game_ex} instead
    public int flowerCheckMax = 0;

    public int flowerReq = -1;
    public GameObject flowerReqObject;
    
    private tk2dTextMesh mFlowerCheckText;

    private bool mDoFlowerCheck = false;
    private int mCurNumFlowers = -1;
    private PlayerController mPlayerControl;

    public static int GetFlowerValue(int ind) {
        return UserData.instance.GetInt(string.Format(playerFlowerFormat, ind), 0);
    }

    public static void SetFlowerValue(int ind, int val) {
        UserData.instance.SetInt(string.Format(playerFlowerFormat, ind), val);
    }

    public static float GetTimeValue(int ind) {
        return UserData.instance.GetFloat(string.Format(playerTimeFormat, ind), 9999.0f);
    }

    public static void SetTimeValue(int ind, float val) {
        UserData.instance.SetFloat(string.Format(playerTimeFormat, ind), val);
    }

    public static int GetNumHitValue(int ind) {
        return UserData.instance.GetInt(string.Format(playerNumHitFormat, ind), 999);
    }

    public static void SetNumHitValue(int ind, int val) {
        UserData.instance.SetInt(string.Format(playerNumHitFormat, ind), val);
    }

    protected override void UnitEnter(PlayerController unit) {
        base.UnitEnter(unit);

        if(CheckFlowerReq(unit.player)) {
            if(toggleObject != null)
                toggleObject.SetActive(true);

            if(game_ex > 0) {
                if(flowerCheckObject != null) {
                    flowerCheckObject.SetActive(true);

                    mDoFlowerCheck = true;

                    mCurNumFlowers = -1;
                }
            }
        }
        else if(flowerReqObject != null) {
            flowerReqObject.SetActive(true);
        }

        mPlayerControl = unit;
    }

    protected override void UnitExit(PlayerController unit) {
        base.UnitExit(unit);

        if(toggleObject != null)
            toggleObject.SetActive(false);

        if(flowerReqObject != null) {
            flowerReqObject.SetActive(false);
        }

        if(flowerCheckObject != null) {
            flowerCheckObject.SetActive(false);
        }

        mDoFlowerCheck = false;

        mPlayerControl = null;
    }

    void Start() {
        if(toggleObject != null)
            toggleObject.SetActive(false);

        if(flowerCheckObject != null) {
            tk2dTextMesh[] texts = flowerCheckObject.GetComponentsInChildren<tk2dTextMesh>(true);
            mFlowerCheckText = texts.Length > 0 ? texts[0] : null;
            flowerCheckObject.SetActive(false);
        }

        if(flowerReqObject != null) {
            flowerReqObject.SetActive(false);
        }
    }

    void Update() {
        if(mPlayerControl != null && mDoFlowerCheck) {
            int numFlower;
            if(flowerCheckUsePlayerData) {
                numFlower = GetFlowerValue(game_ex);
            }
            else {
                numFlower = mPlayerControl.player.numFlowers;
            }

            if(numFlower != mCurNumFlowers) {
                if(mFlowerCheckText != null) {
                    mFlowerCheckText.text = string.Format("{0}/{1}", numFlower, flowerCheckMax);
                    mFlowerCheckText.Commit();
                }

                mCurNumFlowers = numFlower;
            }
        }
    }

    public override void Action(PlayerController ctrl) {
        base.Action(ctrl);

        Player player = ctrl.player;

        if(CheckFlowerReq(player)) {
            if(SoundPlayerGlobal.instance != null)
                SoundPlayerGlobal.instance.Play("tele");

            if(game_2_complete) {
                SceneState.instance.SetGlobalFlag(playerGameState, 1, true, true);
            }
            if(game_3_complete) {
                SceneState.instance.SetGlobalFlag(playerGameState, 2, true, true);
            }
            if(game_4_complete) {
                SceneState.instance.SetGlobalFlag(playerGameState, 3, true, true);
            }

            if(game_ex > 0 && game_ex_save) {
                SceneState.instance.SetGlobalFlag(playerGameExState, game_ex, true, true);

                //save flowers acquired if it's more
                int curNumFlowers = player.numFlowers;
                int lastNumFlowers = GetFlowerValue(game_ex);
                if(curNumFlowers > lastNumFlowers)
                    SetFlowerValue(game_ex, curNumFlowers);

                //save time if less
                float curTime = player.curPlayTime;
                float lastTime = GetTimeValue(game_ex);
                if(curTime < lastTime) {
                    SetTimeValue(game_ex, curTime);
                }

                //save hits if less
                int curHits = player.health.numHits;
                int lastHits = GetNumHitValue(game_ex);
                if(curHits < lastHits) {
                    SetNumHitValue(game_ex, curHits);
                }

                Debug.Log("You took " + curTime + " to finish.");
                Debug.Log("Num hits you took: " + curHits);
            }

            //remember where to place the player
            if(PlayerLevelStartPoint.instance != null) {
                PlayerLevelStartPoint.instance.Apply(toScene, waypoint);
            }

            FsmString fsmSceneString = player.FSM.FsmVariables.FindFsmString("toScene");
            fsmSceneString.Value = useLastSavedScene ? UserData.instance.GetString(playerLastSaveScene, toScene) : toScene;

            player.FSM.Fsm.Event(EntityEvent.Kill);
        }
    }

    bool CheckFlowerReq(Player player) {
        if(game_ex > 0 && flowerReq > 0) {
            int numFlowers = player.numFlowers; //get data from player
            return numFlowers >= flowerReq;
        }

        return true;
    }
}
