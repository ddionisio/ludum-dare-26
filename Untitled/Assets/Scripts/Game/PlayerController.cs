using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float fallSpeed;

    public float speed;

    public float jumpSpeed;

    private CharacterController mCharCtrl;

    private bool isJump;
    
    void Awake() {
        mCharCtrl = GetComponent<CharacterController>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;

        InputManager input = Main.instance.input;

        Vector3 delta = Vector3.zero;

        delta.x = speed * input.GetAxis(0, InputAction.MoveHorizontal) * dt;
        delta.y = fallSpeed * dt;

        mCharCtrl.Move(delta);
	}

    void OnInputJump(InputManager.Info data) {
    }

    void OnInputAction(InputManager.Info data) {
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        //Debug.Log("hit: " + hit.collider.gameObject.name);
    }
}
