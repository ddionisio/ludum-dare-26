using UnityEngine;
using System.Collections;

public class PlayerActEndExit : PlayerActSensor {
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

        player.ExitEnding();
    }

    void Start() {
        activateEnter.SetActive(false);
    }
}
