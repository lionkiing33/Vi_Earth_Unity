using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //1
    public float movementSpeed = 3.0f;

    //2
    Vector2 movement = new Vector2();

    //2.1
    Animator animator;

    //2.2
    string animationState = "AnimationState";

    //3
    Rigidbody2D rb2D;

    //3.1
    enum CharStates
    {
        walkEast = 1,
        walkSouth = 2,
        walkWest = 3,
        walkNorth = 4,
        idleSouth = 5
    }

    // 초기화용
    private void Start()
    {
        //4
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update는 프레임당 한 번씩 불린다.
    private void Update()
    {
        //지금은 비워둔다.
        UpdateState();
    }

    //5
    void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        //6
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //7
        movement.Normalize();

        //8
        rb2D.velocity = movement * movementSpeed;
    }

    private void UpdateState()
    {
        if(movement.x > 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkEast);
        }
        else if (movement.x < 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkWest);
        }
        else if (movement.y > 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkNorth);
        }
        else if (movement.y < 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkSouth);
        }
        else
        {
            animator.SetInteger(animationState, (int)CharStates.idleSouth);
        }
    }
}