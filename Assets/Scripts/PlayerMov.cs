using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    public Rigidbody2D body;
    public float acceleration;
    public float speed;
    public float maxSpeed;

    public Vector2 movement;
    public Vector2 lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // -1 is left
        movement.y = Input.GetAxisRaw("Vertical"); // -1 is down

        if (movement.x != 0 || movement.y != 0)
            speed += acceleration * Time.deltaTime;
        else
            speed -= 2 * acceleration * Time.deltaTime;
        
        speed = Mathf.Clamp(speed, 0, maxSpeed);
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }
}
