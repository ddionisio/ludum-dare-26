// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("2D ToolKit/Sprite")]
	[Tooltip("Plays a sprite animation. \nNOTE: The Game Object must have a tk2dAnimatedSprite attached.")]
	public class Tk2dPlayAnimation : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dAnimatedSprite component attached.")]
		[CheckForComponent(typeof(tk2dAnimatedSprite))]
		public FsmOwnerDefault gameObject;
		
		
		//[Tooltip("The anim Lib name. Leave empty to use the one current selected")]
		//public FsmString animLibName;
		
		[RequiredField]
		[Tooltip("The clip name to play")]
		public FsmString clipName;
		
		
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
			//animLibName = null;
			clipName = null;
		}
		
		public override void OnEnter()
		{
			_getSprite();
			
			DoPlayAnimation();		
		}

		void DoPlayAnimation()
		{
			if (_sprite == null)
			{
				LogWarning("Missing tk2dAnimatedSprite component");
				return;
			}
			
			/*if (!animLibName.Value.Equals(""))
			{
				string _animLib = animLibName.Value;
			}*/
			
			
			if (_sprite.Playing == false) {
				
				_sprite.Play (clipName.Value);
			}
		}
	}

}