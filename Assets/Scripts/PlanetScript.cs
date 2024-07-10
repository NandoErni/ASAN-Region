using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlanetScript : MonoBehaviour
{

    public float Gravitation = 1;
    private LineRenderer _line;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _gravityTrigger;
    private float _radius => (float)Math.Pow(Gravitation, .4) * 2f;

    [SerializeField]
    private Sprite[] POSSIBLE_SPRITES;

    private List<Rigidbody2D> _objectsUnderGravityInfluence = new List<Rigidbody2D>();

    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gravityTrigger = GetComponent<CircleCollider2D>();
        _gravityTrigger.radius = _radius;
        _gravityTrigger.isTrigger = true;

        transform.localScale = Vector3.one * Gravitation * 10;

        _line.colorGradient = GetColorGradient();
        _line.useWorldSpace = false;
        _line.widthMultiplier = .5f;
        CreatePointsForCircle(51);
        RemovePointsForCircle();

        PickRandomSprite();
    }

    private void PickRandomSprite()
    {
        Random random = new Random();
        int index = random.Next(0, POSSIBLE_SPRITES.Length);
        _spriteRenderer.sprite = POSSIBLE_SPRITES[index];
    }

    private void FixedUpdate()
    {
        foreach (var obj in _objectsUnderGravityInfluence)
        {
            var direction = transform.position - obj.transform.position;
            var force = direction.normalized * 1000 * Gravitation / (direction.magnitude * direction.magnitude);
            obj.AddForce(force);
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        var otherRigidBody = trigger.gameObject.GetComponent<Rigidbody2D>();

        if (otherRigidBody != null && trigger.tag == Tags.PLAYER_TAG)
        {
            _objectsUnderGravityInfluence.Add(otherRigidBody);
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {

        var otherRigidBody = trigger.gameObject.GetComponent<Rigidbody2D>();

        if (otherRigidBody != null && trigger.tag == Tags.PLAYER_TAG)
        {
            _objectsUnderGravityInfluence.Remove(otherRigidBody);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var spaceship = collision.gameObject.GetComponent<SpaceshipController>();
        if (spaceship != null)
        {
            spaceship.Die();
        }
    }

    private Gradient GetColorGradient()
    {
        var gradient = new Gradient();

        var colors = new GradientColorKey[2];
        colors[0] = new GradientColorKey(_spriteRenderer.color, 0.0f);
        colors[1] = new GradientColorKey(_spriteRenderer.color, 1.0f);

        var alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(.04f, 0.0f);
        alphas[1] = new GradientAlphaKey(.04f, 1.0f);

        gradient.SetKeys(colors, alphas);

        return gradient;
    }

    private void RemovePointsForCircle()
    {
        CreatePointsForCircle(0);
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
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * _radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * _radius;

            _line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360 + anglePerSegment) / (numberOfPoints);
        }
    }
}
