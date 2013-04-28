using UnityEngine;
using System.Collections;

using HutongGames.PlayMaker;

public class PlayerActChangeScene : PlayerActSensor {
    public string toScene;

    public override void Action(PlayerController ctrl) {
        base.Action(ctrl);

        Player player = ctrl.player;
        FsmString fsmSceneString = player.FSM.FsmVariables.FindFsmString("toScene");
        fsmSceneString.Value = toScene;

        player.FSM.Fsm.Event(EntityEvent.Kill);
    }
}
