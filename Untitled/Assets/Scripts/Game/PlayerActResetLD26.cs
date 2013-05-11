using UnityEngine;
using System.Collections;

public class PlayerActResetLD26 : PlayerActSensor {
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

        SceneState.instance.SetGlobalValue(PlayerActChangeScene.playerGameState, 0, true);
    }

    void Start() {
        activateEnter.SetActive(false);

    }

}
