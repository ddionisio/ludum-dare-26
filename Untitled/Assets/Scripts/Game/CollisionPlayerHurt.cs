using UnityEngine;
using System.Collections;

public class CollisionPlayerHurt : MonoBehaviour {
    public float hurtAmount = 10.0f;
    public float hurtPeriod = 1.0f;
    public float pushForce = 0.0f;

    private WaitForSeconds mHurtPeriodWait;
    private bool mHurtPeroidActive = false;

    void Awake() {
        mHurtPeriodWait = new WaitForSeconds(hurtPeriod);
    }

    void OnCollisionEnter(Collision col) {
        foreach(ContactPoint contact in col.contacts) {
            Player player = contact.otherCollider.GetComponent<Player>();
            if(player != null) {
                if(pushForce > 0.0f) {
                    Vector2 dir = player.transform.position - contact.point;
                    dir.Normalize();
                    player.controller.body.AddForce(dir * pushForce);
                }

                if(hurtAmount > 0.0f && !mHurtPeroidActive) {
                    StartCoroutine(HurtPeriod(player));
                }
            }
        }
    }

    IEnumerator HurtPeriod(Player player) {
        mHurtPeroidActive = true;

        player.health.Hit(hurtAmount);

        yield return mHurtPeriodWait;

        mHurtPeroidActive = false;
    }
}
