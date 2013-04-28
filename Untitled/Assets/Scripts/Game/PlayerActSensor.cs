using UnityEngine;
using System.Collections;

using HutongGames.PlayMaker;

public class PlayerActSensor : SensorSingle<PlayerController> {

    private PlayMakerFSM mFSM;

    void Awake() {
        mFSM = GetComponent<PlayMakerFSM>();
    }

    protected override bool UnitVerify(PlayerController unit) {
        //check if player already has a current act sensor
        return unit.curActSensor == null || unit.curActSensor == this;
    }

    protected override void UnitEnter(PlayerController unit) {
        unit.curActSensor = this;

        //special something for the fsm, do something fancy to indicate readiness
        if(mFSM != null) {
            mFSM.Fsm.Event(EntityEvent.TriggerEnter);
        }
    }

    protected override void UnitExit(PlayerController unit) {
        if(unit.curActSensor == this)
            unit.curActSensor = null;

        if(mFSM != null) {
            mFSM.Fsm.Event(EntityEvent.TriggerExit);
        }
    }

    //called by player controller when they hit action
    public virtual void Action(PlayerController ctrl) {
        if(mFSM != null)
            mFSM.Fsm.Event(EntityEvent.TriggerAct);
    }
}
