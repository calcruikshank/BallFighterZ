using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransformHandler : MonoBehaviour
{
    public GameObject playerToAnimate;
    /*public GameObject facingRight;
    public GameObject facingForward;
    public GameObject facingUp;
    public GameObject facingDownRight;*/
    public GameObject one;
    public GameObject two;
    public GameObject three;
    public GameObject four;
    public GameObject five;
    public GameObject six;
    public GameObject seven;
    public GameObject eight;
    public GameObject nine;
    public GameObject ten;
    public GameObject eleven;
    public GameObject twelve;
    public GameObject thirteen;
    public GameObject fourteen;
    public GameObject fifteen;
    public GameObject sixteen;

    public GameObject spriteHolder;
    Transform[] spriteTranformsArray = new Transform[16];
    TrailRenderer trailRender;
    // Start is called before the first frame update
    void Start()
    {
        trailRender = this.gameObject.GetComponent<TrailRenderer>();
        trailRender.emitting = false;

        //spriteTranformsArray = spriteHolder.GetComponentsInChildren<Transform>();
        spriteTranformsArray[0] = one.transform;
        spriteTranformsArray[1] = two.transform;
        spriteTranformsArray[2] = three.transform;
        spriteTranformsArray[3] = four.transform;
        spriteTranformsArray[4] = five.transform;
        spriteTranformsArray[5] = six.transform;
        spriteTranformsArray[6] = seven.transform;
        spriteTranformsArray[7] = eight.transform;
        spriteTranformsArray[8] = nine.transform;
        spriteTranformsArray[9] = ten.transform;
        spriteTranformsArray[10] = eleven.transform;
        spriteTranformsArray[11] = twelve.transform;
        spriteTranformsArray[12] = thirteen.transform;
        spriteTranformsArray[13] = fourteen.transform;
        spriteTranformsArray[14] = fifteen.transform;
        spriteTranformsArray[15] = sixteen.transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = playerToAnimate.transform.position;
        if (playerToAnimate.transform.eulerAngles.z < 11.25f && playerToAnimate.transform.eulerAngles.z > -11.25)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[4].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 33.75f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[5].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 56.25f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[6].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 78.75f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[7].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 101.25f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[8].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 123.75f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[9].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 146.25f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[10].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 168.75f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[11].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 191.25f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[12].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 213.75f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[13].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 236.25f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[14].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 258.75f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[15].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 281.25f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[0].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 303.75f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[1].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 326.25f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[2].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 348.75f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[3].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }
        if (playerToAnimate.transform.eulerAngles.z < 348.75f && playerToAnimate.transform.eulerAngles.z > -22.5)
        {
            for (int i = 0; i < spriteTranformsArray.Length; i++)
            {
                spriteTranformsArray[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            spriteTranformsArray[4].GetComponent<SpriteRenderer>().enabled = true;
            return;
        }











        /*if (playerToAnimate.transform.right.x < 0)
        {
            //this.transform.localScale = new Vector3(-0.35f, 0.35f, 0.35f);
        }
        if (playerToAnimate.transform.right.x > 0)
        {
            this.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }
        if (playerToAnimate.transform.right.x > .75f || playerToAnimate.transform.right.x < -.75f)
        {
            facingRight.SetActive(true);
            facingUp.SetActive(false);
            facingForward.SetActive(false);
            facingDownRight.SetActive(false);
            return;
        }
        if (playerToAnimate.transform.right.y > .5f)
        {

            facingDownRight.SetActive(false);
            facingRight.SetActive(false);
            facingForward.SetActive(false);
            facingUp.SetActive(true);
            return;
        }
        if (playerToAnimate.transform.right.x < .35f && playerToAnimate.transform.right.x > -.35f)
        {
            facingDownRight.SetActive(false);
            facingRight.SetActive(false);
            facingForward.SetActive(true);
            facingUp.SetActive(false);
            return;
        }
        if (playerToAnimate.transform.right.x > .35f || playerToAnimate.transform.right.x < -.35f)
        {
            facingDownRight.SetActive(true);
            facingUp.SetActive(false);
            facingForward.SetActive(false);
            facingRight.SetActive(false);
            return;
        }*/


    }
    public void SetPlayer(GameObject playerGO)
    {
        playerToAnimate = playerGO;
    }

    public void SetEmittingToTrue()
    {
        if (trailRender != null)
        {
            trailRender.emitting = true;
        }
    }
    public void SetEmittingToFalse()
    {
        if (trailRender != null)
        {
            trailRender.emitting = false;
        }
    }
    public void EnableEmitter()
    {
        if (trailRender != null)
        {
            trailRender.enabled = (true);
        }
    }
    public void DisableEmitter()
    {
        if (trailRender != null)
        {
            trailRender.enabled = (false);
        }
    }
}
