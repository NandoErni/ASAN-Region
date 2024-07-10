using UnityEngine;
using System.Collections;

public class HoverForGameObjects : MonoBehaviour
{
    private Vector2 _initialPosition;

    private Vector2 _hoverDestination => new Vector2(_initialPosition.x, _initialPosition.y + HOVER_DISTANCE);

    private Vector2 _targetPosition;

    private const float HOVER_DISTANCE = 8;

    private const int HOVER_TIME = 3;

    void Start()
    {
        _initialPosition = transform.localPosition;
        _targetPosition = _hoverDestination;
    }

    void Update()
    {
        if (Mathf.Abs(transform.localPosition.y - _targetPosition.y) < .1f)
        {
            _targetPosition = _targetPosition == _initialPosition ? _hoverDestination : _initialPosition;
        }

        transform.localPosition = Vector2.LerpUnclamped(transform.localPosition, _targetPosition, Time.deltaTime * HOVER_TIME);
    }
}
