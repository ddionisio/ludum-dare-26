using UnityEngine;
using System.Collections;

using HutongGames.PlayMaker;

public class PlayerActChangeScene : PlayerActSensor {
    public string toScene;

    public string waypoint;

    public GameObject toggleObject;

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
