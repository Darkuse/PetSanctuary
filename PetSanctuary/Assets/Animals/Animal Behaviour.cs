using UnityEngine;

public class AnimalBehaviour : MonoBehaviour
{
    private AnimalInfo _info;
    private AnimalMovement _movement;
    private CircleCollider2D circleCollider;

    private void Start()
    {
        _info = GetComponent<AnimalInfo>();
        _movement = GetComponent<AnimalMovement>();
        circleCollider = GetComponent<CircleCollider2D>();
        _movement.SetAnimalSpeed(_info.walkingSpeed);
        circleCollider.radius = _info.detectionRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered [" + _info.name+"] collision");
            _movement.RunFromPlayer(_info.runningSpeed, _info.runningTime, collision.transform.position);
        }
    }
}
