using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem dust;
    private Rigidbody2D rb;
    private float moveH, moveV;
    private int horizontalSide = 1;
    [SerializeField] private float moveSpeed = 2.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveH = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveV = Input.GetAxisRaw("Vertical") * moveSpeed;
        if (moveH < 0 && horizontalSide == 1)
        {
            horizontalSide = -1;
            Flip();
        }
        if(moveH > 0 && horizontalSide != 1)
        {
            horizontalSide = 1;
            Flip();
        }
    }

    private void FixedUpdate()
    {
        
        rb.velocity = new Vector2(moveH, moveV);
    }


    private void CreateDust()
    {
        dust.Play();
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x = horizontalSide;
        transform.localScale = theScale;
        CreateDust();
    }
}
