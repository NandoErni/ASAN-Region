using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipsLeftUI : MonoBehaviour
{

    public Sprite[] Sprites;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.NumberOfSpaceshipsLeft == 0 || GameManager.Instance.NumberOfSpaceshipsLeft > 5)
        {
            _spriteRenderer.sprite = null;

        } else
        {

            _spriteRenderer.sprite = Sprites[GameManager.Instance.NumberOfSpaceshipsLeft - 1];
        }
    }
}
