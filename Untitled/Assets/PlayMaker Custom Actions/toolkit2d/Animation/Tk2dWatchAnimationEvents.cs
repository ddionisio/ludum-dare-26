// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("2D ToolKit/Sprite")]
	[Tooltip("Receive animation events and animation complete event of the current animation playing. \nNOTE: The Game Object must have a tk2dAnimatedSprite attached.")]
	public class Tk2dWatchAnimationEvents : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dAnimatedSprite component attached.")]
		[CheckForComponent(typeof(tk2dAnimatedSprite))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Trigger event defined in the clip. The event holds the following triggers infos: the eventInt, eventInfo and eventFloat properties")]
		public FsmEvent animationTriggerEvent;
		
		[Tooltip("Animation complete event. The event holds the clipId reference")]
		public FsmEvent animationCompleteEvent;
		
		
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
			animationTriggerEvent = null;
			animationCompleteEvent = null;
		}
		
		public override void OnEnter()
		{
			_getSprite();
			
			DoWatchAnimationWithEvents();		
		}

		void DoWatchAnimationWithEvents()
		{
			if (_sprite == null)
			{
				LogWarning("Missing tk2dAnimatedSprite component");
				return;
			}
		
			if (animationTriggerEvent !=null){
				_sprite.animationEventDelegate = AnimationEventDelegate;
			}
			if (animationCompleteEvent !=null){
				_sprite.animationCompleteDelegate = AnimationCompleteDelegate;
			}
		}

		void AnimationEventDelegate (tk2dAnimatedSprite sprite, tk2dSpriteAnimationClip clip, tk2dSpriteAnimationFrame frame, int frameNum)
		{
			Fsm.EventData.IntData = frame.eventInt;
			Fsm.EventData.StringData = frame.eventInfo;
			Fsm.EventData.FloatData = frame.eventFloat;
			Fsm.Event(animationTriggerEvent);
		}

		void AnimationCompleteDelegate (tk2dAnimatedSprite sprite, int clipId)
		{ 
			Fsm.EventData.IntData = clipId;
			Fsm.Event (animationCompleteEvent);      
		}
	}

}