using UnityEngine;
using System.Collections;

using HutongGames.PlayMaker;

public class PlayerActChangeScene : PlayerActSensor {
    public const string playerGameState = "pgame";

    public string toScene;

    public string waypoint;

    public GameObject toggleObject;

    public bool game_2_complete;
    public bool game_3_complete;
    public bool game_4_complete;

    protected override void UnitEnter(PlayerController unit) {
        base.UnitEnter(unit);

        if(toggleObject != null)
            toggleObject.SetActive(true);
    }

    protected override void UnitExit(PlayerController unit) {
        base.UnitExit(unit);

        if(toggleObject != null)
            toggleObject.SetActive(false);
    }

    void Start() {
        if(toggleObject != null)
            toggleObject.SetActive(false);
    }

    public override void Action(PlayerController ctrl) {
        base.Action(ctrl);

        if(game_2_complete) {
            SceneState.instance.SetGlobalFlag(playerGameState, 1, true, true);
        }
        if(game_3_complete) {
            SceneState.instance.SetGlobalFlag(playerGameState, 2, true, true);
        }
        if(game_4_complete) {
            SceneState.instance.SetGlobalFlag(playerGameState, 3, true, true);
        }

        //remember where to place the player
        if(PlayerLevelStartPoint.instance != null) {
            PlayerLevelStartPoint.instance.Apply(toScene, waypoint);
        }

        Player player = ctrl.player;
        FsmString fsmSceneString = player.FSM.FsmVariables.FindFsmString("toScene");
        fsmSceneString.Value = toScene;

        player.FSM.Fsm.Event(EntityEvent.Kill);
    }
}
