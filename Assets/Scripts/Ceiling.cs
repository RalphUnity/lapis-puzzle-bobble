using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Ceiling : MonoBehaviour
{
    [SerializeField] private float secondsToMove = 15f;
    
    private float _moveOffset = 1f;

    [HideInInspector] public float offset = 0f;

    // Start is called before the first frame update
    void Start() =>  InvokeRepeating(nameof(MoveDown), secondsToMove, secondsToMove);

    private void MoveDown()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - _moveOffset);
        offset += _moveOffset;
    }
}
