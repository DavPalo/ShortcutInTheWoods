using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class VehicleController : NetworkBehaviour
{
    public Rigidbody2D body;
    public float acceleration;
    public float speed;
    public float maxSpeed;

    public float horizontal;
    public float vertical;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        this.enabled = false;
    }

    /*void FixedUpdate()
    {
        if (horizontal != 0 || vertical != 0)
            speed += acceleration * Time.deltaTime;
        else
            speed -= 2*acceleration * Time.deltaTime;
        
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }*/

    private Vector2 movementVector;
    private float currentForewardDirection = 1;
    public UnityEvent<float> OnSpeedChange = new UnityEvent<float>();

    public void Move(Vector2 movementVector)
    {
        this.movementVector = movementVector;
        CalculateSpeed(movementVector);
        OnSpeedChange?.Invoke(this.movementVector.magnitude);
        if (movementVector.y > 0)
        {
            if (currentForewardDirection == -1)
                speed = 0;
            currentForewardDirection = 1;
        }
        else if (movementVector.y < 0)
        {
            if (currentForewardDirection == 1)
                speed = 0;
            currentForewardDirection = -1;
        }

    }

    private void CalculateSpeed(Vector2 movementVector)
    {
        if (Mathf.Abs(movementVector.y) > 0)
        {
            speed += acceleration * Time.deltaTime;
        }
        else
        {
            speed -= acceleration * Time.deltaTime;
        }
        speed = Mathf.Clamp(speed, 0, maxSpeed);
    }

    private void FixedUpdate()
    {
        body.velocity = (Vector2)transform.up * speed * currentForewardDirection * Time.fixedDeltaTime;
        //rb2d.MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -movementVector.x * rotationSpeed * Time.fixedDeltaTime));
    }
}
