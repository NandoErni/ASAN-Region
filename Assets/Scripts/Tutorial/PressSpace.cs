using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSpace : MonoBehaviour
{
    private Vector2 _targetScale = Vector2.one;


    // Update is called once per frame
    void LateUpdate()
    {
        if (GameManager.Instance.NumberOfSpaceshipsLeft == 0)
        {
            _targetScale = Vector2.zero;
        }

        transform.localScale = Vector2.Lerp(transform.localScale, _targetScale, Time.deltaTime * 8);
    }
}
