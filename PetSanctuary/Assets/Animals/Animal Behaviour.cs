using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalBehaviour : MonoBehaviour
{
    private AnimalInfo _info;
    private AnimalMovement _movement;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;

    private void Start()
    {
        _info = GetComponent<AnimalInfo>();
        _movement = GetComponent<AnimalMovement>();
        circleCollider = GetComponent<CircleCollider2D>();
        _movement.SetAnimalSpeed(_info.walkingSpeed);
        circleCollider.radius = _info.detectionRadius;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered [" + _info.name+"] collision");
            StartCoroutine(Run(_info.runningSpeed, _info.runningTime, collision.transform.position));
            // _movement.RunFromPlayer(_info.runningSpeed, _info.runningTime, collision.transform.position);
        }
    }
#if DEBUG

#endif
    private IEnumerator Run(float speed, float time, Vector3 playerPosition)
    {

        float startTime = Time.time;

        float endTime = startTime + time;

        while (Time.time < endTime)
        {
            Vector3 direction = transform.position - playerPosition;
            direction.Normalize();
            rb.AddForce(direction * speed * 10);

            yield return null;
        }

        rb.velocity = Vector2.zero;
    }
}
