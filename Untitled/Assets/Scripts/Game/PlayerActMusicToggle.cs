using UnityEngine;
using System.Collections;

public class PlayerActMusicToggle : PlayerActSensor {
    public GameObject activateEnter;

    public tk2dTextMesh text;

    public string format = "MUSIC? {0}";

    public string yes = "YES";
    public string no = "NO";

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

        Main.instance.userSettings.isMusicEnable = !Main.instance.userSettings.isMusicEnable;

        RefreshText();
    }

    void Start() {
        activateEnter.SetActive(false);

        RefreshText();
    }

    void RefreshText() {
        text.text = string.Format(format, Main.instance.userSettings.isMusicEnable ? yes : no);
        text.Commit();
    }
}
