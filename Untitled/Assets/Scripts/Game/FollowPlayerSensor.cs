using UnityEngine;
using System.Collections;

public class FollowPlayerSensor : SensorSingle<Player> {
    private FlockUnit mFlockUnit;

    void Awake() {
        mFlockUnit = M8.Util.GetComponentUpwards<FlockUnit>(transform, true);
    }

    protected override bool UnitVerify(Player unit) {
        return mFlockUnit.moveTarget != unit.transform;
	}

    protected override void UnitEnter(Player unit) {
        Transform t = mFlockUnit.transform;
        if(unit.flowers.IndexOf(t) == -1)
            unit.flowers.Add(t);

        mFlockUnit.moveTarget = unit.transform;
    }
}
