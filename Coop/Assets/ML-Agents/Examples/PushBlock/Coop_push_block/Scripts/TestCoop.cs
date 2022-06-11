using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class TestCoop : MonoBehaviour
{
    
    public class PlayerInfo
    {
        public TestBehaviorScript Agent;

        public Vector3 StartingPos;

        public Quaternion StartingRot;

        public Rigidbody Rb;
    }

    public List<PlayerInfo> AgentsList = new List<PlayerInfo>();
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;
    float fc =0;//frame count for each prefab
    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;

    private int m_ResetTimer;

    // Start is called before the first frame update
    void Start()
    {
        fc = 0;

        // Initialize TeamManager
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_RedAgentGroup = new SimpleMultiAgentGroup();

        foreach (var item in AgentsList)
        {
            item.StartingPos = item.Agent.transform.position;
            item.StartingRot = item.Agent.transform.rotation;
            item.Rb = item.Agent.GetComponent<Rigidbody>();
            if (item.Agent.team == Team.Blue)
            {
                m_BlueAgentGroup.RegisterAgent(item.Agent);
            }
            else
            {
                m_RedAgentGroup.RegisterAgent(item.Agent);
            }
        }

    }

    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_BlueAgentGroup.GroupEpisodeInterrupted();
            m_RedAgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in AgentsList)
        {
            Hit(item.Agent);
            
         //   AgentGroupActIncentive(item.Agent);
        }
        fc++;
        m_RedAgentGroup.AddGroupReward(-0.0001f * fc);
        m_BlueAgentGroup.AddGroupReward(-0.0001f * fc * fc);
    }


    void Hit(TestBehaviorScript col)
    {

        // Touched goal.
        if (col.gameObject.CompareTag("goal") && (col.transform.tag == "blue"))
        {
            print("Blue Entered Goal");
            m_BlueAgentGroup.AddGroupReward(2f);
            m_RedAgentGroup.AddGroupReward(-2f);
            if (col.tag == "blue") { col.AddReward(1f); }
            //if (col.tag == "red") { col.AddReward(1f); }
            //envController.ResetScene();
            //OnEpisodeBegin();
            //EndEpisode();
        }

    }

    /*void AgentGroupActIncentive(IndivisualPlayer col)
    {
        if (col.tag=="red")
        {
            col.
        }
    }*/
    public void ResetScene()
    {
        m_ResetTimer = 0;
        fc = 0;
        //Reset Agents
        foreach (var item in AgentsList)
        {
            var randomPosX = Random.Range(-5f, 5f);
            var newStartPos = new Vector3(0f,1f,0f) + new Vector3(randomPosX, 0f, 0f);
            var rot = item.Agent.rotSign * Random.Range(80.0f, 100.0f);
            var newRot = Quaternion.Euler(0, rot, 0);
            item.Agent.transform.SetPositionAndRotation(newStartPos, newRot);

            item.Rb.velocity = Vector3.zero;
            item.Rb.angularVelocity = Vector3.zero;
        }
    }
}

