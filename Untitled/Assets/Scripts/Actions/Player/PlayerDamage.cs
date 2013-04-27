using UnityEngine;
using HutongGames.PlayMaker;

namespace Game.Actions {
    [ActionCategory("Game")]
    [Tooltip("damage the player")]
    public class PlayerDamage : FsmStateAction {
        [RequiredField]
        public FsmGameObject player;

        public FsmFloat damage;

        public override void Reset() {
            player = null;
            damage = null;
        }

        // Code that runs on entering the state.
        public override void OnEnter() {
            //get player health component
            PlayerHealth playerHealth = player.Value.GetComponent<PlayerHealth>();

            playerHealth.Hit(damage.Value);

            Finish();
        }
    }
}