using UnityEngine;
using System.Collections;

using HutongGames.PlayMaker;

public class PlayerActChangeScene : PlayerActSensor {
    public const string playerGameState = "pgame";
    public const string playerGameExState = "pexgame";
    public const string playerLastSaveScene = "plastscene";

    public const string playerFlowerFormat = "pflower_{0}";

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
        if(SceneState.instance != null) {
            return SceneState.instance.GetGlobalValue(string.Format(playerFlowerFormat, ind));
        }
        return 0;
    }

    public static void SetFlowerValue(int ind, int val) {
        if(SceneState.instance != null) {
            SceneState.instance.SetGlobalValue(string.Format(playerFlowerFormat, ind), val, true);
        }
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
