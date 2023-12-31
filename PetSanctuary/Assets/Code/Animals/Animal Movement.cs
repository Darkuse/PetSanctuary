using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Vector2 nextPosition;
    private float walkingRadius;
    public bool runningFromPlayer = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        walkingRadius = GetComponentInParent<Animal>().animalInfo.walkingRadius;
        GetComponent<CircleCollider2D>().radius = GetComponentInParent<Animal>().animalInfo.detectionRadius;
        ChooseNextPosition();
    }

    private void Update()   
    {
        MoveToNextPosition();
    }

    private void ChooseNextPosition()
    {
        float random1 = Random.Range(-walkingRadius, walkingRadius);
        float random2 = Random.Range(-walkingRadius, walkingRadius);
        nextPosition = new Vector2(random1, random2);
    }

    public void ForceNewPosition()
    {
        nextPosition = new Vector2(0, 0);
    }

    private void MoveToNextPosition()
    {
        Vector3 targetPosition = nextPosition;
        targetPosition += transform.parent.position;
        Vector3 currentPosition = transform.position;

        float distance = Vector3.Distance(targetPosition, currentPosition);

        if (distance < 0.1f)
        {
            ChooseNextPosition();
            return;
        }

        Vector3 direction = targetPosition - currentPosition;

        direction.Normalize();
        rb.MovePosition(currentPosition + direction * walkingSpeed * Time.deltaTime);

    }

    public void SetAnimalSpeed(float speed)
    {
        walkingSpeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<AnimalBehaviour>().RunFromPlayer(collision);
        }
    }

}
