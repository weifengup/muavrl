using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;
using Commons;
using System;
public class Target : MonoBehaviour
{
    public Floor floor;
    private Collider coll;
    private Collider[] buildingColliders;
    private Boolean reach;
    // Start is called before the first frame update
    void Start()
    {
        coll=GetComponent<Collider>();
        GameObject[] buildings=GameObject.FindGameObjectsWithTag ("Buildings"); 
        buildingColliders=new Collider[buildings.Length];
        for(int i=0;i<buildings.Length;i++){
            buildingColliders[i]=buildings[i].GetComponent<Collider>();
        }
        do{
            setPosition();
        }while(checkIfContact());
        this.reach=false;


    }
    public void Reset(){
        // coll=GetComponent<Collider>();
        // GameObject[] buildings=GameObject.FindGameObjectsWithTag ("Buildings"); 
        // buildingColliders=new Collider[buildings.Length];
        // for(int i=0;i<buildings.Length;i++){
        //     buildingColliders[i]=buildings[i].GetComponent<Collider>();
        // }
        this.reach=false;
        // do{
            setPosition();
        // }while(checkIfContact());
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
    void Update()
    {
    
    }
    bool checkIfContact(){
        for(int i=0;i<this.buildingColliders.Length;i++){
            if(coll.bounds.Intersects(this.buildingColliders[i].bounds)){
                print("?");
                return true;
            }
        }
        return false;

    }
    public Boolean getReach(){
        return this.reach;
    }
    public void setReach(){
        this.reach=true;
    }
     public void OnTriggerEnter(Collider collider){
        //单智能体 reach_score-=3, +=5
        if(collider.gameObject.CompareTag("Buildings")){
            Reset();
        }
        else if(collider.gameObject.CompareTag("Teammate")){
            // Debug.Log("reach");
            // reach=true;
        }  

    }
}
