using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moment : MonoBehaviour
{
    public Vector3 nextPosition;

    private bool isMoving = false;
    [SerializeField] private float moveSpeed;

    public float moveThreshold;


    public void SetMoving(bool moving)
    {
        isMoving = moving;
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetNextPosition(Vector3 newPos)
    {
        nextPosition = newPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //Debug.Log($"Moving from {transform.position} to {nextPosition}. Step distance {Time.deltaTime * moveSpeed}. Abs distance {Mathf.Abs(Vector3.Distance(transform.position, nextPosition))}");
            transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * moveSpeed);

            if(Mathf.Abs(Vector3.Distance(transform.position, nextPosition)) <= .0006)
            {
                transform.position = nextPosition;
                isMoving = false;
            }

        }
    }
}
