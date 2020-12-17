using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputMouse : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 movement;
    Vector2 inputMovement;
    public Vector2 mousePosition;
    public Vector2 lastLookedPosition;
    Vector2 lastMoveDir;

    public Transform rightHand;
    public Transform leftHand;
    public float punchSpeed = 80f;
    public float returnSpeed = 10f;

    public bool punchedRight = false;
    public bool punchedLeft = false;
    public bool returningRight = false;
    public bool returningLeft = false;
    public float moveSpeed = 8f;

    public Transform grabbedPosition;

    public Vector2 oppositeForce;

    public float brakeSpeed = 20f;

    public bool isBlocking;
    public float timer;

    private State state;
    private enum State
    {
        WithoutBall,
        WithBall,
        Knockback,
        Diving,
        Grabbed
    }

    void Awake()
    {
        state = State.WithoutBall;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(lastLookedPosition);
        //Debug.Log(state);
        switch (state)
        {
            case State.WithoutBall:
                HandleMovement();
                HandleThrowHands();
                HandleBlock();
                break;
            case State.Diving:
                //HandleDiving();
                break;
            case State.Knockback:
                HandleKnockback();
                break;
            case State.Grabbed:
                HandleGrabbed();
                break;
        }
    }
    void FixedUpdate()
    {
        switch (state)
        {
            case State.WithoutBall:
                FixedHandleMovement();
                break;
            case State.Knockback:
                FixedHandleKnockback();
                break;

        }
    }

    public void HandleMovement()
    {
        movement.x = inputMovement.x;
        movement.y = inputMovement.y;
        movement = movement;
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement;
        }
    }
    public void FixedHandleMovement()
    {
        rb.velocity = movement * moveSpeed;
    }


    private void OnKeyboardMove(InputValue value)
    {
        inputMovement = value.Get<Vector2>();
        
    }
    public void OnMouseLook(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
        FaceMouse();
    }

    void FaceMouse()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.right = direction;
        
    }

    public void HandleThrowHands()
    {
        if (punchedRight == true && returningRight == false)
        {
            ThrowRightHand();
        }
        if (rightHand.localPosition.x >= 1.4f && punchedRight == false)
        {
            returningRight = true;
        }
        if (returningRight)
        {
            ReturnRightHand();
            punchedRight = false;
        }


        if (punchedLeft == true && returningLeft == false)
        {
            ThrowLeftHand();
        }

        if (leftHand.localPosition.x >= 1.4f && punchedLeft == false)
        {
            returningLeft = true;
        }
        if (returningLeft)
        {
            ReturnLeftHand();
            punchedLeft = false;

        }
    }

    public void ThrowRightHand()
    {
        rightHand.localPosition = Vector3.MoveTowards(rightHand.localPosition, new Vector2(1.4f, .4f), punchSpeed * Time.deltaTime);
    }
    public void ReturnRightHand()
    {
        rightHand.localPosition = Vector3.MoveTowards(rightHand.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
    }
    public void ThrowLeftHand()
    {
        leftHand.localPosition = Vector3.MoveTowards(leftHand.localPosition, new Vector2(1.4f, -.4f), punchSpeed * Time.deltaTime);
    }
    public void ReturnLeftHand()
    {
        leftHand.localPosition = Vector3.MoveTowards(leftHand.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
    }


    private void OnReleasePunchRight()
    {
        punchedRight = false;
    }
    private void OnReleasePunchLeft()
    {
        punchedLeft = false;
    }


    public void HandleKnockback()
    {
        if (rb.velocity.magnitude <= 5f)
        {
            Debug.Log(state);
            rb.velocity = new Vector2(0, 0);
            state = State.WithoutBall;
            
        }
        if (rb.velocity.magnitude > 0)
        {
            oppositeForce = -rb.velocity;
            brakeSpeed = brakeSpeed + (150f * Time.deltaTime);
            rb.AddForce(oppositeForce * Time.deltaTime * brakeSpeed);
        }
        

    }
    public void FixedHandleKnockback()
    {
        
    }

    public void HandleBlock()
    {
        if (punchedLeft || punchedRight)
        {
            timer += Time.deltaTime;

            if (timer > .2f)
            {
                moveSpeed = 3.5f;
                isBlocking = true;
            }
        }

        if (punchedRight == false && punchedLeft == false)
        {
            isBlocking = false;
            timer = 0;
            moveSpeed = 8f;
        }

    }

    public void HandleGrabbed()
    {
        rb.transform.position = grabbedPosition.position;

    }

    private void OnPunchRight()
    {
        if (state != State.WithoutBall)
        {
            return;
        }
        punchedRight = true;
        returningRight = false;
    }
   



    private void OnPunchLeft()
    {
        if (state != State.WithoutBall)
        {
            return;
        }
        punchedLeft = true;
        returningLeft = false;
    }

    public void ChangeStateToKnockback()
    {
        brakeSpeed = 30f; //chnage this to damage * current percentage later
        state = State.Knockback;
    }

    public void ChangeStateToGrabbed(Transform handThatGrabbed)
    {

        state = State.Grabbed;
        grabbedPosition = handThatGrabbed;
        leftHand.GetComponent<Collider2D>().isTrigger = true;
        rightHand.GetComponent<Collider2D>().isTrigger = true;
    }



}
