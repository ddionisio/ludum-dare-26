using UnityEngine;
using System.Collections;

public class CollisionPlayerHurt : MonoBehaviour {
    public float hurtAmount = 10.0f;
    public float hurtPeriod = 1.0f;

    private WaitForSeconds mHurtPeriodWait;
    private PlayerHealth mPlayer;

    void Awake() {
        mHurtPeriodWait = new WaitForSeconds(hurtPeriod);
    }

    void OnCollisionEnter(Collision col) {
        foreach(ContactPoint contact in col.contacts) {
            PlayerHealth player = contact.otherCollider.GetComponent<PlayerHealth>();
            if(player != null) {
                player.Hit(hurtAmount);
                mPlayer = player;
            }
        }
    }

    void OnCollisionExit(Collision col) {
        foreach(ContactPoint contact in col.contacts) {
            PlayerHealth player = contact.otherCollider.GetComponent<PlayerHealth>();
            if(player != null && player == mPlayer) {
                StopAllCoroutines();
                mPlayer = null;
            }
        }
    }

    IEnumerator HurtPeriod() {
        while(mPlayer != null) {
            mPlayer.Hit(hurtAmount);
            yield return mHurtPeriodWait;
        }
    }
}
