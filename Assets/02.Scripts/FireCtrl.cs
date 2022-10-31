using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;
    public AudioClip fireSfx;

    private new AudioSource audio;
    private MeshRenderer muzzleFlash;
    private RaycastHit hit;
    private float nexttFire;
    private readonly float fireRate = 0.1f;


    void Start()
    {
        audio = GetComponent<AudioSource>();
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;
    }

    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20.0f, Color.green);
        if (Input.GetMouseButton(0))
        {
            if (Time.time >= nexttFire)
            {
                Fire();
                if (Physics.Raycast(firePos.position, firePos.forward, out hit, 20.0f, 1 << 6))
                {
                    Debug.Log($"Hit = {hit.transform.name}");
                    hit.transform.GetComponent<MonsterCtrl>()?.OnDamage(hit.point, hit.normal);
                }
                nexttFire = Time.time + fireRate;
            }
        }
    }

    void Fire()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
        audio.PlayOneShot(fireSfx, 1.0f);
        StartCoroutine(ShowMuzzleFlash());
    }
    IEnumerator ShowMuzzleFlash()
    {
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) *0.5f;
        muzzleFlash.material.mainTextureOffset = offset;
        Vector3 scale = Vector3.one * Random.Range(0.4f, 0.8f);
        muzzleFlash.transform.localScale = scale;
        float angle = Random.Range(0, 24) * 15.0f;
        muzzleFlash.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.01f, 0.06f));
        muzzleFlash.enabled = false;
    }
}
