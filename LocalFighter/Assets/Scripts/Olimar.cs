using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Olimar : PlayerController
{
    float spawnPikminTimer = 0f;
    bool spawnPikmin = false;
    [SerializeField] GameObject PikminPrefab;
    [SerializeField] Transform pikminHolder;
    public List<Pikmin> pikminList = new List<Pikmin>();
    public Transform[] pikminPositionArray;
    int numOfPikmin = 0;

    public override void Start()
    {

        canAirShieldThreshold = .1f;
        if (playerAnimatorBase != null)
        {
            animationTransformHandler = Instantiate(playerAnimatorBase, transform.position, Quaternion.identity).GetComponent<AnimationTransformHandler>();
            animationTransformHandler.SetPlayer(this.gameObject);
            animator = animationTransformHandler.GetComponent<Animator>();
        }

        pikminPositionArray = pikminHolder.GetComponentsInChildren<Transform>();
        totalShieldRemaining = 225f / 255f;
        if (team % 2 == 0)
        {
            GameObject redHandObject = Instantiate(redHand, Vector3.zero, Quaternion.identity);
            redHandObject.transform.SetParent(rightHandTransform, false);
            GameObject redHandObject1 = Instantiate(redHand, Vector3.zero, Quaternion.identity);
            redHandObject1.transform.SetParent(leftHandTransform, false);

            shield = Instantiate(redShield, Vector3.zero, Quaternion.identity);
            shield.transform.SetParent(this.transform, false);

            Color redColor = new Color(255f / 255f, 97f / 255f, 96f / 255f);
            playerBody.material.SetColor("_Color", redColor);
            //shield.GetComponent<SpriteRenderer>().material.SetColor("_Color", redColor);
        }
        if (team % 2 == 1)
        {
            GameObject blueHandObject = Instantiate(blueHand, Vector3.zero, Quaternion.identity);
            blueHandObject.transform.SetParent(rightHandTransform, false);
            GameObject blueHandObject1 = Instantiate(blueHand, Vector3.zero, Quaternion.identity);
            blueHandObject1.transform.SetParent(leftHandTransform, false);

            shield = Instantiate(blueShield, Vector3.zero, Quaternion.identity);
            shield.transform.SetParent(this.transform, false);

            Color blueColor = new Color(124f / 255f, 224f / 255f, 224f / 255f);
            playerBody.material.SetColor("_Color", blueColor);
            //shield.GetComponent<SpriteRenderer>().material.SetColor("_Color", blueColor);
        }
        rightHandCollider = rightHandTransform.GetComponent<CircleCollider2D>();
        leftHandCollider = leftHandTransform.GetComponent<CircleCollider2D>();
        canDash = true;
        stocksLeft = 4;
        if (gameManager != null)
        {
            stocksLeftText.text = (stocksLeft.ToString());
            gameManager.SetTeam((PlayerController)this);
        }

    }

    protected override void Update()
    {
        switch (state)
        {
            case State.Normal:
                HandleMovement();
                HandleThrowingHands();
                HandleShielding();
                HandleUniversal();
                HandlePikminInput();
                break;
            case State.Knockback:
                HandleKnockback();
                HandleThrowingHands();
                HandleShielding();
                HandleAirShielding();
                break;
            case State.Grabbed:
                HandleGrabbed();
                HandleShielding();
                HandleThrowingHands();
                break;
            case State.FireGrabbed:
                HandleGrabbed();
                HandleShielding();
                HandleThrowingHands();
                break;
            case State.Grabbing:
                HandleMovement();
                HandlePummel();
                break;
            case State.Stunned:
                HandleStunned();
                break;
            case State.Dashing:
                HandleDash();
                HandleThrowingHands();
                HandleShielding();
                break;
            case State.PowerShieldStunned:
                HandlePowerShieldStunned();
                break;
            case State.PowerShielding:
                HandlePowerShielding();
                break;
            case State.PowerDashing:
                HandlePowerDashing();
                HandleThrowingHands();
                HandleShielding();
                break;
            case State.ShockGrabbed:
                HandleShockGrabbed();
                HandleShielding();
                HandleThrowingHands();
                break;
        }
    }

    void HandlePikminInput()
    {
        if (spawnPikminTimer > 0) spawnPikmin = true;
        if (spawnPikmin)
        {
            if (numOfPikmin + 1 < pikminPositionArray.Length)
            {
                Debug.Log("Spawn pkmin");
                GameObject newPikmin = Instantiate(PikminPrefab, pikminPositionArray[numOfPikmin + 1].position, Quaternion.identity);
                newPikmin.transform.localScale = newPikmin.transform.localScale * .5f;
                newPikmin.GetComponent<Pikmin>().FollowTransform(pikminPositionArray[numOfPikmin + 1]);
                numOfPikmin++;
                spawnPikminTimer = 0;
            }

            spawnPikmin = false;
        }
    }

    protected override void OnDash()
    {
        spawnPikminTimer = inputBuffer;
    }
    protected override void OnMouseDash()
    {
        spawnPikminTimer = inputBuffer;
        Debug.Log("pressed dash");
    }
}
