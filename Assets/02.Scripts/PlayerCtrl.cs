using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    private Transform tr;
    private Animation anim;
    private readonly float initHp = 100.0f;
    private GameObject bloodEffect;
    private Image hpBar;

    public float moveSpeed = 10.0f;
    public float turnSpeed = 250.0f;
    public float currHp;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        currHp = initHp;
        tr = GetComponent<Transform>();
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
        hpBar = GameObject.FindGameObjectWithTag("HP_BAR").GetComponent<Image>();
        anim = GetComponent<Animation>();
        anim.Play("Idle");
        turnSpeed = 0.0f;
        yield return new WaitForSeconds(1.0f);
        turnSpeed = 250.0f;
    }

    //private float h => Input.GetAxis("Horizontal");
    //private float v => Input.GetAxis("Vertical");

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        //Debug.Log("H : " + h);
        //Debug.Log("V : " + v);

        Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);
        if (dir.sqrMagnitude > 1.0f) dir = dir.normalized;
        tr.Translate(dir * moveSpeed * Time.deltaTime);
        tr.Rotate(Vector3.up * r * Time.deltaTime * turnSpeed);
        PlayerAnim(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        //if (v >= 0.1f) anim.Play("RunF");
        //else if (v <= -0.1f) anim.Play("RunB");
        //else if (h >= 0.1f) anim.Play("RunR");
        //else if (h <= -0.1f) anim.Play("RunL");
        //else anim.Play("Idle");
        if (v >= 0.1f) anim.CrossFade("RunF", 0.25f);
        else if (v <= -0.1f) anim.CrossFade("RunB", 0.25f);
        else if (h >= 0.1f) anim.CrossFade("RunR", 0.25f);
        else if (h <= -0.1f) anim.CrossFade("RunL", 0.25f);
        else anim.CrossFade("Idle", 0.25f);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (currHp >= 0.0f && coll.CompareTag("PUNCH"))
        {
            currHp -= 10.0f;
            DisplayHealth();
            Debug.Log($"Player hp = {currHp / initHp}");
            //Debug.Log($"Player hp = {(currHp / initHp) * 100.0f} %");
            if (currHp <= 0.0f)
            {
                PlayerDie();
            }
            Vector3 bloodPos = tr.position + (Vector3.up * 1.5f);
            GameObject blood = Instantiate<GameObject>(bloodEffect, bloodPos, tr.rotation);
            Destroy(blood, 0.5f);
        }
    }

    void DisplayHealth()
    {
        hpBar.fillAmount = currHp / initHp;
    }

    void PlayerDie()
    {
        Debug.Log("Player Die!!!");
        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        //foreach (var monster in monsters)
        //{
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
        OnPlayerDie();
        //GameObject.Find("GameManager").GetComponent<GameManager>().IsGameOver = true;
        GameManager.instance.IsGameOver = true;
    }
}
