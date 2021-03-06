﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/* ============================================ 
   Editor      : Strix                               
   Date        : 2017-03-27 오전 8:00:42
   Description : 
   Edit Log    : 
   ============================================ */
   
public static class PrimitiveHelper
{
	static public Vector3 Inverse(this Vector3 vecTarget)
	{
		return vecTarget * -1;
	}

	/// <summary>
	/// String Format : "(X.00, Y.00, Z.00)"
	/// </summary>
	/// <param name="strText"></param>
	/// <param name="vecOut"></param>
	/// <returns></returns>
	static public bool TryParse_Vector3(this string strText, out Vector3 vecOut)
	{
		vecOut = Vector3.zero;
		strText = strText.Trim();

		// Parsing X
		int iIndex = strText.IndexOf( "," );
		string strFloat = strText.Substring( 1, iIndex - 1); // (를 뺀다.

		if (float.TryParse( strFloat, out vecOut.x ) == false) return false;
		strText = strText.Substring( iIndex + 1);

		// Parsing Y
		iIndex = strText.IndexOf( "," );
		strFloat = strText.Substring( 0, iIndex );

		if (float.TryParse( strFloat, out vecOut.y ) == false) return false;
		strText = strText.Substring( iIndex + 1);
		strText = strText.Substring( 0, strText.Length - 1 );

		// Parsing Z
		float.TryParse( strText, out vecOut.z );

		return true;
	}

	static public Vector3 ConvertToVector3( this Color pColor )
	{
		return new Vector3( pColor .r, pColor .g, pColor .b);
	}

	static public Color ConvertToColor( this Vector3 sVector )
	{
		return new Color( sVector.x, sVector.y, sVector.z );
	}

	static public Vector3 RandomRange(Vector3 vecMinRange, Vector3 vecMaxRange)
	{
		float fRandX = Random.Range(vecMinRange.x, vecMaxRange.x);
		float fRandY = Random.Range(vecMinRange.y, vecMaxRange.y);
		float fRandZ = Random.Range(vecMinRange.z, vecMaxRange.z);

		return new Vector3(fRandX, fRandY, fRandZ);
	}

	static public Vector2 RandomRange(Vector2 vecMinRange, Vector2 vecMaxRange)
	{
		float fRandX = Random.Range(vecMinRange.x, vecMaxRange.x);
		float fRandY = Random.Range(vecMinRange.y, vecMaxRange.y);

		return new Vector2(fRandX, fRandY);
	}

	static public Vector3 AddFloat(this Vector3 vecOrigin, float fAddValue)
	{
		vecOrigin.x += fAddValue;
		vecOrigin.y += fAddValue;
		vecOrigin.z += fAddValue;

		return vecOrigin;
	}

	public static Vector3 GetPositionByResolution(Vector3 v3Pos)
	{
		return new Vector3(v3Pos.x / Screen.width, v3Pos.y / Screen.height, v3Pos.z);
	}

	public static float GetDistanceByResolution(Vector3 v3PosOne, Vector3 v3PosTwo)
	{
		Vector2 v2Offset = (v3PosOne - v3PosTwo);
		Vector2 v2CalcResolution = new Vector2(v2Offset.x / Screen.width, v2Offset.y / Screen.height);

		return v2CalcResolution.magnitude;
	}

	public static float GetDistanceByResolutionSqrt(Vector3 v3PosOne, Vector3 v3PosTwo)
	{
		float fOffsetOne = Mathf.Abs(v3PosOne.x - v3PosTwo.x) / Screen.width;
		float fOffsetTwo = Mathf.Abs(v3PosOne.y - v3PosTwo.y) / Screen.height;

		float fCalcSqrtDistance = Mathf.Sqrt((fOffsetOne * fOffsetOne) + (fOffsetTwo * fOffsetTwo));

		return fCalcSqrtDistance;
	}

