using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    float MoveSpeed = 0;

    private Vector2 vector;
    private Rigidbody2D rb;
    private SpriteRenderer rend;
    private Animator ani;

    private void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }
    
   void Update()
    {   
        vector.x = Input.GetAxisRaw("Horizontal");
        vector.y = Input.GetAxisRaw("Vertical");
        rb.velocity = vector * MoveSpeed;
        Flip();
        AnimationUpdate();
      
      

    }
    private void Flip()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rend.flipX = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rend.flipX = false;
        }
    }

    private void AnimationUpdate()
    {
        if( vector.x == 0f && vector.y == 0f) 
        {
            ani.SetBool("Run", false);
        }
        else if (vector.x != 0f && vector.y != 0f)
        {
            ani.SetBool("Run", true);
        }
    }
}
