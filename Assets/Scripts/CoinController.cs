using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tags.PLAYER_TAG)
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        GameManager.Instance.CollectCoin();
        Destroy(gameObject);
    }
}