	public static Vector3 GetPlaneRaycastPos(Plane sPlane, Ray sRay)
	{
		float fDistance = 0f;
		bool bSuccess = sPlane.Raycast(sRay, out fDistance);
		if (bSuccess == false)
		{
			Debug.Log( "Plane 과 Ray 교차에 실패했습니다. Plane 의 높이를 확인해주세요.");
			return Vector3.zero;
		}

		Vector3 v3CalcIntersectionPos = (sRay.origin + sRay.direction * fDistance);

		return v3CalcIntersectionPos;
	}

	public static Vector2 GetCenterByResolution()
	{
		return new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
	}

	public static float GetScreenRatio()
	{
		return Screen.width / (float)Screen.height;
	}

	public static Vector3 GetCenterPos(Vector3 v3PosOne, Vector3 v3PosTwo)
	{
		return (v3PosOne - v3PosTwo) * 0.5f;
	}

	static public string CutString(this string strText, char chCut, out string strCut)
	{
		strCut = null;
		int iLength = strText.Length;
		for (int i = 0; i < iLength; i++)
		{
			if (strText[i].Equals(chCut))
			{
				strCut = strText.Substring(0, i);
				strText = strText.Substring(i + 1);
				break;
			}
		}

		return strText;
	}

	static public string CutString(this string strText, char chCut, out int iValue)
	{
		iValue = -1;

		string strTemp;
		strText = strText.CutString(chCut, out strTemp);

		if (strTemp != null)
			int.TryParse(strTemp, out iValue);

		return strText;
	}

	public static string CommaString(this int iValue)
	{
		if (iValue.Equals(0)) return "0";
		return string.Format("{0:#,###}", iValue );
	}

	public static string CommaString(this float fValue)
	{
		if (fValue.Equals( 0f )) return "0";
		return string.Format( "{0:#,###.#}", fValue );
	}

	public static string CommaString(this object pObject)
	{
		return string.Format("{0:#,##0}", pObject);
	}

	private static readonly string[] const_arrPrefix_Ordinal = {
		"th", "st", "nd", "rd", "th", "th", "th", "th", "th", "th"
	};

	private static string ProcGetOrdinalPrefix(int iValue)
	{
		int iNum = Mathf.Abs(iValue);
		int iLastTwoDigits = iNum % 100;
		int iLastDigit = iNum % 10;
		int idx = (iLastTwoDigits >= 11 && iLastTwoDigits <= 13) ? 0 : iLastDigit;

		return const_arrPrefix_Ordinal[idx];
	}

	public static string ToOrdinal(this int iNum)
	{
		return new StringBuilder().Append(iNum).Append(ProcGetOrdinalPrefix(iNum)).ToString();
	}

	static public ENUM[] DoGetEnumArray<ENUM>( int iIndexStart, int iIndexEnd )
	{
		int iLoopIndex = iIndexEnd - iIndexStart;
		if (iIndexStart == 0)
			iLoopIndex += 1;

		ENUM[] arrEnumArray = new ENUM[iLoopIndex];

		for (int i = 0; i < iLoopIndex; i++)
		{
			try
			{
				arrEnumArray[i] = (ENUM)System.Enum.Parse( typeof( ENUM ), string.Format( "{0}{1}", typeof( ENUM).Name, i ) );
			}
			catch
			{
				Debug.LogWarning( typeof( ENUM ).ToString() + " 에 " + string.Format( "{0}{1}", typeof( ENUM ).Name, i ) + "이 존재하지 않습니다." );
				break;
			}
		}

		return arrEnumArray;
	}

	static public T[] GetEnumArray<T>()
		where T : System.IConvertible, System.IComparable
	{
		if (typeof( T ).IsEnum == false)
			throw new System.ArgumentException( "GetValues<T> can only be called for types derived from System.Enum", "T" );

		return (T[])System.Enum.GetValues( typeof( T ) );
	}

