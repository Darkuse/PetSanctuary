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
    [SerializeField]
    GameObject objecst;

    private void Awake()
    {
        rb = transform.GetChild(0).GetComponent<Rigidbody2D>();
        walkingRadius = GetComponent<AnimalInfo>().detectionRadius;
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

    private void MoveToNextPosition()
    {

        Vector3 targetPosition = nextPosition;
        targetPosition += transform.position;
        Vector3 currentPosition = transform.GetChild(0).position;

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

}
