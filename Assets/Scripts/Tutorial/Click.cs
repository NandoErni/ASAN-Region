using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{

    private Vector2 _targetScale = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _targetScale = Vector3.one;
        }

        transform.localScale = Vector2.Lerp(transform.localScale, _targetScale, Time.deltaTime * 8);
    }
}
