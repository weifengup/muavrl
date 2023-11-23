using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SocialPlatforms.Impl;
public class PlaneAgent : Agent
{
    public GameObject floor; // 地面对象
    public GameObject target; // 目标的 GameObject

    private Rigidbody rBody;//球刚体
    public float moveSpeed = 0.01f;
    public float rotationSpeed = 100.0f;

    private int score=0;
    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        score=0;
    }

    /// <summary>
    /// Agent重置
    /// </summary>
    public override void OnEpisodeBegin()
    {
        Bounds floorBounds = GetFloorBounds();

        float x, y, z;
        x = Random.Range(floorBounds.min.x, floorBounds.max.x);
        y = Random.Range(transform.localScale.y / 2, 5);
        z = Random.Range(floorBounds.min.z, floorBounds.max.z);
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition; // 直接更改脚本所附加的物体的位置

        float x2, y2, z2;
        x2 = Random.Range(floorBounds.min.x, floorBounds.max.x);
        y2 = Random.Range(transform.localScale.y / 2, 5);
        z2 = Random.Range(floorBounds.min.z, floorBounds.max.z);
        Vector3 newPosition2 = new Vector3(x2, y2, z2);
        // 随机设置目标的位置
        target.transform.position = newPosition2;
        score=0;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // 观察飞机和目标的位置
        sensor.AddObservation(target.transform.position);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rBody.velocity);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var continuousActionsOut = actionBuffers.ContinuousActions;
        // 控制飞机的移动
        float moveX = continuousActionsOut[0];
        float moveY = continuousActionsOut[1];
        float moveZ = continuousActionsOut[2];
        //当然上面这两句可以互换，因为Brain并不知道action[]数组中数值具体含义
        rBody.AddForce(moveX * moveSpeed,moveY * moveSpeed,moveZ * moveSpeed);

        // 设置奖励
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget < 1.0f)
        {
            SetReward(1f); // 到达目标，奖励为1
            EndEpisode();
        }
        else if (transform.position.y < 0 || transform.position.y > 5)
        {
            EndEpisode();
        }
        else
        {
            SetReward(-0.01f); // 惩罚，鼓励飞机快速到达目标
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Buildings"))
        {
            // 碰撞到建筑物时扣除分数
            int pointsToDeduct = 10; // 根据实际需要设置扣除的分数
            score -= pointsToDeduct;
            Debug.Log("Score: " + score);
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 通过键盘输入控制飞机的移动和旋转
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
        // continuousActionsOut[2] = Input.GetAxis("Rotate");
        Debug.Log(continuousActionsOut[0]);
    }
    Bounds GetFloorBounds()
    {
        Renderer floorRenderer = floor.GetComponent<Renderer>();
        Bounds bounds = floorRenderer.bounds;
        return bounds;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
