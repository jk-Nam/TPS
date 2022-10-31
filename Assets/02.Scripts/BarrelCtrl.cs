using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;
    public Texture[] textures; 
    public AudioClip expSfx;
    public float radius = 10.0f;

    private Transform tr;
    private Rigidbody rb;
    private int hitCount = 0;
    private new MeshRenderer renderer;

   
    private new AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        renderer = GetComponentInChildren<MeshRenderer>();
        //int idx = Random.Range(0, textures.Length);
        //int idx = Random.Range(1, textures.Length);
        int idx = Random.Range(0, 3);
        renderer.material.mainTexture = textures[idx];
        audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET")) 
        {
            if (++hitCount == 3)
            {
                ExpBarrel(); 
            }
        }
    }

    void ExpBarrel()
    {
        audio.PlayOneShot(expSfx, 3.0f);
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);
        Destroy(exp, 4.0f);
        //rb.mass = 1.0f;
        //rb.AddForce(Vector3.up * 800.0f);
        indirectDamage(tr.position);
        renderer.material.mainTexture = textures[3];
        //Destroy(this.gameObject, 3.0f);
        

    }

    private void indirectDamage(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);
        foreach (var coll in colls)
        {
            rb = coll.GetComponentInParent<Rigidbody>();
            rb.mass = 1.0f;
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
        }
    }
}
