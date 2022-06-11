using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class TestBehaviorScript : Agent
{

    public Team team;
    private int m_ResetTimer;
    public float agentRunSpeed = 2f;
    float m_Existential;
    float m_LateralSpeed = 2f;
    float m_ForwardSpeed = 2f;
    public GameObject wall;
    public TestCoop envController;
    public Rigidbody agentRb;
    public int rotSign;
    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

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
        agentRb.AddForce(dirToGo * agentRunSpeed,
            ForceMode.VelocityChange);
    }

    /*void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= envController.MaxEnvironmentSteps && envController.MaxEnvironmentSteps > 0)
        {
            envController.ResetScene();
        }

    }*/
    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        MoveAgent(actionBuffers.DiscreteActions);
    }
}
