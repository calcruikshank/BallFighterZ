using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransformHandler : MonoBehaviour
{
    public GameObject playerToAnimate;
    public GameObject facingRight;
    public GameObject facingForward;
    public GameObject facingUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerToAnimate.transform.right);
        this.transform.position = playerToAnimate.transform.position;
        if (playerToAnimate.transform.right.x < 0)
        {
            this.transform.localScale = new Vector3(-0.35f, 0.35f, 0.35f);
        }
        if (playerToAnimate.transform.right.x > 0)
        {
            this.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }
        if (playerToAnimate.transform.right.x > .5f || playerToAnimate.transform.right.x < -.5f)
        {
            facingRight.SetActive(true);
            facingUp.SetActive(false);
            facingForward.SetActive(false);
        }
        if (playerToAnimate.transform.right.y > .5f)
        {
            facingRight.SetActive(false);
            facingForward.SetActive(false);
            facingUp.SetActive(true);
        }
        if (playerToAnimate.transform.right.y < -.5f)
        {
            facingRight.SetActive(false);
            facingForward.SetActive(true);
            facingUp.SetActive(false);
        }
    }
}
