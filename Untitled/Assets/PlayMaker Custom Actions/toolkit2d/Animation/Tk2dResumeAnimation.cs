// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ToolKit 2D")]
	[Tooltip("Resume a sprite animation. Use Tk2dPauseAnimation for dynamic control. \nNOTE: The Game Object must have a tk2dAnimatedSprite attached.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W721")]
	public class Tk2dResumeAnimation : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dAnimatedSprite component attached.")]
		[CheckForComponent(typeof(tk2dAnimatedSprite))]
		public FsmOwnerDefault gameObject;

		
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
		}
		
		public override void OnEnter()
		{
			_getSprite();
			
			DoResumeAnimation();
			
			Finish();
		}

		void DoResumeAnimation()
		{

			if (_sprite == null)
			{
				LogWarning("Missing tk2dAnimatedSprite component");
				return;
			}
			if (_sprite.Paused)
			{
				_sprite.Resume();
			}
		}

	}
}