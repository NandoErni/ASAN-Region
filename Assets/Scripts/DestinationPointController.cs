using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPointController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var spaceship = collision.GetComponent<SpaceshipController>();

        if (collision.tag == Tags.PLAYER_TAG && spaceship != null)
        {
            spaceship.ArriveAtDestination();
        }
    }
}