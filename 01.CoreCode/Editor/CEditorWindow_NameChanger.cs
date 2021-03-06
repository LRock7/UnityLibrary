﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

/* ============================================ 
   Editor      : Strix                               
   Date        : 2017-02-04 오후 4:33:17
   Description : 
   Edit Log    : 
   ============================================ */

public class CEditorWindow_NameChanger : EditorWindow
{
	public enum ENameChangeType
	{
		New,
		Replace,
		Append,
		Insert,
	}

    public enum EChangeType
    {
        Number,
        Alphabet,
		Alphabet_Grade,
	}

	public enum EAlphabetGrade
	{
		F,
		D,
		C,
		B,
		A,
		S,
		SS,
		SSS,
	}

	/* const & readonly declaration             */

	/* enum & struct declaration                */

	/* public - Variable declaration            */

	static public CEditorWindow_NameChanger instance;

	/* private - Variable declaration           */
	private List<GameObject> _listGameObject = new List<GameObject>();
	private List<Object> _listObject = new List<Object>();

	private ENameChangeType _eNameChangeType;
	private EChangeType _eFillFormat;

	private string _strNameFormat;
	private string _strReplaceTarget;
	private string _strReplaceDest;
	private int _iStartNum;

	private EAlphabetGrade _eAlphabetGrade = EAlphabetGrade.F;

	// ========================================================================== //

	/* public - [Do] Function
     * 외부 객체가 호출                         */

