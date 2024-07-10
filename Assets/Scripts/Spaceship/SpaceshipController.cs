using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceshipController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private CameraController _camera;
    public Sprite[] PossibleSprites;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _camera = Camera.main.GetComponent<CameraController>();
        _rigidbody = GetComponent<Rigidbody2D>();

        System.Random random = new();
        int index = random.Next(0, PossibleSprites.Length);
        _spriteRenderer.sprite = PossibleSprites[index];

    }

    private void LateUpdate()
    {
        CheckIsOutOfBounds();
    }

    public void ArriveAtDestination()
    {
        GameManager.Instance.FinishSpaceship(this);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        var angle = Mathf.Atan2(_rigidbody.velocity.y, _rigidbody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));

    }

    public void Die(bool diedThroughBlackhole = false)
    {
        _camera.ShakeCamera();
        if (diedThroughBlackhole)
        {
            GameManager.Instance.CursorCrash.Play();
        } else
        {
            GameManager.Instance.SpaceshipExplosion.Play();
        }
        GameManager.Instance.Spaceships.Remove(gameObject);
        Destroy(gameObject);
    }

    private void CheckIsOutOfBounds()
    {
        if (transform.position.magnitude > 400)
        {
            Die();
        }
    }
}
