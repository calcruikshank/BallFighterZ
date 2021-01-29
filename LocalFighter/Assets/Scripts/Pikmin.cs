using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikmin : MonoBehaviour
{
    Transform positionToFollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (positionToFollow != null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, positionToFollow.position, 12 * Time.deltaTime);
        }
    }

    public void FollowTransform(Transform positionInTransformArray)
    {
        positionToFollow = positionInTransformArray;
    }
}
