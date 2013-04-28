using UnityEngine;
using System.Collections;

public class PlayerActSoundToggle : PlayerActSensor {
    public GameObject activateEnter;

    public tk2dTextMesh text;

    public string format = "SOUND? {0}";

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

        Main.instance.userSettings.isSoundEnable = !Main.instance.userSettings.isSoundEnable;

        if(Main.instance.userSettings.isSoundEnable)
            Main.instance.userSettings.volume = 1.0f; //just being safe...

        RefreshText();
    }

    void Start() {
        activateEnter.SetActive(false);

        RefreshText();
    }
        
    void RefreshText() {
        text.text = string.Format(format, Main.instance.userSettings.isSoundEnable ? yes : no);
        text.Commit();
    }
}
