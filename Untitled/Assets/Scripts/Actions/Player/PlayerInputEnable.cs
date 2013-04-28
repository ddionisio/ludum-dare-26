using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("Game")]
public class PlayerInputEnable : M8.PlayMaker.FSMActionComponentBase<PlayerController>
{
    public FsmBool enable;

    public override void Reset() {
        base.Reset();

        enable = null;
    }

	// Code that runs on entering the state.
	public override void OnEnter()
	{
        base.OnEnter();

        mComp.inputEnabled = enable.Value;

		Finish();
	}


}
