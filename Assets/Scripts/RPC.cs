using System.Collections;
using System.Collections.Generic;
using AustinHarris.JsonRpc;
using UnityEngine;
using Commons;
using System;
public class RPC : MonoBehaviour
{
    class API:JsonRpcService{

        // Plane[] teammates;
        PlaneSingle[] teammates;
        Target[] targets;
        GameObject[] obstales;
        // Simulation simulation;
        public API(Target[] targets,PlaneSingle[] teammates,GameObject[] obstales){
            this.targets=targets;
            this.teammates=teammates;
            this.obstales=obstales;
            // this.simulation=simulation;
        }
        [JsonRpcMethod] 
        public Agents GetPlanes(){
            List<Agent> planes=new List<Agent>();
            for(int i=0;i<this.teammates.Length;i++){
                // planes[i]=new Agent(this.teammates[i].name,new MyVector3(this.teammates[i].transform.localPosition));
                planes.Add(new Agent(this.teammates[i].name,new MyVector3(this.teammates[i].transform.localPosition)));
            }
            return new Agents(this.teammates.Length,planes);
        }
        [JsonRpcMethod]
        public int GetAgentNum(){
            return teammates.Length;
        }
        [JsonRpcMethod]
        public int GetActionNum(){
            return 3;
        }
        [JsonRpcMethod]
        public List<int> GetActorDims(){
            List<int> actor_dims=new List<int>();
            for(int i=0;i<teammates.Length;i++){
                actor_dims.Add(teammates[i].GetObservationSize());
            }
            return actor_dims;
        }
        [JsonRpcMethod]
        public RLResult Step(List<float[]> actions){
            List<List<float>> observations=new List<List<float>>();
            List<float> rewards=new List<float>();
            List<Boolean> done=new List<Boolean>();
            List<String> info=new List<String>();
            for(int i=0;i<teammates.Length;i++){
                // print(teammates[i].transform.localPosition);
                teammates[i].Step(actions[i]);
                // simulation.Simulate();
                // print(teammates[i].transform.localPosition);
                List<float> observation=teammates[i].GetObservation();
                List<float>temp=new List<float>(observation.Count);
                foreach(float o in observation){
                    temp.Add(o);
                }
                observations.Add(temp);
                done.Add(teammates[i].GetReach());
                rewards.Add(teammates[i].GetReward());
                info.Add(teammates[i].gameObject.name);
            }
            return new RLResult(observations,rewards,done,info);
        }
        [JsonRpcMethod]
        public RLResult Reset(){
            List<List<float>> observations=new List<List<float>>();
            List<float> rewards=new List<float>();
            List<Boolean> done=new List<Boolean>();
            List<String> info=new List<String>();
            for(int i=0;i<targets.Length;i++){
                targets[i].Reset();
            }
            for(int i=0;i<teammates.Length;i++){
                teammates[i].Reset();
                List<float> observation=teammates[i].GetObservation();
                List<float>temp=new List<float>(observation.Count);
                foreach(float o in observation){
                    temp.Add(o);
                }
                observations.Add(temp);
                rewards.Add(teammates[i].GetReward());
                done.Add(teammates[i].GetReach());
                info.Add(teammates[i].gameObject.name);
            }
            return new RLResult(observations,rewards,done,info);
        }
    }
    public PlaneSingle[] planes;
    public Target[] targets;

    private GameObject[] obstacles;
    private API api;

    // private Simulation simulation;
    // Start is called before the first frame update
    void Start()
    {
        // planes = GameObject.FindGameObjectsWithTag("Teammate");
        // targets = GameObject.FindGameObjectsWithTag ("Target"); 
        obstacles = GameObject.FindGameObjectsWithTag ("Buildings"); 
        // simulation=GetComponent<Simulation>();
        api=new API(targets,planes,obstacles);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
