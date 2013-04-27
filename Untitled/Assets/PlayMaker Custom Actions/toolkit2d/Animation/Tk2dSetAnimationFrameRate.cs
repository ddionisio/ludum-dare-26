// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("2D ToolKit/Sprite")]
	[Tooltip("Set the current clip frames per seconds on a animated sprite. \nNOTE: The Game Object must have a tk2dAnimatedSprite attached.")]
	public class Tk2dSetAnimationFrameRate : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dAnimatedSprite component attached.")]
		[CheckForComponent(typeof(tk2dAnimatedSprite))]
		public FsmOwnerDefault gameObject;
		
		
		[RequiredField]
		[Tooltip("The frame per seconds of the current clip")]
		public FsmFloat framePerSeconds;
		
		[Tooltip("Repeat every Frame")]
		public bool everyFrame;
		
		private tk2dAnimatedSprite _sprite;
		
		private void _getSprite()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_sprite =  go.GetComponent<tk2dAnimatedSprite>();
		}
		
				
		public override void Reset()
		{
			gameObject = null;
			framePerSeconds = 30;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			_getSprite();
			
			
			DoSetAnimationFPS();	
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoSetAnimationFPS();
		}
		void DoSetAnimationFPS()
		{
			if (_sprite == null)
			{
				LogWarning("Missing tk2dAnimatedSprite component");
				return;
			}
			
			_sprite.CurrentClip.fps = framePerSeconds.Value;
		}
	}

}