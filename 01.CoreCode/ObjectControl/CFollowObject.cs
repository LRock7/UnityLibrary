﻿using UnityEngine;
using System.Collections;

public class CFollowObject : CObjectBase
{
    public enum EFollowPos
    {
        All,
        X,
        XY,
        XZ,
        Y,
        YZ,
        Z
    }

    public enum EFollowMode
    {
        ControlOutside,
        FixedUpdate,
        Update,   
    }

    [SerializeField]
    private EFollowPos _eFollowPos = EFollowPos.All;
    [SerializeField]
    private Transform _pTransTarget = null;
    [SerializeField]
    private EFollowMode _bUseFixedUpdate = EFollowMode.Update;

    [SerializeField]
    private float _fShakeMinusDelta = 0.1f;

    private Vector3 _vecOriginPos;
    private Vector3 _vecTargetOffset;
    private float _fRemainShakePow;

    private bool _bFollowX;
    private bool _bFollowY;
    private bool _bFollowZ;

    // ========================== [ Division ] ========================== //

    public void DoShakeObject(float fShakePow)
    {
        if(_fRemainShakePow <= 0f)
        {
            //Debug.Log("Shake Start CurrentPos : "  + _pTransformCashed.position + " Offset : " + _vecTargetOffset);
            _vecOriginPos = _pTransformCached.position;
        }

        _fRemainShakePow = fShakePow;
    }

    public void DoInitTarget(Transform pTarget)
    {
        _pTransTarget = pTarget;
        _vecTargetOffset = _pTransTarget.position - _pTransformCached.position;
        
        _bFollowX = _eFollowPos == EFollowPos.All || _eFollowPos == EFollowPos.X || _eFollowPos == EFollowPos.XY || _eFollowPos == EFollowPos.XZ;
        _bFollowY = _eFollowPos == EFollowPos.All || _eFollowPos == EFollowPos.Y || _eFollowPos == EFollowPos.XY || _eFollowPos == EFollowPos.YZ;
        _bFollowZ = _eFollowPos == EFollowPos.All || _eFollowPos == EFollowPos.Z || _eFollowPos == EFollowPos.XZ || _eFollowPos == EFollowPos.YZ;
    }

	public void DoRemoveTarget()
	{
		_pTransTarget = null;
	}

    public void DoUpdateFollow()
    {
        if (_pTransTarget != null)
        {
            Vector3 vecFollowPos = _pTransformCached.position;
            Vector3 vecTargetPos = _pTransTarget.position;

            if (_eFollowPos != EFollowPos.All)
            {
                if (_bFollowX)
                    vecFollowPos.x = vecTargetPos.x - _vecTargetOffset.x;

                if (_bFollowY)
                    vecFollowPos.y = vecTargetPos.y - _vecTargetOffset.y;

                if (_bFollowZ)
                    vecFollowPos.z = vecTargetPos.z - _vecTargetOffset.z;
            }
            else
                vecFollowPos = vecTargetPos - _vecTargetOffset;


            if (_fRemainShakePow > 0f)
            {
                _fRemainShakePow -= _fShakeMinusDelta;
                if (_fRemainShakePow <= 0f)
                {
                    if (_bFollowX == false)
                        vecFollowPos.x = _vecOriginPos.x;

                    if (_bFollowY == false)
                        vecFollowPos.y = _vecOriginPos.y;

                    if (_bFollowZ == false)
                        vecFollowPos.z = _vecOriginPos.z;
                }
                else
                {
                    Vector3 vecShakePos = PrimitiveHelper.RandomRange(vecFollowPos.AddFloat(-_fRemainShakePow), vecFollowPos.AddFloat(_fRemainShakePow));

                    if (_bFollowX) vecShakePos.x = vecFollowPos.x;
                    if (_bFollowY) vecShakePos.y = vecFollowPos.y;
                    if (_bFollowZ) vecShakePos.z = vecFollowPos.z;

                    vecFollowPos = vecShakePos;
                }
            }
            
            _pTransformCached.position = vecFollowPos;
        }
    }

    // ========================== [ Division ] ========================== //

    protected override void OnAwake()
    {
        base.OnAwake();

        if(_pTransTarget != null)
        {
            DoInitTarget(_pTransTarget);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (_bUseFixedUpdate == EFollowMode.Update)
            DoUpdateFollow();
    }

    private void FixedUpdate()
    {
        if (_bUseFixedUpdate == EFollowMode.FixedUpdate)
            DoUpdateFollow();
    }

    // ========================== [ Division ] ========================== //
}