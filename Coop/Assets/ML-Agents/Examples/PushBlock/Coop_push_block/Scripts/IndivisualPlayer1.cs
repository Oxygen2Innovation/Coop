using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;

public class IndivisualPlayer1 : Agent
{
    public float agentRunSpeed = 2f;
    [HideInInspector]
    public Team team;

    private int m_ResetTimer;
    
    float m_Existential;
    float m_LateralSpeed;
    float m_ForwardSpeed;
    public GameObject wall;
    public TeamCoop envController;

    [HideInInspector]
    public Rigidbody agentRb;
    //SoccerSettings m_SoccerSettings;
    BehaviorParameters m_BehaviorParameters;
    public Vector3 initialPos;
    public float rotSign;
    //VectorSensor sensor1;
    EnvironmentParameters m_ResetParams;



    public override void Initialize()
    {


        TeamCoop envController = GetComponentInParent<TeamCoop>();
        if (envController != null)
        {
            m_Existential = 1f / envController.MaxEnvironmentSteps;
        }
        else
        {
            m_Existential = 1f / MaxStep;
        }

        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        if (m_BehaviorParameters.TeamId == (int)Team.Blue)
        {
            team = Team.Blue;
            initialPos = new Vector3(transform.position.x - 5f, .5f, transform.position.z);
            rotSign = 1f;
        }
        else
        {
            team = Team.Red;
            initialPos = new Vector3(transform.position.x + 5f, .5f, transform.position.z);
            rotSign = 1f;
        }
        //m_SoccerSettings = FindObjectOfType<SoccerSettings>();
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
        var rightAxis = act[1];
        var rotateAxis = act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                break;
            case 2:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                dirToGo = transform.right * -m_LateralSpeed;
                break;
        }

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
        agentRb.AddForce(dirToGo * agentRunSpeed, ForceMode.VelocityChange);
    }
    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= envController.MaxEnvironmentSteps && envController.MaxEnvironmentSteps > 0)
        {
            envController.ResetScene();
        }

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

    {
        MoveAgent(actionBuffers.DiscreteActions);
        if (this.tag == "blue")
        {
            print("blue move");

        }
        if (this.tag == "red")
        {
            print("red move");
        }
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
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }
        //right
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);

    }
    void OnCollisionEnter(Collision c)
    {

        if (c.gameObject.CompareTag("goal") && transform.tag == "blue")
        {
            print("Blue Entered Goal");
            AddReward(.2f);
            envController.ResetScene();
            //EndEpisode();
        }
        /* if (c.gameObject.CompareTag("blueGoal") && transform.tag == "purpleAgent")
         {
             AddReward(.2f);
             envController.ResetScene();
             //EndEpisode();
         }*/
    }

}
