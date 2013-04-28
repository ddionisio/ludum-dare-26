using UnityEngine;
using HutongGames.PlayMaker;

namespace Game.Actions {
    [ActionCategory("Game")]
    [Tooltip("Set certain flags for flock unit")]
    public class FlockSetTarget : M8.PlayMaker.FSMActionComponentBase<FlockUnit> {
        [RequiredField]
        public FsmGameObject go;

        public override void Reset() {
            base.Reset();
            go = null;
        }

        // Code that runs on entering the state.
        public override void OnEnter() {
            base.OnEnter();

            if(mComp != null) {
                mComp.moveTarget = go.Value.transform;
            }

            Finish();
        }
    }
}
