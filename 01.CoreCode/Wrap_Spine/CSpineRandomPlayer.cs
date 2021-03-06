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

#if Spine
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Spine;
using Spine.Unity;
using System;

[RequireComponent(typeof(CSpineController))]
public class CSpineRandomPlayer : CObjectBase
{
	/* const & readonly declaration             */

	/* enum & struct declaration                */

	/* public - Field declaration            */

	[System.Serializable]
	public class SAnimation : IRandomItem
	{
		[SpineAnimation]
		public string strAnimation;

		[Range(0, 100f)]
		public int iRandomPercent;
		public float fRandomDuration_Min;
		public float fRandomDuration_Max;

		public int IRandomItem_GetPercent()
		{
			return iRandomPercent;
		}

		public float GetRandomDuration()
		{
			return UnityEngine.Random.Range( fRandomDuration_Min, fRandomDuration_Max );
		}
	}

	/* protected - Field declaration         */

	/* private - Field declaration           */

	[SerializeField][SpineAnimation]
	private string strDefaultAnimationName;

	[SerializeField]
	private List<SAnimation> _listRandomAnimation = new List<SAnimation>();
	private CSpineController _pSpineController;

	private int _iInstanceID;

	// ========================================================================== //

	/* public - [Do] Function
     * 외부 객체가 호출(For External class call)*/

	/* public - [Event] Function             
       프랜드 객체가 호출(For Friend class call)*/

	// ========================================================================== //

	#region Protected

	/* protected - [abstract & virtual]         */

	/* protected - [Event] Function           
       자식 객체가 호출(For Child class call)		*/

	/* protected - Override & Unity API         */

	protected override void OnAwake()
	{
		base.OnAwake();

		GetComponent( out _pSpineController );
		_iInstanceID = GetInstanceID();

		CManagerRandomTable<SAnimation>.instance[_iInstanceID].DoAddRandomItem_Range( _listRandomAnimation );
	}

	protected override void OnEnableObject()
	{
		base.OnEnableObject();

		StartCoroutine( CoUpdateRandomAnimation() );
	}

	#endregion Protected

	// ========================================================================== //

	#region Private

	/* private - [Proc] Function             
       로직을 처리(Process Local logic)           */

	private IEnumerator CoUpdateRandomAnimation()
	{
		if (_listRandomAnimation.Count == 0)
		{
			_pSpineController.DoPlayAnimation_Loop( strDefaultAnimationName );
		}
		else
		{
			while (true)
			{
				SAnimation pCurrentRandomAnimation = CManagerRandomTable<SAnimation>.instance[_iInstanceID].GetRandomItem();
				_pSpineController.DoPlayAnimation( pCurrentRandomAnimation.strAnimation );

				yield return new WaitForSeconds( pCurrentRandomAnimation.GetRandomDuration() );
			}
		}
	}

	/* private - Other[Find, Calculate] Func 
       찾기, 계산등 단순 로직(Simpe logic)         */

	#endregion Private
}
#endif