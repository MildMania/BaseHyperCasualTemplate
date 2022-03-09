using System;
using System.Collections.Generic;
using MVVM;
using UnityEngine;

public class CharacterJoystickInputController : CharacterInputController,
    MoveInputManager.IJoystickInputBound
{
    private void Awake()
    {
        RegisterToPhases();
    }

    private void OnDestroy()
    {
        UnregisterFromPhases();
    }

    private void RegisterToPhases()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseTraverseStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnPhaseTraverseFinished;
    }

    private void UnregisterFromPhases()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseTraverseStarted;
        PhaseBaseNode.OnTraverseFinished_Static -= OnPhaseTraverseFinished;
    }

    private void OnPhaseTraverseStarted(PhaseBaseNode phase)
    {
        if (!(phase is GamePhase))
            return;

        RegisterToInputReceiver();
    }

    private void OnPhaseTraverseFinished(PhaseBaseNode phase)
    {
        if (!(phase is GamePhase))
            return;

        UnregisterFromInputReceiver();
    }

    private void RegisterToInputReceiver()
    {
        MoveInputManager.Instance.RegisterInputReceiver(this);
    }

    private void UnregisterFromInputReceiver()
    {
        MoveInputManager.Instance.UnregisterInputReceiver(this);
    }

    public void OnInputStarted(Vector2 input)
    {
        OnCharacterInputStarted?.Invoke(input);
    }

    public void OnInputPerformed(Vector2 input)
    {
        OnCharacterInputPerformed?.Invoke(input);
    }

    public void OnInputCancelled(Vector2 input)
    {
        OnCharacterInputCancelled?.Invoke(input);
    }
}