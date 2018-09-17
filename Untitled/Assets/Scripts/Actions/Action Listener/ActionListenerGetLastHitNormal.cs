using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace Game.Actions {
    [ActionCategory("Game")]
    [HutongGames.PlayMaker.Tooltip("Get the normal of the collision we last hit. This is updated on each EntityActionHitEnter event.")]
    public class ActionListenerGetLastHitNormal : M8.PlayMaker.FSMActionComponentBase<ActionListener> {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the normal value.")]
        public FsmVector2 storeVector;

        public override void Reset() {
            base.Reset();

            storeVector = null;
        }

        public override void OnEnter() {
            base.OnEnter();

            if(mComp != null)
                storeVector.Value = mComp.lastHitInfo.normal;

            Finish();
        }
    }
}