using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3[] gene;
    private Rigidbody rgd;
    private Vector3 vec;
    private int frm_cnt = 0;
    private float duration;
    private float arrvl_time = 0f;
    private float mx_time = 0f;
    private float speed = 10f;
    private bool is_arrvl = false;



    void Start(){
        rgd = GetComponent<Rigidbody>();
    }



    void FixedUpdate(){
        if(!(is_arrvl)){
            arrvl_time += Time.deltaTime;
        }

        mx_time += Time.deltaTime;

        vec = gene[frm_cnt];
        frm_cnt++;

        rgd.AddForce(vec*speed, ForceMode.Force);
    }



    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.name == "Wall"){
            is_arrvl = true;
        }
    }



    public void Set(float drtn, int amnt_gene, Vector3[] vecs){
        duration = drtn;
        gene = new Vector3[amnt_gene];

        for(int i=0;i<amnt_gene;i++){
            gene[i] = vecs[i];
        }
    }



    public Vector3[] Get(){
        return gene;
    }



    public float Get_pnt(){
        if(!(is_arrvl)){
            arrvl_time = 0f;
        }

        float res = (1 - arrvl_time/mx_time);

        return res;
    }
}
