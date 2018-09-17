using UnityEngine;
using HutongGames.PlayMaker;

namespace Game.Actions {
    [ActionCategory("Game")]
    [HutongGames.PlayMaker.Tooltip("Add force to motion.")]
    public class MotionAddForce : M8.PlayMaker.FSMActionComponentBase<MotionBase> {
        [HutongGames.PlayMaker.Tooltip("Direction")]
        public FsmVector2 dir;

        [HutongGames.PlayMaker.Tooltip("Amount of force on given direction.")]
        public FsmFloat force;

        public override void Reset() {
            base.Reset();

            dir = Vector2.zero;
            force = 0.0f;
        }

        // Code that runs on entering the state.
        public override void OnEnter() {
            base.OnEnter();

            if(mComp != null) {
                Vector2 f = dir.Value * force.Value;
                mComp.body.AddForce(f.x, f.y, 0.0f);
            }

            Finish();
        }
    }
}