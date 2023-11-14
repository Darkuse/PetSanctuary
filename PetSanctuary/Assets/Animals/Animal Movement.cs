using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    [SerializeField]
    private float walkingSpeed;
    private Rigidbody2D rb;
    public List<Transform> walkingPositions;
    [SerializeField]
    private bool walking = false;
    [SerializeField]
    private int nextPosition;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        ChooseNextPosition();
    }

    private void Update()
    {
        if (walking)
        {
            MoveToNextPosition();
        }
    }

    private void ChooseNextPosition()
    {
        nextPosition = Random.Range(0, walkingPositions.Count);
        walking = true;
    }

    private void MoveToNextPosition()
    {
        Vector3 targetPosition = walkingPositions[nextPosition].position;
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

    private void Run()
    {

    }

    public void SetAnimalSpeed(float speed)
    {
        walkingSpeed = speed;
    }

    public void RunFromPlayer(float speed, float time, Vector3 playerPosition)
    {
        StartCoroutine(Run(speed, time,playerPosition));
    }

    private IEnumerator Run(float speed, float time, Vector3 playerPosition)
    {
        walking = false;

        float startTime = Time.time;

        float endTime = startTime + time;

        while (Time.time < endTime)
        {
            Vector3 direction = transform.position - playerPosition;
            direction.Normalize();
            rb.AddForce( direction * speed *10 );

            yield return null;
        }

        walking = true;
    }
}
