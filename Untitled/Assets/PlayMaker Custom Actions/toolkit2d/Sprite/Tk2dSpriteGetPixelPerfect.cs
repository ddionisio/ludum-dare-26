// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("2D ToolKit/Sprite")]
	[Tooltip("Get the pixel perfect flag of a sprite. \nNOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite)")]
	public class Tk2dSpriteGetPixelPerfect : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite).")]
		[CheckForComponent(typeof(tk2dBaseSprite))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Is the sprite pixelPerfect")]
		[UIHint(UIHint.Variable)]
		public FsmBool pixelPerfect;
		
		[ActionSection("")] 
		
		[Tooltip("Repeat every frame.")]
		public bool everyframe;
		
		private tk2dBaseSprite _sprite;
		
		private void _getSprite()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_sprite =  go.GetComponent<tk2dBaseSprite>();
		}
		
				
		public override void Reset()
		{
			gameObject = null;
			pixelPerfect = null;
			
			everyframe = false;
		}
		
		public override void OnEnter()
		{
			_getSprite();
			
			DoGetSpritePixelPerfect();

			if (!everyframe)
			{
				Finish();
			}

		}
		
		public override void OnUpdate()
		{			
			DoGetSpritePixelPerfect();

		}

		void DoGetSpritePixelPerfect()
		{

			if (_sprite == null)
			{
				LogWarning("Missing tk2dBaseSprite component");
				return;
			}
			
			if (_sprite.pixelPerfect != pixelPerfect.Value)
			{
				 pixelPerfect.Value = _sprite.pixelPerfect;
			}
		}
		
	
	}
}