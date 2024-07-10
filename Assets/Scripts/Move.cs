using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    private const float _velocity = 10;
    private Vector3 _directory;
    private Rigidbody _rb;

    private void Start()
    {
        transform.name = "Healthy";
        _rb = GetComponent<Rigidbody>();

        do
        {
            _directory = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        } while (_directory == Vector3.zero);

        StartCoroutine(ChangeDirection());
    }

    private void FixedUpdate()
    {
        transform.Translate(_directory * _velocity * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.name)
        {
            case "x":
                _directory.x = -_directory.x;
                break;
            case "z":
                _directory.z = -_directory.z;
                break;
        }
    }

    private void OnCollisionExit()
    {
        if (_rb.velocity != Vector3.zero) _rb.velocity = Vector3.zero;
    }

    private IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(.5f, 1.5f) * Get.DayTime);
        do
        {
            _directory = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        } while (_directory == Vector3.zero);

        StartCoroutine(ChangeDirection());
    }
}