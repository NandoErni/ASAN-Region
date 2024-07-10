using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera => GetComponent<Camera>();

    private void LateUpdate()
    {
        var bound = EncapsulateObjectsInBound();


        var newPosition = Vector2.Lerp(transform.position, bound.center, Time.deltaTime * 5);
        transform.position = new Vector3(newPosition.x, newPosition.y, -10);

        var newSize = Mathf.Sqrt(bound.size.x * bound.size.x + bound.size.y * bound.size.y) / 2f;
        var cameraSize = Mathf.Max(newSize, 70);
        Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, cameraSize, Time.deltaTime);
    }

    private Bounds EncapsulateObjectsInBound()
    {

        var bound = new Bounds(Vector3.zero, Vector3.zero);
        var spaceships = GameManager.Instance.Spaceships;

        foreach (var spaceship in spaceships)
        {
            bound.Encapsulate(spaceship.transform.position);
        }

        bound.Encapsulate(FindAnyObjectByType<StartingPointController>().transform.position);
        bound.Encapsulate(FindAnyObjectByType<DestinationPointController>().transform.position);

        return bound;
    }

    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        transform.localPosition = transform.localPosition + new Vector3(.5f, -.5f, 0);
        yield return new WaitForSeconds(.05f);

        transform.localPosition = transform.localPosition + new Vector3(.7f, .7f, 0);
        yield return new WaitForSeconds(.05f);

        transform.localPosition = transform.localPosition + new Vector3(-.5f, -.5f, 0);
        yield return new WaitForSeconds(.05f);

        transform.localPosition = transform.localPosition + new Vector3(.5f, .5f, 0);
        yield return new WaitForSeconds(.05f);

        transform.localPosition = transform.localPosition + new Vector3(-.7f, -.7f, 0);
        yield return new WaitForSeconds(.05f);

        transform.localPosition = transform.localPosition + new Vector3(-.5f, .5f, 0);
        yield return new WaitForSeconds(.05f);

        transform.localPosition = transform.localPosition + new Vector3(.7f, -.7f, 0);
        yield return new WaitForSeconds(.05f);

        transform.localPosition = transform.localPosition + new Vector3(-.7f, .7f, 0);
        yield return new WaitForSeconds(.05f);

    }
}
