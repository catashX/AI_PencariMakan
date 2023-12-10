using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;
using Unity.VisualScripting;

public class WorkForFoodAgent : Agent
{
    [Header("Observation")]
    public GameObject button;
    public GameObject target;
    public bool useTime;
    public foodSpawner spawner;
    bool isFed;
    //public bool detectWall;


    [Header("movement")]
    public float speed;
    public Vector3 maxVel;
    Rigidbody rb;
    Transform initTransform;
    public AnimProceduralController proceduralController;
    [Header("Stats")]
    public float initStarveTime;
    public float timeGainPerFood;
    float currentStarveTime;
    Vector3 initPos;
    public float points;

    [Header("Obj")]
    public TextMeshProUGUI time;
    public TextMeshProUGUI point;

    [Header("Button")]
    public Vector2 buttonRandomDistanceBound;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPos = transform.localPosition;
        currentStarveTime = initStarveTime;
    }
    public override void OnEpisodeBegin()
    {
        if (!isFed)
        {
            transform.localPosition = new Vector3(initPos.x,0.25f,initPos.z);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            proceduralController.resetLegs();
            points = 0;
            currentStarveTime = initStarveTime;
        }
        spawner.deSpawnFood();
        relocateButton();
        isFed = false;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(button.GetComponent<button>().isFoodSpawned ? 1 : 0);
        Vector3 buttonDir = (button.transform.localPosition - transform.localPosition).normalized;
        sensor.AddObservation(buttonDir.x);
        sensor.AddObservation(buttonDir.z);
        if (target != null)
        {
            Vector3 dir = (target.transform.localPosition - transform.localPosition).normalized;
            sensor.AddObservation(dir.x);
            sensor.AddObservation(dir.z);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //base.Heuristic(actionsOut);
        ActionSegment<float> continuousactions = actionsOut.ContinuousActions;
        continuousactions[0] = Input.GetAxisRaw("Horizontal");
        continuousactions[1] = Input.GetAxisRaw("Vertical");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //base.OnActionReceived(actions);
        Vector3 input = new Vector3(actions.ContinuousActions[0], 0, actions.ContinuousActions[1]);
        rb.AddForce(speed * input, ForceMode.Impulse);
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxVel.x, maxVel.x), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -maxVel.z, maxVel.z));
    }

    private void FixedUpdate()
    {
        if (useTime)
        {
            doStarve();
            time.text = "time: " + string.Format("{0:0.00}", currentStarveTime);
        }
        point.text = "point: " + points.ToString();
    }

    void doStarve()
    {
        currentStarveTime = Mathf.Min(currentStarveTime - Time.deltaTime, initStarveTime);
        if (currentStarveTime < 0) killx();
    }

    public void killx()
    {
        SetReward(-1f);
        EndEpisode();
    }

    public void OnAgentClickButton() {
        Debug.Log("clicked");
        SetReward(+1f);
        target = spawner.respawnFood();
    }

    public void feed()
    {
        points += 1;
        SetReward(+1f);
        currentStarveTime = Mathf.Min(currentStarveTime + timeGainPerFood, initStarveTime);
        isFed = true;
        relocateButton();
        EndEpisode();
    }

    void relocateButton()
    {
        button.GetComponent<button>().foodEaten();
    }
}
