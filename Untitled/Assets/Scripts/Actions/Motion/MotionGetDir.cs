using UnityEngine;
using HutongGames.PlayMaker;

namespace Game.Actions {
    [ActionCategory("Game")]
    [HutongGames.PlayMaker.Tooltip("Get the motion direction.")]
    public class MotionGetDir : M8.PlayMaker.FSMActionComponentBase<MotionBase> {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the dir.")]
        public FsmVector2 storeVector;

        public override void Reset() {
            base.Reset();

            storeVector = null;
        }

        // Code that runs on entering the state.
        public override void OnEnter() {
            base.OnEnter();

            if(mComp != null)
                storeVector.Value = mComp.dir;

            Finish();
        }
    }
}