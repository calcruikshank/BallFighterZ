using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingAnimationBehavior : MonoBehaviour
{
    [SerializeField] Transform rightHandTransform;
    [SerializeField] Transform leftHandTransform;
    [SerializeField] GameObject right1, right2, right3, right4;
    [SerializeField] GameObject left1, left2, left3, left4;
    // Start is called before the first frame update
    void Start()
    {
        right1.SetActive(true);
        right2.SetActive(false);
        right3.SetActive(false);
        right4.SetActive(false);
        left1.SetActive(true);
        left2.SetActive(false);
        left3.SetActive(false);
        left4.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PunchLeftAnimation();
        //.64 1.189 2 < .3
        if (rightHandTransform.localPosition.x <= 0)
        {
            right1.SetActive(true);
            right2.SetActive(false);
            right3.SetActive(false);
            right4.SetActive(false);
            return;
        }
        if (rightHandTransform.localPosition.x < 1f && rightHandTransform.localPosition.x > .6f)
        {
            right1.SetActive(false);
            right2.SetActive(true);
            right3.SetActive(false);
            right4.SetActive(false);
            return;
        }
        if (rightHandTransform.localPosition.x >= 1f && rightHandTransform.localPosition.x < 1.2f)
        {
            right1.SetActive(false);
            right2.SetActive(false);
            right3.SetActive(true);
            right4.SetActive(false);
            return;
        }
        if (rightHandTransform.localPosition.x >= 1.2f && rightHandTransform.localPosition.x < 2f)
        {
            right1.SetActive(false);
            right2.SetActive(false);
            right3.SetActive(false);
            right4.SetActive(true);
            return;
        }
        if (rightHandTransform.localPosition.x >= 2f)
        {
            Debug.Log("fully extended");
            right1.SetActive(false);
            right2.SetActive(false);
            right3.SetActive(false);
            right4.SetActive(true);
            return;
        }
    }

    void PunchLeftAnimation()
    {
        if (leftHandTransform.localPosition.x <= 0)
        {
            left1.SetActive(true);
            left2.SetActive(false);
            left3.SetActive(false);
            left4.SetActive(false);
            return;
        }
        if (leftHandTransform.localPosition.x < 1f && leftHandTransform.localPosition.x > .6f)
        {
            left1.SetActive(false);
            left2.SetActive(true);
            left3.SetActive(false);
            left4.SetActive(false);
            return;
        }
        if (leftHandTransform.localPosition.x >= 1f && leftHandTransform.localPosition.x < 1.2f)
        {
            left1.SetActive(false);
            left2.SetActive(false);
            left3.SetActive(true);
            left4.SetActive(false);
            return;
        }
        if (leftHandTransform.localPosition.x >= 1.2f && leftHandTransform.localPosition.x < 2f)
        {
            left1.SetActive(false);
            left2.SetActive(false);
            left3.SetActive(false);
            left4.SetActive(true);
            return;
        }
        if (leftHandTransform.localPosition.x >= 2f)
        {
            Debug.Log("fully extended");
            left1.SetActive(false);
            left2.SetActive(false);
            left3.SetActive(false);
            left4.SetActive(true);
            return;
        }
    }
}
