using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CursorController : MonoBehaviour
{
    [HideInInspector]
    public Camera Camera;

    private Vector3 SMALL_SCALE = new Vector3(7, 7, 1);
    private Vector3 BIG_SCALE = new Vector3(20, 20, 1);
    private const int NUMBER_OF_POINTS_FOR_LINE = 300;
    private const float MASS = 3000;

    private const float SCALE_SPEED = 10f;
    private LineRenderer _line;

    void Start()
    {
        Camera = Camera.main;
        transform.localScale = SMALL_SCALE;
    }

    public void InitCircle()
    {
        _line = GetComponent<LineRenderer>();
        _line.useWorldSpace = false;
        _line.colorGradient = new Gradient();
        _line.widthMultiplier = .5f;
        CreatePointsForCircle(NUMBER_OF_POINTS_FOR_LINE + 1);
    }

    void Update()
    {
        var cameraPos = Camera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(cameraPos.x, cameraPos.y, 0);

        if (GameManager.Instance.IsActivatingCursor)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, BIG_SCALE, Time.deltaTime * SCALE_SPEED );
        } else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, SMALL_SCALE, Time.deltaTime * SCALE_SPEED);
        }

        if (GameManager.Instance.CursorTimeLeft > 0 && _line != null)
        {
            UpdateCursorTimeLinePositions();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsActivatingCursor)
        {
            InfluenceSpaceships();
        }
    }

    private void UpdateCursorTimeLinePositions()
    {
        var cursorTimeInPercent = GameManager.Instance.CursorTimeLeft / GameManager.Instance.MaxCursorTime;

        _line.positionCount = (int)(NUMBER_OF_POINTS_FOR_LINE * cursorTimeInPercent);
    }

    private void InfluenceSpaceships()
    {
        Vector3 mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);

        foreach (var spaceship in GameManager.Instance.Spaceships)
        {
            var rigidBody = spaceship.GetComponent<Rigidbody2D>();
            var direction = mousePos - spaceship.transform.position;
            var force = direction.normalized * MASS / (direction.magnitude * direction.magnitude);
            rigidBody.AddForce(force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tags.PLAYER_TAG)
        {
            var spaceship = collision.gameObject.GetComponent<SpaceshipController>();
            if (spaceship != null)
            {
                spaceship.Die(true);
            }
        }
    }

    private void CreatePointsForCircle(int numberOfPoints)
    {

        _line.positionCount = numberOfPoints;
        float x;
        float y;

        float anglePerSegment = 360 / (numberOfPoints - 1);
        float angle = anglePerSegment;

        for (int i = 0; i < numberOfPoints; i++)
        {
            var radius = transform.localScale.x * .1f;
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            _line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360 + anglePerSegment) / (numberOfPoints);
        }
    }
}
