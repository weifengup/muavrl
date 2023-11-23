using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;
using System;
namespace Commons{
    class MyVector3{
        public float x;
        public float y;
        public float z;
        public MyVector3(Vector3 v){
            this.x=v.x;
            this.y=v.y;
            this.z=v.z;
        }
        public Vector3 AsVector3(){
            return new Vector3(this.x,this.y,this.z);
        }
    }
    class RLResult{
        public List<List<float>> observations;
        public List<float> rewards;
        public List<Boolean> done;
        public List<String> info;
        public RLResult(List<List<float>> observations,List<float> rewards,List<Boolean> done,List<String> info){
            this.observations=new List<List<float>>();
            foreach (var observation in observations)
            {
                var copiedInnerList = new List<float>(observation.Count); // 创建一个新的内部列表
                foreach (var value in observation)
                {
                    copiedInnerList.Add(value);
                }
                this.observations.Add(copiedInnerList);
            }
            this.rewards=new List<float>(rewards.Count);
            foreach(float reward in rewards){
                this.rewards.Add(reward);
            }
            this.done=new List<bool>(done.Count);
            foreach(Boolean d in done){
                this.done.Add(d);
            }
            this.info=new List<string>(info.Count);
            foreach(String str in info){
                this.info.Add(str);
            }
        }
    }
    class Agent{
        public string name;
        public MyVector3 location;
        public Agent(){

        }
        public Agent(string name){
            this.name=name;
        }
        public Agent(string name, MyVector3 location)
        {
            this.name=name;
            this.location=location;
        }
    }
    class Agents{
        public int n;
        public List<Agent> agents;
        public Agents(int n,List<Agent> agents){
            this.n=n;
            this.agents=agents;
        }
    }
    
}
