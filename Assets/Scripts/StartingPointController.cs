using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPointController : MonoBehaviour
{

    public GameObject SpaceshipPrefab;

    [HideInInspector]
    public int NumberOfSpaceships;

    public float MaxCursorTime;

    public Vector2 SpaceshipShootingDirection;

    public Transform ArrowBox;

    private void Awake()
    {
        NumberOfSpaceships = 5;
    }

    private void Start()
    {
        var angle = Mathf.Atan2(SpaceshipShootingDirection.y, SpaceshipShootingDirection.x) * Mathf.Rad2Deg;
        ArrowBox.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    void Update()
    {
     if (Input.GetButtonDown("Jump") && GameManager.Instance.NumberOfSpaceshipsLeft > 0)
        {
            var force = SpaceshipShootingDirection * 100;
            ShootSpaceships(force);
            GameManager.Instance.ShootSpaceShip();
        }
    }

    private void ShootSpaceships(Vector2 force)
    {
        var spaceship = Instantiate(SpaceshipPrefab);
        GameManager.Instance.Spaceships.Add(spaceship);
        spaceship.transform.position = transform.position;
        var rigidbody = spaceship.GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            rigidbody.AddForce(force);
        }
        GameManager.Instance.SpaceshipStart.Play();
        
    }
}
