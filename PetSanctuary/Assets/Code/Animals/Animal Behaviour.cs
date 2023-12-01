using System.Collections;
using UnityEngine;

public class AnimalBehaviour : MonoBehaviour
{
    private AnimalInfo _info;
    private AnimalMovement _movement;
    private Rigidbody2D rb;

    private void Start()
    {
        _info = GetComponent<AnimalInfo>();
        _movement = transform.GetChild(0).GetComponent<AnimalMovement>();
        _movement.SetAnimalSpeed(_info.walkingSpeed);
        rb = GetComponent<Rigidbody2D>();
    }

    public void RunFromPlayer(Collider2D collision)
    {
        Debug.Log("Player entered [" + _info.name + "] collision");
        StartCoroutine(Run(_info.runningSpeed, _info.runningTime, collision.transform.position));
        // _movement.RunFromPlayer(_info.runningSpeed, _info.runningTime, collision.transform.position);
    }


    private IEnumerator Run(float speed, float time, Vector3 playerPosition)
    {
        _movement.SetAnimalSpeed(_info.runningSpeed);
        float startTime = Time.time;

        float endTime = startTime + time;

        while (Time.time < endTime)
        {
            Vector3 direction = transform.position - playerPosition;
            direction.Normalize();
            rb.AddForce(direction * speed * 10);
            _movement.ForceNewPosition();

            yield return null;
        }

        rb.velocity = Vector2.zero;
        _movement.SetAnimalSpeed(_info.walkingSpeed);
    }
}
