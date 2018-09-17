// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Sets looping on the AudioSource component on a Game Object.")]
	public class SetAudioLoop : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		public FsmOwnerDefault gameObject;
		public FsmBool loop;

		public override void Reset()
		{
			gameObject = null;
			loop = false;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go != null)
			{
				var audio = go.GetComponent<AudioSource>();
				if (audio != null)
				{
					audio.loop = loop.Value;
				}
			}
			
			Finish();
		}
	}
}