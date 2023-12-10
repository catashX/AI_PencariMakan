using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;

public class SearchForFoodAgent : Agent
{
    [Header("Observation")]
    public GameObject target;
    public bool useTime;
    public foodSpawner spawner;
    bool isFed;
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
            transform.localPosition = new Vector3(initPos.x, 0.25f, initPos.z);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            proceduralController.resetLegs();
            points = 0;
            currentStarveTime = initStarveTime;
        }
        target = spawner.respawnFood();
        isFed = false;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(this.transform.localPosition);
        Vector3 dir = (target.transform.localPosition - transform.localPosition).normalized;
        if (target != null)
        {
            sensor.AddObservation(dir.x);
            sensor.AddObservation(dir.z);
        }
        else {
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
        Vector3 input = new Vector3(actions.ContinuousActions[0],0,actions.ContinuousActions[1]);
        rb.AddForce(speed * input, ForceMode.Impulse);
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxVel.x, maxVel.x), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -maxVel.z, maxVel.z));
    }

    private void FixedUpdate()
    {
        if (useTime)
        {
            doStarve();
            time.text = "time: " + string.Format("{0:0.00}",currentStarveTime);
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

    public void feed()
    {
        points += 1;
        SetReward(+1f);
        currentStarveTime = Mathf.Min(currentStarveTime + timeGainPerFood, initStarveTime);
        isFed = true;
        EndEpisode();
    }
}
