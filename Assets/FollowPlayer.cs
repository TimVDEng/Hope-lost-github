using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public float followSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + offset, followSpeed * Time.deltaTime);
            
    }
}
