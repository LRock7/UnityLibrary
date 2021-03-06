﻿#region Header
/* ============================================ 
 *			    Strix Unity Library
 *		https://github.com/strix13/UnityLibrary
 *	============================================ 	
 *	관련 링크 :
 *	
 *	설계자 : 
 *	작성자 : KJH
 *	
 *	기능 : 
   ============================================ */
#endregion Header
#if dUI_DoozyUI
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DoozyUI;

public class CManagerDoozyUI<CLASS, ENUM_UIELEMENT> : CSingletonBase<CLASS>
    where CLASS : CManagerDoozyUI<CLASS, ENUM_UIELEMENT>
    where ENUM_UIELEMENT  : System.IFormattable, System.IConvertible, System.IComparable
{
	/* const & readonly declaration             */

	/* enum & struct declaration                */

	#region Field

	/* public - Field declaration            */

	/* protected - Field declaration         */

	static public string p_strCategory
	{
		get
		{
			if (string.IsNullOrEmpty( _strCategory ))
				_strCategory = typeof( CLASS ).Name;

			return _strCategory;
		}
	}
	

	/* private - Field declaration           */

	private static Dictionary<string, UnityAction> _mapRemoveAnimationsStart = new Dictionary<string, UnityAction>();
	private static Dictionary<string, UnityAction> _mapRemoveAnimationsFinish = new Dictionary<string, UnityAction>();

	//private static Dictionary<string, NavigationPointerData> _mapNavigationData = new Dictionary<string, NavigationPointerData>();

	static private string _strCategory = "";

	#endregion Field

	#region Public

	// ========================================================================== //

	/* public - [Do] Function
     * 외부 객체가 호출(For External class call)*/

	// UIManager 에서 얻어오는건 리스트로 얻어오는데 이건 하나만 얻어오게 한다.
	// Static 정적 멤버라 Awake 때 초기화 해줘야함...
	private static Dictionary<ENUM_UIELEMENT, Component> _mapCachedTypeUIElement = new Dictionary<ENUM_UIELEMENT, Component>();

	public static COMPONENT GetUITypeByElement<COMPONENT>(ENUM_UIELEMENT eUIElementName)
		where COMPONENT : Component
	{
		COMPONENT pCompo = null;

		if (_mapCachedTypeUIElement.ContainsKey(eUIElementName))
			return _mapCachedTypeUIElement[eUIElementName] as COMPONENT;

		return pCompo;
	}

	// Start 이후에 실행되어야 한다.
	public static void DoShowUIElement(ENUM_UIELEMENT eUIElementName, UnityAction OnInAnimationsStart= null, UnityAction OnInAnimationsFinish = null, bool bInstant = false)
	{
		ProcShowUIElement( eUIElementName.ToString(), bInstant, OnInAnimationsStart, OnInAnimationsFinish);
	}

	public static void DoShowUIElementNav(ENUM_UIELEMENT eUIElementName, UnityAction OnInAnimationsStart = null, UnityAction OnInAnimationsFinish = null, bool bInstant = false)
	{
		ProcShowUIElementAndAddNavigation(eUIElementName.ToString(), bInstant, OnInAnimationsStart, OnInAnimationsFinish);
	}

	public static void DoHideUIElement(ENUM_UIELEMENT eUIElementName, UnityAction OnOutAnimationsStart = null, UnityAction OnOutAnimationsFinish = null, bool bInstant = false)
	{
		ProcHideUIElement( eUIElementName.ToString(), bInstant, OnOutAnimationsStart, OnOutAnimationsFinish);
	}

	public static UITYPE DoShowUIElement<UITYPE>(ENUM_UIELEMENT eUIElementName, UnityAction OnInAnimationsStart = null, UnityAction OnInAnimationsFinish = null, bool bInstant = false)
		where UITYPE : Component
	{
		ProcShowUIElement( eUIElementName.ToString(), bInstant, OnInAnimationsStart, OnInAnimationsFinish);
		return GetUITypeByElement<UITYPE>( eUIElementName);
	}

	//public static UITYPE DoShowUIElementNav<UITYPE>(ENUM_UIELEMENT eUIElementName, UnityAction OnInAnimationsStart = null, UnityAction OnInAnimationsFinish = null, bool bInstant = false)
	//	where UITYPE : Component
	//{
	//	ProcShowUIElementAndAddNavigation(eUIElementName.ToString(), bInstant, OnInAnimationsStart, OnInAnimationsFinish);
	//	return GetUITypeByElement<UITYPE>(eUIElementName);
	//}

	/* public - [Event] Function             
       프랜드 객체가 호출(For Friend class call)*/

	#endregion Public

	// ========================================================================== //

	#region Protected

	/* protected - [abstract & virtual]         */

	/* protected - [Event] Function           
       자식 객체가 호출(For Child class call)		*/

	/* protected - Override & Unity API         */

	protected override void OnAwake()
	{
		base.OnAwake();

		_mapCachedTypeUIElement.Clear();

		_mapRemoveAnimationsStart.Clear();
		_mapRemoveAnimationsFinish.Clear();

		ProcRegisterUIElement();

		//_mapNavigationData.Clear();
	}

	#endregion Protected

	// ========================================================================== //

	#region Private

	/* private - [Proc] Function             
       로직을 처리(Process Local logic)           */

	private static void ProcShowUIElement(string strElementName, bool bInstant, UnityAction OnInAnimationsStart, UnityAction OnInAnimationsFinish)
	{
		ProcInitAnimationEvent(strElementName, p_strCategory, Anim.AnimationType.In, OnInAnimationsStart, OnInAnimationsFinish);
		UIManager.ShowUiElement(strElementName, p_strCategory, bInstant);
	}

	private static void ProcHideUIElement(string strElementName, bool bInstant, UnityAction OnOutAnimationsStart, UnityAction OnOutAnimationsFinish)
	{
		//if (_mapNavigationData.ContainsKey(strElementName))
		//{
		//	_mapNavigationData.Remove(strElementName);
		//	UINavigation.RemoveLastItemFromHistory();
		//}

		ProcInitAnimationEvent(strElementName, p_strCategory, Anim.AnimationType.Out, OnOutAnimationsStart, OnOutAnimationsFinish);
		UIManager.HideUiElement(strElementName, p_strCategory, bInstant);
	}

	private static void ProcShowUIElementAndAddNavigation(string strElementName, bool bInstant, UnityAction OnOutAnimationsStart, UnityAction OnOutAnimationsFinish)
	{
		//if (_mapNavigationData.ContainsKey(strElementName)) return;

		//NavigationPointerData pNavData = new NavigationPointerData();
		//NavigationPointer pNavPointer = new NavigationPointer { category = p_strCategory, name = strElementName };
		//pNavData.addToNavigationHistory = false;
		//pNavData.hide.Add(pNavPointer);

		//UINavigation.AddItemToHistory(pNavData);
		//_mapNavigationData.Add(strElementName, pNavData);

		ProcShowUIElement(strElementName, bInstant, OnOutAnimationsStart, OnOutAnimationsFinish);
	}

	private static void ProcInitAnimationEvent(string strUIElementName, string strUICategoryName, Anim.AnimationType eAnimationType, UnityAction OnAnimationsStart, UnityAction OnAnimationsFinish)
	{
		List<UIElement> listUIElement = UIManager.GetUiElements(strUIElementName, strUICategoryName);
		int iCount = listUIElement.Count;
		for (int i = 0; i < iCount; i++)
		{
			UIElement pUIElement = listUIElement[i];

			// DoozyUI UIElement 의 애니메이션 콜백은 이미 있으므로
			// RemoveAllListener 를 해주면 기존 콜백 작동이 안된다.
			// 그래서 수동으로 넣고 임의로 삭제해준다...
			if (_mapRemoveAnimationsStart.ContainsKey(strUIElementName))
			{
				UnityAction onAction = _mapRemoveAnimationsStart[strUIElementName];

				pUIElement.OnInAnimationsStart.RemoveListener(onAction);
				pUIElement.OnOutAnimationsStart.RemoveListener(onAction);
				_mapRemoveAnimationsStart.Remove(strUIElementName);
			}

			if (_mapRemoveAnimationsFinish.ContainsKey(strUIElementName))
			{
				UnityAction onAction = _mapRemoveAnimationsFinish[strUIElementName];

				pUIElement.OnInAnimationsFinish.RemoveListener(onAction);
				pUIElement.OnOutAnimationsFinish.RemoveListener(onAction);
				_mapRemoveAnimationsFinish.Remove(strUIElementName);
			}

			switch (eAnimationType)
			{
				case Anim.AnimationType.In:
					if (OnAnimationsStart != null)
					{
						pUIElement.OnInAnimationsStart.AddListener(OnAnimationsStart);
						_mapRemoveAnimationsStart.Add(strUIElementName, OnAnimationsStart);
					}

					if (OnAnimationsFinish != null)
					{
						pUIElement.OnInAnimationsFinish.AddListener(OnAnimationsFinish);
						_mapRemoveAnimationsFinish.Add(strUIElementName, OnAnimationsFinish);
					}
					break;
				case Anim.AnimationType.Out:
					if (OnAnimationsStart != null)
					{
						pUIElement.OnOutAnimationsStart.AddListener(OnAnimationsStart);
						_mapRemoveAnimationsStart.Add(strUIElementName, OnAnimationsStart);
					}

					if (OnAnimationsFinish != null)
					{
						pUIElement.OnOutAnimationsFinish.AddListener(OnAnimationsFinish);
						_mapRemoveAnimationsFinish.Add(strUIElementName, OnAnimationsFinish);
					}
					break;
			}
		}
	}

	private void ProcRegisterUIElement()
	{
		UIElement[] arrUIElement = GetComponentsInChildren<UIElement>(true);

		int iLen = arrUIElement.Length;
		for (int i = 0; i < iLen; i++)
		{
			UIElement pUIElement = arrUIElement[i];
			CUGUIPanelBase pPanel = pUIElement.GetComponent<CUGUIPanelBase>();

			if (pPanel == null) continue;

			string strUIElementName = pUIElement.elementName;
			ENUM_UIELEMENT eUIElement = strUIElementName.ConvertEnum<ENUM_UIELEMENT>();

			if (_mapCachedTypeUIElement.ContainsKey(eUIElement))
			{
				Debug.LogWarning("이미 _mapCachedTypeUIElement 에 " + eUIElement + " 가 있습니다...");
			}
			else
			{
				_mapCachedTypeUIElement.Add(eUIElement, pPanel as Component);
				pUIElement.RegisterToUIManager();
			}
		}
	}

	/* private - Other[Find, Calculate] Func 
       찾기, 계산등 단순 로직(Simpe logic)         */

	#endregion Private
}
   #endif