using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class PlayerActChangeStage : PlayerActSensor {
    public const string stageSceneKey = "pstage";

    public string toScene;

    public int toStage;

    public GameObject activateEnter;
    
    protected override void UnitEnter(PlayerController unit) {
        base.UnitEnter(unit);

        activateEnter.SetActive(true);
    }

    protected override void UnitExit(PlayerController unit) {
        base.UnitExit(unit);

        activateEnter.SetActive(false);
    }

    //called by player controller when they hit action
    public override void Action(PlayerController ctrl) {
        base.Action(ctrl);

        Player player = ctrl.player;

        if(SoundPlayerGlobal.instance != null)
            SoundPlayerGlobal.instance.Play("tele");

        //set stage value
        SceneState.instance.SetGlobalValue(stageSceneKey, toStage, true);

        //remember where to place the player
        if(PlayerLevelStartPoint.instance != null) {
            PlayerLevelStartPoint.instance.Apply(toScene, "");
        }

        FsmString fsmSceneString = player.FSM.FsmVariables.FindFsmString("toScene");
        fsmSceneString.Value = toScene;

        player.FSM.Fsm.Event(EntityEvent.Kill);
    }

    void Start() {
        activateEnter.SetActive(false);
    }
}
