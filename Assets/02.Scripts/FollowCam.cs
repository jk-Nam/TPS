using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform targetTr;

    private Transform camTr;

    [Range(2.0f, 20.0f)]
    public float distance = 3.0f;
    [Range(0.0f, 10.0f)]
    public float height = 2.5f;

    public float damping = 0.1f;
    public float targetOffset = 1.9f;
    private Vector3 velocity = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        camTr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
       
        //camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
        Vector3 pos = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
        //camTr.position = Vector3.Slerp(camTr.position, pos, Time.deltaTime * damping);
        camTr.position = Vector3.SmoothDamp(camTr.position, pos, ref velocity, damping);

        camTr.LookAt(targetTr.position + (targetTr.up * targetOffset));
    }
}
