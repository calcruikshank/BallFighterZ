using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 mousePosition, movement, inputMovement, lastMoveDir;
    
    
    [SerializeField] float moveSpeed;

    PhotonView PV;

    private State state;
    private enum State
    {
        Normal,
        Knockback,
        Diving,
        Grabbed
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!PV.IsMine) //if the player is not owned by the user
        {
            Destroy(rb); //destroys rigidbody of enemy so we dont calculate physics twice
        }
    }

    void Update()
    {
        if (!PV.IsMine) //if the player is not owned by the user
        {
            return;
        }
        
        switch (state)
        {
            case State.Normal:
                HandleMovement();
                break;
        }
    }
    void FixedUpdate()
    {
        if (!PV.IsMine) //if the player is not owned by the user
        {
            return;
            
        }
        switch (state)
        {
            case State.Normal:
                FixedHandleMovement();
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
        if (!PV.IsMine)
        {
            return;
        }
        inputMovement = value.Get<Vector2>();
    }
    public void OnMouseLook(InputValue value)
    {
        if (!PV.IsMine)
        {
            return;
        }
        mousePosition = value.Get<Vector2>();
        FaceMouse();
    }
    void FaceMouse()
    {
        if (!PV.IsMine)
        {
            return;
        }
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.right = direction;
    }
}
