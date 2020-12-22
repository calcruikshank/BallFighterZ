using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 mousePosition, movement, inputMovement, lastMoveDir, oppositeForce;
    
    
    [SerializeField] float moveSpeed, punchRange, punchSpeed, returnSpeed, currentPercentage, brakeSpeed;
    public bool punchedRight, punchedLeft, returningRight, returningLeft = false;
    public RightHand rightHandScript;
    public LeftHand leftHandScript;
    public PhotonView PV;

    public Transform rightHandTransform;
    public Transform leftHandTransform;
    public CircleCollider2D rightHandCollider, leftHandCollider;

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
        rightHandCollider = rightHandTransform.GetComponent<CircleCollider2D>();
        leftHandCollider = leftHandTransform.GetComponent<CircleCollider2D>();
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
                HandleThrowingHands();
                break;
            case State.Knockback:
                HandleKnockback();
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
    
    

    public void Knockback(float damage, Vector2 direction)
    {
        if (!PV.IsMine) return; 
        PV.RPC("RPCKnockback", RpcTarget.All, damage, direction);
        
    }
    
    public void HandleKnockback()
    {
        if (rb.velocity.magnitude <= 5f)
        {
            rb.velocity = new Vector2(0, 0);
            state = State.Normal;
        }
        if (rb.velocity.magnitude > 0)
        {
            oppositeForce = -rb.velocity;
            brakeSpeed = brakeSpeed + (150f * Time.deltaTime);
            rb.AddForce(oppositeForce * Time.deltaTime * brakeSpeed);
        }
    }


    public void HandleThrowingHands()
    {
        if (punchedRight && returningRight == false)
        {
            rightHandCollider.enabled = true;
            PV.RPC("RPCPunchRight", RpcTarget.AllBuffered);
        }
        if (returningRight)
        {
            punchedRight = false;
            PV.RPC("RPCReturnRight", RpcTarget.AllBuffered);
            
        }
        if (punchedLeft && returningLeft == false)
        {
            leftHandCollider.enabled = true;
            PV.RPC("RPCPunchLeft", RpcTarget.AllBuffered);
        }
        if (returningLeft)
        {
            punchedLeft = false;
            PV.RPC("RPCReturnLeft", RpcTarget.AllBuffered);
        }
    }

    #region RPCRegion
    [PunRPC]
    public void RPCPunchRight()
    {
        rightHandCollider.enabled = true;
        rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(punchRange, .4f), punchSpeed * Time.deltaTime);
        if (rightHandTransform.localPosition.x >= punchRange)
        {
            returningRight = true;
        }

    }
    [PunRPC]
    public void RPCReturnRight()
    {
        rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
        if (rightHandTransform.localPosition.x <= 0f)
        {
            rightHandCollider.enabled = false;
            returningRight = false;
        }
    }
    [PunRPC]
    public void RPCPunchLeft()
    {
        leftHandCollider.enabled = true;
        leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(punchRange, -.4f), punchSpeed * Time.deltaTime);
        if (leftHandTransform.localPosition.x >= punchRange)
        {
            returningLeft = true;
        }

    }
    [PunRPC]
    public void RPCReturnLeft()
    {
        leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
        if (leftHandTransform.localPosition.x <= 0f)
        {
            leftHandCollider.enabled = false;
            returningLeft = false;
        }
    }
    [PunRPC]
    public void RPCKnockback(float damage, Vector2 direction)
    {
        if (!PV.IsMine) return;
        currentPercentage += damage;
        brakeSpeed = 30f;
        Debug.Log(damage + " damage");
        float knockbackValue = (14 * ((currentPercentage + damage) * (damage / 3)) / 100) + 7; //knockback that scales
        rb.AddForce(direction * knockbackValue, ForceMode2D.Impulse);

        Debug.Log(currentPercentage + "current percentage");
        state = State.Knockback;
    }
    #endregion


    #region InputRegion
    void OnKeyboardMove(InputValue value)
    {
        if (!PV.IsMine) return;
        
        inputMovement = value.Get<Vector2>();
    }
    void OnMouseLook(InputValue value)
    {
        if (!PV.IsMine) return;
        
        mousePosition = value.Get<Vector2>();
        FaceMouse();
    }
    void FaceMouse()
    {
        if (!PV.IsMine) return;
        
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.right = direction;
    }
    void OnPunchRight()
    {
        if (!PV.IsMine) return;
        if (returningRight) return;
        punchedRight = true;
    }
    void OnPunchLeft()
    {
        if (!PV.IsMine) return;
        if (returningLeft) return;
        punchedLeft = true;
    }
    #endregion
}
