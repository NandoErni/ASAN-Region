using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    private RectTransform _rectTransform;

    private Vector2 _initialPosition;

    private Vector2 _hoverDestination => new Vector2(_initialPosition.x, _initialPosition.y + HOVER_DISTANCE);

    private Vector2 _targetPosition;

    private const float HOVER_DISTANCE = 5;

    private const int HOVER_TIME = 4;

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initialPosition = _rectTransform.position;
        _targetPosition = _hoverDestination;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(_rectTransform.position.y - _targetPosition.y) < .1f)
        {
            _targetPosition = _targetPosition == _initialPosition ? _hoverDestination : _initialPosition;
        }

        _rectTransform.position = Vector2.LerpUnclamped(_rectTransform.position, _targetPosition, Time.deltaTime * HOVER_TIME);
    }
}