	static public void DoShuffleList<Compo>( this List<Compo> list )
	{
		if (list == null)
		{
			Debug.LogWarning( "Shuffle List에서 Shuffle할게 없다.." + typeof( Compo ).Name );
			return;
		}

		for (int i = 0; i < list.Count; i++)
		{
			int RandomIndex = Random.Range( i, list.Count );
			Compo temp = list[RandomIndex];
			list[RandomIndex] = list[i];
			list[i] = temp;
		}
	}

	static public void DoResetTransform( this Transform pTrans )
	{
		pTrans.localPosition = Vector3.zero;
		pTrans.localRotation = Quaternion.identity;
		pTrans.localScale = Vector3.one;
	}

	public static int GetEnumLength<TENUM>()
		where TENUM : System.IConvertible, System.IComparable
	{
		System.Array pArray = GetEnumArray<TENUM>();

		return pArray.Length;
	}

	public static bool CheckIsValidString( string strTarget )
	{
		return strTarget != null && strTarget.Length != 0;
	}

	public static void SetActive(this CObjectBase pObj, bool bEnable)
	{
		if (pObj == null) return;

		if (pObj.p_pGameObjectCached == null)
			pObj.EventOnAwake();

		pObj.p_pGameObjectCached.SetActive(bEnable);
	}

	public static void SetEnableCachedIndex(this CObjectBase[] arrObj, int iCachedLen, int iMaxLen, bool bEnable)
	{
		for (int i = 0; i < iCachedLen; i++)
			arrObj[i].SetActive(i < iMaxLen);
	}

	public static void SetEnableIndex(this CObjectBase[] arrObj, int iMaxLen, bool bEnable)
	{
		int iLen = arrObj.Length;
		for (int i = 0; i < iLen; i++)
		{
			arrObj[i].SetActive(i < iMaxLen);
		}
	}

	public static void SetActiveAll(this CObjectBase[] arrObj, bool bEnable)
	{
		int iLen = arrObj.Length;
		for (int i = 0; i < iLen; i++)
			arrObj[i].SetActive(bEnable);
	}

	public static void PrintArray<ARRAY>(this ARRAY[] arrData)
	{
		StringBuilder pStringBuider = new StringBuilder();

		int iLen = arrData.Length;
		for (int i = 0; i < iLen; i++)
			pStringBuider.Append(i).Append(" : ").Append(arrData[i]).Append("\n");

		Debug.Log( pStringBuider.ToString());
	}

	static Dictionary<System.Type, CDictionary_ForEnumKey<System.Enum, string>> g_mapEnumToString = new Dictionary<System.Type, CDictionary_ForEnumKey<System.Enum, string>>();
	public static string ToString_GarbageSafe( this System.Enum eEnum )
	{
		System.Type pType = eEnum.GetType();
		if (g_mapEnumToString.ContainsKey( pType ) == false)
			g_mapEnumToString.Add( pType, new CDictionary_ForEnumKey<System.Enum, string>() );

		CDictionary_ForEnumKey<System.Enum, string> mapEnumToString = g_mapEnumToString[pType];
		if (mapEnumToString.ContainsKey( eEnum ) == false)
			mapEnumToString.Add( eEnum, System.Enum.GetName( pType, eEnum ) );

		return mapEnumToString[eEnum];
	}

	static public void Sort_ObjectSibilingIndex( this List<GameObject> listpObject )
	{
		listpObject.Sort( Comparer_Object );
	}

	/// <summary>
	/// 이거쓰면 유니티 에디터가 뻗는다.. 왜뻗는지 의문
	/// </summary>
	/// <typeparam name="TComponent"></typeparam>
	/// <param name="listpObject"></param>
	static public void Sort_ObjectSibilingIndex<TComponent>( this List<TComponent> listpObject )
		where TComponent : UnityEngine.Component
	{
		listpObject.Sort( Comparer_Component );
	}

	static public int Comparer_Object( GameObject pObjectX, GameObject pObjectY )
	{
		int iSiblingIndexX = pObjectX.transform.GetSiblingIndex();
		int iSiblingIndexY = pObjectY.transform.GetSiblingIndex();

		if (iSiblingIndexX < iSiblingIndexY)
			return -1;
		else if (iSiblingIndexX > iSiblingIndexY)
			return 1;
		else
			return 0;
	}

