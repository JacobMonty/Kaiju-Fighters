using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public Rigidbody rb;
    public Animator _animator;
    public SpriteRenderer sr;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // Note: I got this error because of the input package. Will not work if not deleted

        // TODO: Make player faster on y axis only

        rb.linearVelocity = new Vector3(x * speed, 0, y * speed * 1.25f);

        // flip sprite depending on direction
        if (x != 0 && x < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        if(x != 0 || y != 0) 
        {
            _animator.SetBool("isMoving", true);
            //Debug.Log("IsMoving");
        } else
        {
            _animator.SetBool("isMoving", false);
            //Debug.Log("Idle");
        }
    }
}
