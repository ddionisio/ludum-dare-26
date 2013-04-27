using UnityEngine;
using System.Collections;

public abstract class UnitBaseEntity : EntityBase {
    private ActionTarget mActTarget;

    public ActionTarget actionTarget { get { return mActTarget; } }

    public abstract int flockId { get; }

    protected override void Awake() {
        base.Awake();

        mActTarget = GetComponent<ActionTarget>();
    }

    protected override void ActivatorSleep() {
        base.ActivatorSleep();

        ResetActionTarget();
    }

    protected override void OnDestroy() {
        ResetActionTarget();

        base.OnDestroy();
    }

    private void ResetActionTarget() {
        if(mActTarget != null) {
            mActTarget.StopAction();
            mActTarget.indicatorOn = false;
            mActTarget.lockTargetted = false;
        }
    }
}