	[MenuItem("Strix_Tools/NameChanger")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(CEditorWindow_NameChanger));
	}

	/* public - [Event] Function             
       프랜드 객체가 호출                       */

	// ========================================================================== //
	
	/* protected - Override & Unity API         */

	void OnEnable() { instance = this; }
    void OnDisable() { instance = null; }

    void OnGUI()
    {
		GUILayout.BeginHorizontal();
		_eNameChangeType = (ENameChangeType)EditorGUILayout.EnumPopup( "Name Change Type", _eNameChangeType );
		GUILayout.EndHorizontal();

		switch (_eNameChangeType)
		{
			case ENameChangeType.New:
				ProcSettingView_New();
				break;

			case ENameChangeType.Replace:
				ProcSettingView_Replace();
				break;

			case ENameChangeType.Append:
				ProcSettingView_Append();
				break;

			case ENameChangeType.Insert:
				ProcSettingView_Insert();
				break;
		}

		GUILayout.Space(20f);
        if(GUILayout.Button("Change Name!"))
        {
			GameObject[] arrObject = Selection.gameObjects;
			_listGameObject.Clear();
			_listGameObject.AddRange( arrObject );
			_listGameObject.Sort( Comparer_GameObject );

			Object[] arrFile = Selection.objects;
			_listObject.Clear();
			_listObject.AddRange( arrFile );
			_listObject.Sort( Comparer_Object );

			switch(_eNameChangeType)
			{
				case ENameChangeType.New:
					ProcNameChange_New();
					break;

				case ENameChangeType.Replace:
					ProcNameChange_Replace();
					break;

				case ENameChangeType.Append:
					ProcNameChange_Append();
					break;

				case ENameChangeType.Insert:
					ProcNameChange_Insert();
					break;
			}
		}

		GUILayout.Space(20f);
    }

    // ========================================================================== //
	
    /* private - Other[Find, Calculate] Func 
       찾기, 계산 등의 비교적 단순 로직         */

	private void ProcSettingView_New()
	{
		EditorGUILayout.HelpBox( "String.Format 함수의 인자처럼 사용하시면 됩니다.\n 예) : name_{0}", MessageType.Info );
		GUILayout.Space( 20f );

		GUILayout.BeginHorizontal();
		_strNameFormat = EditorGUILayout.TextField( "Name Format", _strNameFormat, GUILayout.Width( 500f ) );
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		_eFillFormat = (EChangeType)EditorGUILayout.EnumPopup( "Fill Format", _eFillFormat );
		GUILayout.EndHorizontal();

		if (_eFillFormat == EChangeType.Number)
		{
			GUILayout.BeginHorizontal();
			_iStartNum = EditorGUILayout.IntField( "StartNum", _iStartNum );
			GUILayout.EndHorizontal();
		}
		else if (_eFillFormat == EChangeType.Alphabet_Grade)
		{
			GUILayout.BeginHorizontal();
			_eAlphabetGrade = (EAlphabetGrade)EditorGUILayout.EnumPopup( "StartGrade", _eAlphabetGrade );
			GUILayout.EndHorizontal();
		}
	}

	private void ProcSettingView_Replace()
	{
		EditorGUILayout.HelpBox( "String.Replace 함수처럼 사용하시면 됩니다.\n", MessageType.Info );
		GUILayout.Space( 20f );

		GUILayout.BeginHorizontal();
		_strReplaceTarget = EditorGUILayout.TextField( "Replace Target", _strReplaceTarget, GUILayout.Width( 500f ) );
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		_strReplaceDest = EditorGUILayout.TextField( "Replace Dest", _strReplaceDest, GUILayout.Width( 500f ) );
		GUILayout.EndHorizontal();
	}

	private void ProcSettingView_Append()
	{
		EditorGUILayout.HelpBox( "이름 끝에 덧붙일 이름을 적어주세요.\n", MessageType.Info );
		GUILayout.Space( 20f );

		GUILayout.BeginHorizontal();
		_strNameFormat = EditorGUILayout.TextField( "Append Name", _strNameFormat, GUILayout.Width( 500f ) );
		GUILayout.EndHorizontal();
	}

	private void ProcSettingView_Insert()
	{
		EditorGUILayout.HelpBox( "이름 앞에 삽입할 이름을 적어주세요.\n", MessageType.Info );
		GUILayout.Space( 20f );

		GUILayout.BeginHorizontal();
		_strNameFormat = EditorGUILayout.TextField( "Insert Name", _strNameFormat, GUILayout.Width( 500f ) );
		GUILayout.EndHorizontal();
	}

	private void ProcNameChange_New()
	{
		int iStartNum = _iStartNum;
		char chAlphabet = 'A';
		EAlphabetGrade eAlphabetGrade = _eAlphabetGrade;

		for (int i = 0; i < _listGameObject.Count; i++)
		{
			if (_eFillFormat == EChangeType.Number)
				_listGameObject[i].name = string.Format( _strNameFormat, iStartNum++ );
			else if (_eFillFormat == EChangeType.Alphabet)
				_listGameObject[i].name = string.Format( _strNameFormat, chAlphabet++ );
			else if (_eFillFormat == EChangeType.Alphabet_Grade)
				_listGameObject[i].name = string.Format( _strNameFormat, eAlphabetGrade++ );
		}

		for (int i = 0; i < _listObject.Count; i++)
		{
			string strAssetPath = AssetDatabase.GetAssetPath( _listObject[i] );
			string strFilePath = Path.Combine( Directory.GetCurrentDirectory(), strAssetPath );
			DirectoryInfo pDirectoryInfo = new DirectoryInfo( strFilePath );
			string strExtension = pDirectoryInfo.Extension;
			pDirectoryInfo = pDirectoryInfo.Parent;
			string strName = "";
			if (_eFillFormat == EChangeType.Number)
				strName = string.Format( _strNameFormat, iStartNum++ );
			else if (_eFillFormat == EChangeType.Alphabet)
				strName = string.Format( _strNameFormat, chAlphabet++ );
			else if (_eFillFormat == EChangeType.Alphabet_Grade)
				strName = string.Format( _strNameFormat, eAlphabetGrade++ );

			string strFilePathNew = Path.Combine( pDirectoryInfo.ToString(), strName );
			strFilePathNew += strExtension;

			AssetDatabase.RenameAsset( strFilePath, strName );
			AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );
			File.Move( strFilePath, strFilePathNew );
		}
	}

	private void ProcNameChange_Replace()
	{
		for (int i = 0; i < _listGameObject.Count; i++)
			_listGameObject[i].name = _listGameObject[i].name.Replace(_strReplaceTarget, _strReplaceDest);

		for (int i = 0; i < _listObject.Count; i++)
		{
			string strAssetPath = AssetDatabase.GetAssetPath( _listObject[i] );
			string strFilePath = Path.Combine( Directory.GetCurrentDirectory(), strAssetPath );
			DirectoryInfo pDirectoryInfo = new DirectoryInfo( strFilePath );

			string strName = pDirectoryInfo.Name.Replace( _strReplaceTarget, _strReplaceDest );
			string strExtension = pDirectoryInfo.Extension;

			pDirectoryInfo = pDirectoryInfo.Parent;
			string strFilePathNew = Path.Combine( pDirectoryInfo.ToString(), strName );

			AssetDatabase.RenameAsset( strFilePath, strName );
			AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );
			File.Move( strFilePath, strFilePathNew );
		}
	}

	private void ProcNameChange_Append()
	{
		for (int i = 0; i < _listGameObject.Count; i++)
			_listGameObject[i].name = _listGameObject[i].name + _strNameFormat;

		for (int i = 0; i < _listObject.Count; i++)
		{
			string strAssetPath = AssetDatabase.GetAssetPath( _listObject[i] );
			string strFilePath = Path.Combine( Directory.GetCurrentDirectory(), strAssetPath );
			DirectoryInfo pDirectoryInfo = new DirectoryInfo( strFilePath );

			string strName = pDirectoryInfo.Name;
			string strExtension = pDirectoryInfo.Extension;
			strName = strName.Remove( (strName.Length - strExtension.Length));
			strName += _strNameFormat + strExtension;

			pDirectoryInfo = pDirectoryInfo.Parent;
			string strFilePathNew = Path.Combine( pDirectoryInfo.ToString(), strName );

			AssetDatabase.RenameAsset( strFilePath, strName );
			AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );
			File.Move( strFilePath, strFilePathNew );
		}
	}

	private void ProcNameChange_Insert()
	{
		for (int i = 0; i < _listGameObject.Count; i++)
			_listGameObject[i].name = _strNameFormat + _listGameObject[i].name;

		for (int i = 0; i < _listObject.Count; i++)
		{
			string strAssetPath = AssetDatabase.GetAssetPath( _listObject[i] );
			string strFilePath = Path.Combine( Directory.GetCurrentDirectory(), strAssetPath );
			DirectoryInfo pDirectoryInfo = new DirectoryInfo( strFilePath );

			string strName = _strNameFormat + pDirectoryInfo.Name;
			string strExtension = pDirectoryInfo.Extension;;

			pDirectoryInfo = pDirectoryInfo.Parent;
			string strFilePathNew = Path.Combine( pDirectoryInfo.ToString(), strName );

			AssetDatabase.RenameAsset( strFilePath, strName );
			AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );
			File.Move( strFilePath, strFilePathNew );
		}
	}

	private int Comparer_GameObject(GameObject pObjectX, GameObject pObjectY)
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

	private int Comparer_Object( Object pObjectX, Object pObjectY )
	{
		return System.String.CompareOrdinal( pObjectX.name, pObjectY.name );
	}
}