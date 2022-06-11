using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;


public enum Team
{
    Blue = 0,
    Red = 1
}

public class IndivisualPlayer : Agent
{
    
    public Team team;

    public int BeingChasedby;
    private int m_ResetTimer;
    public float agentRunSpeed = 2f;
    float m_Existential;
    //float m_LateralSpeed = 2f;
    float m_ForwardSpeed = 10f;
    public GameObject wall;
    public TeamCoop envController;
    public Rigidbody agentRb;
    public float framerate = 60f;
    public bool IsAlive = true;
    //DecisionTreeImplementation decisionTree;
    RayPerceptionOutput.RayOutput[] rayOutputs;
    BehaviorParameters m_BehaviorParameters;
    public Vector3 initialPos;
    public float rotSign;
    EnvironmentParameters m_ResetParams;



    public override void Initialize()
    {

        /*TeamCoop envController = GetComponentInParent<TeamCoop>();
        if (envController != null)
        {
            m_Existential = 1f / envController.MaxEnvironmentSteps;
        }
        else
        {
            m_Existential = 1f / MaxStep;
        }*/

        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        if (m_BehaviorParameters.TeamId == (int)Team.Blue)
        {
            team = Team.Blue;
            initialPos = new Vector3(transform.position.x - 5f, .5f, transform.position.z);
            rotSign = 1f;
        }
        else if(m_BehaviorParameters.TeamId == (int)Team.Red)
        {
            team = Team.Red;
            initialPos = new Vector3(transform.position.x + 5f, .5f, transform.position.z);
            rotSign = 1f;
        }
        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500;

        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    /*void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(purpleGoalTag)) //ball touched purple goal
        {
            envController.GoalTouched(Team.Blue);
        }
        if (col.gameObject.CompareTag(blueGoalTag)) //ball touched blue goal
        {
            envController.GoalTouched(Team.Purple);
        }
    }*/

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

       
        var forwardAxis = act[0];
        var rotateAxis = act[1];
        //var rightAxis = act[1];
        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                break;
            /*case 2:
               // dirToGo = transform.forward * -m_ForwardSpeed;
                break;*/
        }

        /*switch (rightAxis)
        {
            case 1:
                //dirToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                //dirToGo = transform.right * -m_LateralSpeed;
                break;
        }*/
 
        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        //agentRb.AddForce(dirToGo * agentRunSpeed,ForceMode.VelocityChange);
        transform.localPosition = transform.localPosition + (dirToGo * (agentRunSpeed / framerate));
    }
    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= envController.MaxEnvironmentSteps && envController.MaxEnvironmentSteps > 0)
        {
            envController.ResetScene();
        }
        if (transform.localPosition.x > 1250 || transform.localPosition.x < -1250 || transform.localPosition.z > 1250 || transform.localPosition.y < -10 || transform.localPosition.y > 10 || transform.localPosition.z < -1250)
        {
            envController.ResetScene();
        }
    }

    void Update()
    {
        
    }
    /*public void GoalDefence()
    {
        if (transform.tag == "blueAgent")
        {
            AddReward(1 - (float)m_ResetTimer / envController.MaxEnvironmentSteps);
            //m_PurpleAgentGroup.AddGroupReward(-1);
        }
        else if (transform.tag == "purpleAgent")
        {
            AddReward(1 - (float)m_ResetTimer / envController.MaxEnvironmentSteps);
            //m_BlueAgentGroup.AddGroupReward(-1);
        }
    }*/
    public override void OnActionReceived(ActionBuffers actionBuffers)

    { //if(transform.tag == "blue") { MoveAgent(actionBuffers.DiscreteActions); }
       MoveAgent(actionBuffers.DiscreteActions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActionsOut = actionsOut.DiscreteActions;
        //forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 2;
        }
        //right
       /* if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }*/
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(envController.goal.transform.localPosition);

    }
    void OnCollisionEnter(Collision c)
    {

        if (c.gameObject.CompareTag("goal") && transform.tag == "blue")
        {
            AddReward(20f);
            envController.ResetScene();
        }
        if (c.gameObject.CompareTag("reset_boundary") && (transform.tag == "blue" || transform.tag == "red"))
        {
            AddReward(-1f);
            envController.ResetScene();
        }
        if (c.gameObject.CompareTag("red") && transform.tag == "blue")
        {
            AddReward(-2f);
            envController.m_BlueAgentGroup.AddGroupReward(-20);
            //envController.ResetScene();
            //envController.m_BlueAgentGroup.UnregisterAgent(this);
            IsAlive = false;
            envController.m_BlueAgentGroupCount--;
        }
        if (c.gameObject.CompareTag("blue") && transform.tag == "red")
        {
            AddReward(2f);
            envController.m_BlueAgentGroup.AddGroupReward(20);
            //envController.m_BlueAgentGroup.UnregisterAgent(this);
            IsAlive = false;
            envController.m_RedAgentGroupCount--;

            //envController.ResetScene();
        }
    }

    void DetectEnemy()
    { 
       /* if(transform.tag == "red")
        {
            foreach (var ray in rayOutputs)
            {
                if (ray.HitTagIndex == 39){decisionTree.enemy = ray.HitGameObject;}
                else {decisionTree.enemy = null;}
            }
        }
        else{decisionTree.enemy = null;}

        if(transform.tag == "red")
        {
            foreach (var agent in envController.AgentsList)
            {
                if (agent.Agent.tag == "blue" && Vector3.Magnitude(agent.Agent.transform.position - transform.position)>=decisionTree.R_detect)
                {
                    decisionTree.DetectedEnemy = true;
                    decisionTree.enemy = agent.Agent;
                }
            }
        }
        */
    }

}
