using UnityEngine;
using HutongGames.PlayMaker;

namespace Game.Actions {
    [ActionCategory("Game")]
    [Tooltip("Get the flock unit's move target")]
    public class FlockGetTarget : M8.PlayMaker.FSMActionComponentBase<FlockUnit> {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        public FsmGameObject goVar;

        public FsmBool everyFrame;

        public override void Reset() {
            base.Reset();
            goVar = null;
            everyFrame = false;
        }

        // Code that runs on entering the state.
        public override void OnEnter() {
            base.OnEnter();

            if(mComp != null) {
                goVar.Value = mComp.moveTarget != null ? mComp.moveTarget.gameObject : null;
            }

            if(!everyFrame.Value)
                Finish();
        }

        public override void OnUpdate() {
            base.OnUpdate();

            if(mComp != null) {
                goVar.Value = mComp.moveTarget != null ? mComp.moveTarget.gameObject : null;
            }
        }
    }
}
