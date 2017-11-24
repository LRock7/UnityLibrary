﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* ============================================ 
   Editor      : Strix                               
   Date        : 2017-02-21 오후 7:19:05
   Description : 
   Edit Log    : 
   ============================================ */

abstract public class CEffectBase<CLASS_EFFECT, ENUM_EFFECT_NAME> : CObjectBase
	where CLASS_EFFECT : CEffectBase<CLASS_EFFECT, ENUM_EFFECT_NAME>
	where ENUM_EFFECT_NAME : System.IConvertible, System.IComparable
{
	/* const & readonly declaration             */

	/* enum & struct declaration                */
	protected enum EEffectType
	{
		None,
		Particle,
		TwoD,
		Spine,
	}

	/* public - Variable declaration            */

	/* protected - Variable declaration         */

	protected EEffectType _eEffectType = EEffectType.None;

	/* private - Variable declaration           */
	[SerializeField]
	private ENUM_EFFECT_NAME _eEffectName; public ENUM_EFFECT_NAME p_eEffectName { get { return _eEffectName; } set { _eEffectName = value; } }

	private CLASS_EFFECT _pInstance;
	private ParticleSystem _pParticleSystem;
#if NGUI
	private CUI2DSpriteAnimation _p2DSpriteAnimation;
#endif
	private System.Action _OnFinishEffect;

	// ========================================================================== //

	/* public - [Do] Function
     * 외부 객체가 호출                         */

	public void DoPlayEffect( Transform pTransParents )
	{
		_pTransformCached.SetParent( pTransParents );
		Vector3 vecLocalPos = Vector3.zero;
		OnPlayEffectBefore( _eEffectName, ref vecLocalPos );

		_pTransformCached.localPosition = vecLocalPos;
		_pTransformCached.localRotation = Quaternion.identity;

		ProcPlayEffect();

		OnPlayEffectAfter( _eEffectName );
	}

	public void DoPlayEffect( Vector3 V3Targetpos, System.Action OnFinishEffect = null )
	{
		OnPlayEffectBefore( _eEffectName, ref V3Targetpos );

		_OnFinishEffect = OnFinishEffect;
		_pTransformCached.position = V3Targetpos;
		ProcPlayEffect();

		OnPlayEffectAfter( _eEffectName );
	}

	public void DoPlayEffect( ENUM_EFFECT_NAME _eEffectName, Transform pTransParents, Vector3 vecWorldPos )
	{
		_pTransformCached.SetParent( pTransParents );
		OnPlayEffectBefore( _eEffectName, ref vecWorldPos );
		_pTransformCached.position = vecWorldPos;
		ProcPlayEffect();

		OnPlayEffectAfter( _eEffectName );
	}

	/* public - [Event] Function             
       프랜드 객체가 호출                       */

	// ========================================================================== //

	/* protected - [abstract & virtual]         */

	virtual public void OnMakeEffect() { }
	virtual protected void OnPlayEffectBefore( ENUM_EFFECT_NAME eEffectName, ref Vector3 V3Targetpos ) { }
	virtual protected void OnPlayEffectAfter( ENUM_EFFECT_NAME eEffectName ) { }

	virtual protected void OnDefineEffect()
	{
		if (_pParticleSystem != null)
			_eEffectType = EEffectType.Particle;

#if NGUI
		else if (_p2DSpriteAnimation != null)
			_eEffectType = EEffectType.TwoD;
#endif

		else
		{
			_eEffectType = EEffectType.None;
			//if(GetComponent<TrailRenderer>() == null)
			//    Debug.LogWarning("이펙트가 ParticleSystem 또는 2D2priteAnimation을 가지고 있지 않습니다." + name, this);
		}
	}

	virtual protected void ProcPlayEffect()
	{
		gameObject.SetActive( true );
		switch (_eEffectType)
		{
			case EEffectType.Particle:
				StartCoroutine( CoPlayParticleSystem() );
				break;

			case EEffectType.TwoD:
				StartCoroutine( CoPlay2DSpriteAnimation() );
				break;
		}
	}

	/* protected - [Event] Function           
       자식 객체가 호출                         */

	/* protected - Override & Unity API         */
	protected override void OnAwake()
	{
		base.OnAwake();

		_pInstance = this as CLASS_EFFECT;
		_pParticleSystem = GetComponentInChildren<ParticleSystem>();
#if NGUI
		_p2DSpriteAnimation = GetComponentInChildren<CUI2DSpriteAnimation>();
#endif

		OnDefineEffect();
	}

	protected override void OnDisableObject()
	{
		base.OnDisableObject();

		CManagerPooling<ENUM_EFFECT_NAME, CLASS_EFFECT>.instance.DoPush( _pInstance );
	}

	// ========================================================================== //

	/* private - [Proc] Function             
       중요 로직을 처리                         */

	private IEnumerator CoPlayParticleSystem()
	{
		if (_pParticleSystem.main.loop == false)
		{
			_pParticleSystem.Play();
			while (_pParticleSystem.isPlaying)
				yield return null;

			DisableParticleSystem();
		}
	}

	private IEnumerator CoPlay2DSpriteAnimation()
	{
#if NGUI
		if (_p2DSpriteAnimation.frames.Length == 0)
		{
			Debug.LogWarning( "Effect Sprite Frame Length가 0입니다" );
			Disable2DSpriteAnimation();
			yield break;
		}

		if (_p2DSpriteAnimation.loop)
			_p2DSpriteAnimation.Play();
		else
			_p2DSpriteAnimation.Play( Disable2DSpriteAnimation );
#endif
		yield return null;
	}

	private void DisableParticleSystem()
	{
		_pParticleSystem.Stop();

		_pGameObjectCached.SetActive( false );
		ProcOnFinishEffect();
	}

	private void Disable2DSpriteAnimation()
	{
		_pGameObjectCached.SetActive( false );
		ProcOnFinishEffect();
	}

	private void ProcOnFinishEffect()
	{
		if (_OnFinishEffect != null)
		{
			_OnFinishEffect();
			_OnFinishEffect = null;
		}
	}
	/* private - Other[Find, Calculate] Func 
       찾기, 계산 등의 비교적 단순 로직         */

}