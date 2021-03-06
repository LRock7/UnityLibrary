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
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

//[RequireComponent( typeof( Canvas ))]
public class CUGUIPanelBase : CUGUIObjectBase, IUIPanel
{
	/* const & readonly declaration             */

	/* enum & struct declaration                */

	#region Field

	/* public - Field declaration            */

	[SerializeField]
	private bool _bIsAlwaysShow;
	[SerializeField]
	private bool _bIsFixedSortOrder;
	
	bool IUIPanel.p_bIsAlwaysShow
	{
		get
		{
			return _bIsAlwaysShow;
		}
	}

	bool IUIPanel.p_bIsFixedSortOrder
	{
		get
		{
			return _bIsFixedSortOrder;
		}
	}

	/* protected - Field declaration         */

	/* private - Field declaration           */

	#endregion Field

	#region Public

	// ========================================================================== //

	/* public - [Do] Function
     * 외부 객체가 호출(For External class call)*/

	public void IUIPanel_SetOrder( int iSortOrder )
	{
		transform.SetSiblingIndex( iSortOrder );
	}

	public IEnumerator IUIPanel_OnShowPanel_PlayingAnimation( int iSortOrder )
	{
		yield return StartCoroutine( OnShowPanel_PlayingAnimation( iSortOrder ) );
	}

	public IEnumerator IUIPanel_OnHidePanel_PlayingAnimation()
	{
		yield return StartCoroutine( OnHidePanel_PlayingAnimation() );
	}
	
	/* public - [Event] Function             
       프랜드 객체가 호출(For Friend class call)*/

	#endregion Public

	// ========================================================================== //

	#region Protected

	/* protected - [abstract & virtual]         */

	virtual protected IEnumerator OnShowPanel_PlayingAnimation( int iSortOrder ) { yield return null; }
	virtual protected IEnumerator OnHidePanel_PlayingAnimation() { yield return null; }

	/* protected - [Event] Function           
       자식 객체가 호출(For Child class call)		*/

	/* protected - Override & Unity API         */

	#endregion Protected

	// ========================================================================== //

	#region Private

	/* private - [Proc] Function             
       로직을 처리(Process Local logic)           */

	/* private - Other[Find, Calculate] Func 
       찾기, 계산등 단순 로직(Simpe logic)         */

	#endregion Private
}
