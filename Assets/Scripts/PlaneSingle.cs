using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;
using Commons;
using System;
using System.Security.Cryptography;
public class PlaneSingle : MonoBehaviour
{
    public Floor floor;
    private GameObject[] targets;
    private GameObject[] planes;
    private GameObject[] obstacles; 
    private float moveSpeed=0.2f;
    private Rigidbody rBody;//球刚体
    public Boolean reach;
    private float reach_score=0;
    private float time_score=0;
    private float distance;
    private Collider coll;
    private Collider[] buildingColliders;
    private int step;
    // Simulation simulation;
    // Start is called before the first frame update
    void Start()
    {
        planes = GameObject.FindGameObjectsWithTag("Teammate");
        targets = GameObject.FindGameObjectsWithTag ("Target"); 
        obstacles = GameObject.FindGameObjectsWithTag ("Buildings"); 
        coll=GetComponent<Collider>();
        buildingColliders=new Collider[obstacles.Length];
        for(int i=0;i<buildingColliders.Length;i++){
            buildingColliders[i]=obstacles[i].GetComponent<Collider>();
        }
        // do{
            setPosition();
        // }while(checkIfContact());
        //观测其他物体


        rBody = GetComponent<Rigidbody>();
        //初始化得分
        this.reach_score=0;
        this.reach=false;
        this.step=0;
        this.time_score=0;
        this.distance=(float)Math.Sqrt(Math.Pow(transform.localPosition.x - targets[0].transform.localPosition.x, 2) + Math.Pow(transform.localPosition.y - targets[0].transform.localPosition.y, 2)+Math.Pow(transform.localPosition.z - targets[0].transform.localPosition.z, 2));
        // simulation=GetComponent<Simulation>();
    }
    public List<float> GetObservation(){
        List<float> observation=new List<float>();
        float posX=transform.position.x;
        float posY=transform.position.y;
        float posZ=transform.position.z;
        for(int i=0;i<targets.Length;i++){
            // observation.Add(posX-targets[i].transform.position.x);
            // observation.Add(posY-targets[i].transform.position.y);
            // observation.Add(posZ-targets[i].transform.position.z);
            Vector3 obs=Vector3.Normalize(targets[i].transform.position-transform.position);
            observation.Add(obs.x);
            observation.Add(obs.y);
            observation.Add(obs.z);
        }
        for(int i=0;i<planes.Length;i++){
            // observation.Add(posX-planes[i].transform.position.x);
            // observation.Add(posY-planes[i].transform.position.y);
            // observation.Add(posZ-planes[i].transform.position.z);
            if(planes[i].gameObject.name==this.gameObject.name){
                continue;
            }
                
            Vector3 obs=Vector3.Normalize(planes[i].transform.position-transform.position);
            observation.Add(obs.x);
            observation.Add(obs.y);
            observation.Add(obs.z);
        }
        for(int i=0;i<obstacles.Length;i++){
            // observation.Add(posX-obstacles[i].transform.position.x);
            // observation.Add(posY-obstacles[i].transform.position.y);
            // observation.Add(posZ-obstacles[i].transform.position.z);
            Vector3 obs=Vector3.Normalize(obstacles[i].transform.position-transform.position);
            observation.Add(obs.x);
            observation.Add(obs.y);
            observation.Add(obs.z);
        }
        return observation;
    }
    public void Reset(){

        planes = GameObject.FindGameObjectsWithTag("Teammate");
        targets = GameObject.FindGameObjectsWithTag ("Target"); 
        obstacles = GameObject.FindGameObjectsWithTag ("Buildings"); 
        coll=GetComponent<Collider>();
        buildingColliders=new Collider[obstacles.Length];
        for(int i=0;i<buildingColliders.Length;i++){
            buildingColliders[i]=obstacles[i].GetComponent<Collider>();
        }
        // do{
            setPosition();
        // }while(checkIfContact());
        //观测其他物体


        //初始化得分
        this.reach_score=0;
        this.reach=false;
        this.step=0;
        this.time_score=0;
        this.distance=(float)Math.Sqrt(Math.Pow(transform.localPosition.x - targets[0].transform.localPosition.x, 2) + Math.Pow(transform.localPosition.y - targets[0].transform.localPosition.y, 2)+Math.Pow(transform.localPosition.z - targets[0].transform.localPosition.z, 2));

    }
    void setPosition(){
         //初始化位置
        Bounds floorBounds=floor.GetFloorBounds();
        float x, y, z;
        x = UnityEngine.Random.Range(floorBounds.min.x, floorBounds.max.x);
        y = UnityEngine.Random.Range(transform.localScale.y / 2, 5);
        z = UnityEngine.Random.Range(floorBounds.min.z, floorBounds.max.z);
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition; // 直接更改脚本所附加的物体的位置
    }
    bool checkIfContact(){
        for(int i=0;i<this.buildingColliders.Length;i++){
            
            if(coll.bounds.Intersects(this.buildingColliders[i].bounds)){
            
                return true;
            }
        }
        return false;

    }
    public int GetObservationSize(){
        List<float> observation=GetObservation();
        return observation.Count;
    }
    public int GetActionSize(){
        return 3;
    }
    public float GetReward(){
        //奖励为前后两个step的距离差
        float distance2=(float)Math.Sqrt(Math.Pow(transform.localPosition.x - targets[0].transform.localPosition.x, 2) + Math.Pow(transform.localPosition.y - targets[0].transform.localPosition.y, 2)+Math.Pow(transform.localPosition.z - targets[0].transform.localPosition.z, 2));
        float dist_score=(this.distance-distance2);
        // print(this.score);
        this.distance=distance2;

        //奖励为负距离
        // float distance2=(float)Math.Sqrt(Math.Pow(transform.localPosition.x - targets[0].transform.localPosition.x, 2) + Math.Pow(transform.localPosition.y - targets[0].transform.localPosition.y, 2)+Math.Pow(transform.localPosition.z - targets[0].transform.localPosition.z, 2));
        // this.score-=0.1f*distance2;

        // if(this.distance<5*transform.localScale.x)
        //     this.score+=5;
        // print(this.reach_score+dist_score);
        return this.reach_score+dist_score;

        // return dist_score;
    }
    public Boolean GetReach(){
        return this.reach;
    }
    public void Step(float[] continuousActions){
        // 控制飞机的移动
        float moveX = continuousActions[0];
        float moveY = continuousActions[1];
        float moveZ = continuousActions[2];
        // rBody.AddForce(moveX * moveSpeed,moveY * moveSpeed,moveZ * moveSpeed);
        // Vector3 pos=new Vector3(a.x+moveX * moveSpeed,a.y+moveY * moveSpeed,a.z+moveZ * moveSpeed);
        // Debug.Log(pos);
        // transform.position=pos;
        // print(transform.localPosition);
        Vector3 pos=new Vector3(moveX * moveSpeed,moveY * moveSpeed,moveZ * moveSpeed);
        transform.localPosition+=pos;
        // print(transform.localPosition);
        // transform.Translate(moveX * moveSpeed*Time.fixedDeltaTime,moveY * moveSpeed*Time.fixedDeltaTime,moveZ * moveSpeed*Time.fixedDeltaTime);
        this.time_score-=0.01f*(this.step/10);
    }
    public void OnTriggerEnter(Collider collider){
        //单智能体 reach_score-=3, +=5
        if(collider.gameObject.CompareTag("Buildings")){
            Debug.Log("crash to obstacles");
            this.reach_score-=3;
            reach=true;
        }else if(collider.gameObject.CompareTag("Target")){
            Debug.Log("reach target");
            this.reach_score+=5;
            // transform.localPosition=(new Vector3(-100,-100,-100));
            reach=true; 
        }
        else if(collider.gameObject.CompareTag("Teammate")){
            
            Debug.Log("crash to teammate");
            this.reach_score-=3;
            reach=true;
        }  

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
