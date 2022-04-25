using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public float duration;



    private Ball bll;
    private Vector3 wall;
    private Vector3 ball;
    private float pnt = 0f;
    private float strt;
    private float end;
    private bool is_fall = false;



    void Start(){
        ball = this.transform.GetChild(0).position;
        wall = this.transform.GetChild(2).position;

        strt = Vector3.Distance(ball, wall);
    }



    void FixedUpdate(){
        ball = this.transform.GetChild(0).position;

        if(ball.y < -5f){
            is_fall = true;
        }
    }



    public float Get(){
        ball = this.transform.GetChild(0).position;
        wall = this.transform.GetChild(2).position;

        bll = this.transform.GetChild(0).GetComponent<Ball>();
        float tmp = bll.Get_pnt();

        pnt += tmp;
        
        end = Vector3.Distance(ball, wall);
        pnt += (1f - (end/strt));

        if(is_fall){
            pnt -= 0.5f;
        }

        return pnt;
    }



    public void Set(float drtn){
        duration = drtn;
    }
}