	static public int Comparer_Component( Component pObjectX, Component pObjectY )
	{
		int iSiblingIndexX = pObjectX.transform.GetSiblingIndex();
		int iSiblingIndexY = pObjectY.transform.GetSiblingIndex();

		if (iSiblingIndexX < iSiblingIndexY)
			return -1;
		else if (iSiblingIndexX > iSiblingIndexY)
			return 1;
		else
			return 0;
	}

	static public bool GetComponent<COMPONENT>( this UnityEngine.Component pTarget, COMPONENT pComponent )
	where COMPONENT : UnityEngine.Component
	{
		pComponent = pTarget.GetComponent<COMPONENT>();

		return pComponent != null;
	}

	static public int GetNumberDigit_1(this int iTarget)
	{
		return iTarget % 10;
	}

	static public int GetNumberDigit_10( this int iTarget )
	{
		iTarget = iTarget / 10;
		return iTarget % 10;
	}

	static public int GetNumberDigit_100( this int iTarget )
	{
		iTarget = iTarget / 100;
		return iTarget % 10;
	}

	public static float GetPercentage_1(float fCur, float fMax)
	{
		float fCalc = (fCur / fMax);
		if (float.IsNaN(fCalc))
			return 0f;

		return fCalc;
	}

	public static float GetCalcReverseFloat(float fLast, float fCurrent)
	{
		float fCalcReverse = (fLast / (fLast + (fCurrent - fLast)));

		return fCalcReverse;
	}

	private static System.Diagnostics.Stopwatch _pStopwatch = new System.Diagnostics.Stopwatch();
	public static void PerformanceTest(System.Action RunAction, int iStep = 10000, string strTestName = "Test")
	{
		_pStopwatch.Reset();
		_pStopwatch.Start();
		for (int i = 0; i < iStep; i++)
		{
			RunAction();
		}
		_pStopwatch.Stop();

		string strFormat = string.Format("[{0}] 성능 테스트 결과 : {1}", strTestName, _pStopwatch.ElapsedTicks);
		Debug.Log(strFormat);
	}

	static public COMPONENT GetComponentInChildrenOnly<COMPONENT>( this Component pTarget )
	where COMPONENT : UnityEngine.Component
	{
		COMPONENT pFindCompo = null;
		COMPONENT[] arrChildrenCompo = pTarget.GetComponentsInChildren<COMPONENT>();
		for (int i = 0; i < arrChildrenCompo.Length; i++)
		{
			if (arrChildrenCompo[i].transform != pTarget.transform)
			{
				pFindCompo = arrChildrenCompo[i];
				break;
			}
		}

		return pFindCompo;
	}

	static public COMPONENT[] GetComponentsInChildrenOnly<COMPONENT>( this Component pTarget )
		where COMPONENT : UnityEngine.Component
	{
		List<COMPONENT> listComponentChildrenOnly = new List<COMPONENT>();
		COMPONENT[] arrChildrenCompo = pTarget.GetComponentsInChildren<COMPONENT>();
		for (int i = 0; i < arrChildrenCompo.Length; i++)
		{
			if (arrChildrenCompo[i].transform != pTarget.transform)
				listComponentChildrenOnly.Add( arrChildrenCompo[i] );
		}

		return listComponentChildrenOnly.ToArray();
	}

	public enum ETransformSibling
	{
		First,
		Last,
	}

	static public void SetParent_SetSibling( this Transform pTransform, Transform pTransformTarget, ETransformSibling eTransformSibling )
	{
		pTransform.SetParent( pTransformTarget );
		if (eTransformSibling == ETransformSibling.First)
			pTransform.SetAsFirstSibling();
		else if (eTransformSibling == ETransformSibling.Last)
			pTransform.SetAsLastSibling();
	}
}