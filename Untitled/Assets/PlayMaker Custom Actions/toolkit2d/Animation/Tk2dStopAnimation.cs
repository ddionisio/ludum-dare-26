// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("2D ToolKit/Sprite")]
	[Tooltip("Stops a sprite animation. \nNOTE: The Game Object must have a tk2dAnimatedSprite attached.")]
	public class Tk2dStopAnimation : FsmStateAction
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
			
			DoStopAnimation();
			
			Finish();
		}

		void DoStopAnimation()
		{

			if (_sprite == null)
			{
				LogWarning("Missing tk2dAnimatedSprite component: " + _sprite.gameObject.name);
				return;
			}

			_sprite.Stop();	
		}

	}
}