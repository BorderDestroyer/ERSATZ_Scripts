using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    GameObject player;

    [SerializeField]
    int currentPoint;

    [SerializeField]
    int[] passbarrierLocation;

    [SerializeField]
    float _speed;

    GameObject passBarrier;

    [SerializeField]
    Vector3[] cameraPoints;
    [SerializeField]
    Vector3[] PassBarrierPoints;
    Rigidbody2D rb;

    PlayerBehavior pb;

    public bool MoveToNextPoint;
    bool canMove;
    bool canUpdate; 
    void Start()
    {
        canUpdate = false;
        passBarrier = GameObject.FindGameObjectWithTag("PassBarrier");

        rb = GetComponent<Rigidbody2D>();
        pb = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
        player = GameObject.FindGameObjectWithTag("Player");

        currentPoint = 0;
        MoveToNextPoint = false;
    }

    void Update()
    {
        if(currentPoint < cameraPoints.Length)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }

        if(MoveToNextPoint && canMove)
        {
            Debug.Log("Moving");
            MoveToNextPoint = false;
            Vector2 direction = cameraPoints[currentPoint] - transform.position;
            direction = direction.normalized;
            Vector3 targetVelocity = new Vector3(direction.x * _speed, direction.y * _speed);
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, _speed);
            canUpdate = true;
            passBarrier.transform.position = PassBarrierPoints[passbarrierLocation[currentPoint + 1]] + transform.position;

            switch(passbarrierLocation[currentPoint + 1])
            {
                case 0:
                    passBarrier.transform.rotation = Quaternion.Euler(passBarrier.transform.rotation.x, passBarrier.transform.rotation.y, 0);
                    break;
                case 1:
                    passBarrier.transform.rotation = Quaternion.Euler(passBarrier.transform.rotation.x, passBarrier.transform.rotation.y, 90);
                    break;
                case 2:
                    passBarrier.transform.rotation = Quaternion.Euler(passBarrier.transform.rotation.x, passBarrier.transform.rotation.y, 0);
                    break;
                case 3:
                    passBarrier.transform.rotation = Quaternion.Euler(passBarrier.transform.rotation.x, passBarrier.transform.rotation.y, 90);
                    break;
            }

        }

        if(Vector2.Distance(transform.position, cameraPoints[currentPoint]) <= 1 && canUpdate)
        {
            canUpdate = false;
            currentPoint += 1;
            rb.velocity = new Vector3(0, 0, 0);
            passBarrier.transform.position = new Vector3(passBarrier.transform.position.x, passBarrier.transform.position.y, 10);
        }
    }
}
