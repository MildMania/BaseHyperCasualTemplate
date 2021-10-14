using System;
using MMFramework_2._0.PhaseSystem.Core.EventListener;
using UnityEngine;
using EState = CharacterFSMController.EState;
using ETransition = CharacterFSMController.ETransition;
using MMUtils = MMFramework.Utilities.Utilities;

public class CharacterRunState : State<EState, ETransition>
{
    [SerializeField] private CharacterAnimationController _characterAnimationController = null;

    [SerializeField] private CharacterInputController _characterInputController = null;

    [SerializeField] private CharacterMovementBehaviour _characterMovementBehaviour = null;

    [SerializeField] private Transform _characterTransform = null;

    [SerializeField] private float _speed = 2.0f;


    [SerializeField] private float _sidewaysMoveSpeed = 5f;

    [SerializeField] private float _sidewaysDeltaMultiplier = 2f;

    [SerializeField] private float _additionalYSpeedAmount = -1f;

    private float _curSidewaysMoveSliderVal = 0;

    #region Events

    public Action<Vector3> OnCharacterMoved { get; set; }

    #endregion

    protected override EState GetStateID()
    {
        return EState.Run;
    }

    public override void OnEnterCustomActions()
    {
        _characterAnimationController.PlayAnimation(ECharacterAnimation.Run);

        base.OnEnterCustomActions();
    }

    private void Awake()
    {
        SubscribeToEvents();
    }

    private void Update()
    {
        TryMove();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    [PhaseListener(typeof(GamePhase), true)]
    private void SubscribeToEvents()
    {
        RegisterToInputController();
    }

    [PhaseListener(typeof(GamePhase), false)]
    private void UnsubscribeFromEvents()
    {
        UnregisterFromInputController();
    }


    #region Sideways Input

    private void RegisterToInputController()
    {
        _characterInputController.OnCharacterInputStarted += OnInputStarted;
        _characterInputController.OnCharacterInputPerformed += OnInputPerformed;
        _characterInputController.OnCharacterInputCancelled += OnInputCancelled;
    }

    private void UnregisterFromInputController()
    {
        _characterInputController.OnCharacterInputStarted -= OnInputStarted;
        _characterInputController.OnCharacterInputPerformed -= OnInputPerformed;
        _characterInputController.OnCharacterInputCancelled -= OnInputCancelled;
    }

    private void OnInputStarted(Vector2 delta)
    {
        FSM.SetTransition(ETransition.Run);
    }

    private void OnInputPerformed(Vector2 delta)
    {
        if (!FSM.GetCurStateID().Equals(GetStateID()))
        {
            FSM.SetTransition(ETransition.Run);
        }

        _curSidewaysMoveSliderVal += delta.x * _sidewaysDeltaMultiplier;

        _curSidewaysMoveSliderVal = Mathf.Clamp(_curSidewaysMoveSliderVal,
            LevelBoundaryProvider.Instance.GetLeftBoundary().x,
            LevelBoundaryProvider.Instance.GetRightBoundary().x);
    }

    private void OnInputCancelled(Vector2 delta)
    {
    }

    #endregion

    private bool TryMove()
    {
        if (!CanMove())
        {
            return false;
        }

        Vector3 sideWayDir = _characterTransform.right * (_curSidewaysMoveSliderVal - _characterTransform.position.x);

        _characterMovementBehaviour.Move(_characterTransform.forward * _speed +
                                         sideWayDir * _sidewaysMoveSpeed + new Vector3(0, _additionalYSpeedAmount, 0));

        OnCharacterMoved?.Invoke(_characterTransform.position);

        return true;
    }

    private bool CanMove()
    {
        return FSM.GetCurStateID().Equals(GetStateID());
    }
}