using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class TeamCoop : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInfo
    {
        public IndivisualPlayer Agent;

        public Vector3 StartingPos;

        public Quaternion StartingRot;

        public Rigidbody Rb;
    }

    /*public float bluewins = 0;
    public float redwins = 0;
    public float bluewin_percentage = 0f;
    public float redwin_percentage = 0f;
    public float episode_count = 0;*/
    public int NumOfDefendersPatrolling;
    public GameObject goal;
    public GameObject ground ;
    Vector3 groundPos ;
    public List<PlayerInfo> AgentsList = new List<PlayerInfo>();
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 50000;
    float fc;//frame count for each prefab
    public SimpleMultiAgentGroup m_BlueAgentGroup;
    public SimpleMultiAgentGroup m_RedAgentGroup;
    public int m_RedAgentGroupCount;
    public int m_BlueAgentGroupCount;
    private int m_ResetTimer;

    // Start is called before the first frame update
    void Start()
    {
        fc = 0;
        groundPos = ground.transform.localPosition;
        goal = gameObject.transform.GetChild(0).gameObject;
       
        //print(goal.name+""+goal.tag);
        // Initialize TeamManager
    m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_RedAgentGroup = new SimpleMultiAgentGroup();

        foreach (var item in AgentsList)
        {
            item.StartingPos = item.Agent.transform.localPosition;
            item.StartingRot = item.Agent.transform.localRotation;
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
        m_RedAgentGroupCount = m_RedAgentGroup.GetRegisteredAgents().Count;
        m_BlueAgentGroupCount = m_BlueAgentGroup.GetRegisteredAgents().Count;
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
        fc++;
        m_RedAgentGroup.AddGroupReward(0.001f * fc * fc);
        m_BlueAgentGroup.AddGroupReward(-0.001f * fc * fc);
    }

    // Update is called once per frame
    void Update()
    {
        /*bluewin_percentage = (bluewins / episode_count) * 100f;
        redwin_percentage = (redwins / episode_count) * 100f;*/
        foreach (var item in AgentsList)
        {
            Hit(item.Agent);
            StrikerHit(item.Agent);
            ClosestStrikerPunishment(item.Agent);
            PlaneIsAliveCheck(item.Agent);
        }
        if (m_BlueAgentGroupCount <= 0 || m_RedAgentGroupCount <= 0)
        {
            /*redwins++;*/
            ResetScene();
        }

    }


    void Hit(IndivisualPlayer col)
    {

        // Touched goal.
        if (col.gameObject.CompareTag("goal") && (col.transform.tag == "blue"))
        {
            print("Blue Entered Goal");
            m_BlueAgentGroup.AddGroupReward(2f);
            m_RedAgentGroup.AddGroupReward(-2f);
            /*bluewins++;*/
            ResetScene();
        }
        
    }

    void StrikerHit(IndivisualPlayer col)
    {
        if (col.gameObject.CompareTag("red") && (col.transform.tag == "blue"))
        {
            print("red hit blue");
            m_BlueAgentGroup.AddGroupReward(-20f);
            m_RedAgentGroup.AddGroupReward(20f);
            ResetScene();
        }
    }

    void ClosestStrikerPunishment(IndivisualPlayer col)
    {
        if (col.tag == "blue")
        {
           // m_RedAgentGroup.AddGroupReward(-1.6f/(MaxEnvironmentSteps * Vector3.Magnitude(col.transform.localPosition - goal.transform.localPosition)));
            m_BlueAgentGroup.AddGroupReward(1.6f/(MaxEnvironmentSteps* Vector3.Magnitude(col.transform.localPosition - goal.transform.localPosition)));
        }
    }
    public void ResetScene()
    {
        m_ResetTimer = 0;
        fc = 0;
        /*episode_count++;*/
        //Reset Agents
        var randomPosX = Random.Range(-10f, 10f);
        var randomPosZ = Random.Range(-10f, 10f);
        foreach (var item in AgentsList)
        {
            randomPosX = Random.Range(-10f, 10f);
            randomPosZ = Random.Range(-10f, 10f);
            var newStartPos = groundPos + new Vector3(randomPosX, 0f, randomPosZ);
            var rot = item.Agent.rotSign * Random.Range(80.0f, 100.0f);
            var newRot = Quaternion.Euler(0, rot, 0);
            item.Agent.transform.SetPositionAndRotation(newStartPos, newRot);

            item.Rb.velocity = Vector3.zero;
            item.Rb.angularVelocity = Vector3.zero;
            item.Agent.EndEpisode();
            item.Agent.IsAlive = true;
        }
        randomPosX = Random.Range(-10f, 10f);
        randomPosZ = Random.Range(-10f, 10f);
        goal.transform.position = groundPos + new Vector3(randomPosX, 0f, randomPosZ);
        m_RedAgentGroupCount = 4;
        m_BlueAgentGroupCount = 4;

        //EndEpisode();
    }

    public void PlaneIsAliveCheck(IndivisualPlayer c)
    {
        if (!c.IsAlive)
        {
            c.gameObject.SetActive(false);
        }
        if (c.IsAlive)
        {
            c.gameObject.SetActive(true);
        }
    }

    public void JoinPatrol()
    {

    }

}

