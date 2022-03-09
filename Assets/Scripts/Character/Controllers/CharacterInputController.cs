using System;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    public Action<Vector2> OnCharacterInputStarted { get; set; }
    public Action<Vector2> OnCharacterInputCancelled { get; set; }
    public Action<Vector2> OnCharacterInputPerformed { get; set; }
}