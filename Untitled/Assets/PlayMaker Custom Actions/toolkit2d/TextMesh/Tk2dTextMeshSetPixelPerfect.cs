// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("2D ToolKit/TextMesh")]
	[Tooltip("Set the pixelPerfect flag of a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	public class Tk2dTextMeshSetPixelPerfect : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[CheckForComponent(typeof(tk2dTextMesh))]
		public FsmOwnerDefault gameObject;
		

		[Tooltip("Does the text needs to be pixelPerfect")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool pixelPerfect;
		
		[Tooltip("Commit changes")]
		[UIHint(UIHint.FsmString)]		
		public FsmBool commit;

		[Tooltip("IF true, and when pixelPerfect is set to true, it calls MakePixelPerfect()")]
		public bool UseMakePixelPerfect;
		
		[ActionSection("")] 
		
		[Tooltip("Repeat every frame.")]
		public bool everyframe;
		
		private tk2dTextMesh _textMesh;
		
		private void _getTextMesh()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_textMesh =  go.GetComponent<tk2dTextMesh>();
		}
		
				
		public override void Reset()
		{
			gameObject = null;
			pixelPerfect = true;
			UseMakePixelPerfect = true;
			commit = true;
			everyframe = false;
		}
		
		public override void OnEnter()
		{
			_getTextMesh();
			
			DoSetPixelPerfect();
			

			if (!everyframe)
			{
				Finish();
			}

		}
		
		public override void OnUpdate()
		{			
			DoSetPixelPerfect();

		}

		void DoSetPixelPerfect()
		{

			if (_textMesh == null)
			{
				LogWarning("Missing tk2dTextMesh component: ");
				return;
			}
			

			if (_textMesh.pixelPerfect != pixelPerfect.Value)
			{
				_textMesh.pixelPerfect = pixelPerfect.Value;
				
				if (pixelPerfect.Value && UseMakePixelPerfect){
					_textMesh.MakePixelPerfect();
				}
				
				if (commit.Value)
				{
					_textMesh.Commit();
				}
				
			}
			
		}

	}
}