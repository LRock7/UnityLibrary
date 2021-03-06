﻿#region Header
/* ============================================ 
 *			    Strix Unity Library
 *		https://github.com/strix13/UnityLibrary
 *	============================================ 	
 *	관련 링크 :
 *	
 *	설계자 : 
 *	작성자 : Strix
 *	
 *	기능 : 
   ============================================ */
#endregion Header

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// http://www.tantzygames.com/blog/unity-ugui-button-long-press/
public class CUGUIButton_Press : CObjectBase, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	[SerializeField]
	[Tooltip( "How long must pointer be down on this object to trigger a long press" )]
	private float holdTime = 0.1f;

	// Remove all comment tags (except this one) to handle the onClick event!
	//private bool held = false;
	//public UnityEvent onClick = new UnityEvent();
	
	public UnityEvent p_Event_OnPress_Down = new UnityEvent();
	public UnityEvent p_Event_OnPress_Up = new UnityEvent();

	private bool _bExcute_Up = false;

	public void OnPointerDown( PointerEventData eventData )
	{
		Invoke( "OnLongPress", holdTime );
	}

	public void OnPointerUp( PointerEventData eventData )
	{
		ProcPress( false );
	}

	public void OnPointerExit( PointerEventData eventData )
	{
		ProcPress( false );
	}

	private void OnLongPress()
	{
		if (_bExcute_Up == false)
			_bExcute_Up = true;

		ProcPress( true );
	}


	private void ProcPress( bool bIsPress )
	{
		if (bIsPress)
			p_Event_OnPress_Down.Invoke();
		else if (_bExcute_Up)
		{
			_bExcute_Up = false;

			CancelInvoke( "OnLongPress" );
			p_Event_OnPress_Up.Invoke();
		}
	}
}
