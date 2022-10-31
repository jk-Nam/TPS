using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect;

    private void OnCollisionEnter(Collision coll)
    {
        //if (coll.collider.tag == "BULLET")
        if (coll.collider.CompareTag("BULLET"))
        {
            ContactPoint cp = coll.GetContact(0);
            Quaternion rot = Quaternion.LookRotation(-cp.normal);
            GameObject effect = Instantiate<GameObject>(sparkEffect, cp.point, rot);
            effect.transform.SetParent(this.transform);
            Destroy(coll.gameObject);
        }    
    }
}
