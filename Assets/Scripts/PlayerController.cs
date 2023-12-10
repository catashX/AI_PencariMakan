using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [Header("Observation")]
    public GameObject target;
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
    public GameObject button;
    public foodSpawner spawner;
    public NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        initPos = transform.position;
        currentStarveTime = initStarveTime;
        initTransform = transform;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = button;
        spawner.deSpawnFood();
        relocateButton();
    }

    private void FixedUpdate()
    {
        if(target != null) { agent.SetDestination(target.transform.position); }
        doStarve();
        time.text = "time: " + string.Format("{0:0.00}", currentStarveTime);
        point.text = "point: " + points.ToString();
    }

    void doStarve() {
        currentStarveTime = Mathf.Min(currentStarveTime - Time.deltaTime, initStarveTime);
        if (currentStarveTime < 0) killx();
    }

    public void killx() {
        transform.localPosition = new Vector3(initPos.x, 0.25f, initPos.z);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        proceduralController.resetLegs();
        points = 0;
        currentStarveTime = initStarveTime;
        spawner.deSpawnFood();
        relocateButton();
    }

    public void OnAgentClickButton()
    {
        Debug.Log("clicked");
        target = spawner.respawnFood();
    }

    public void feed() {
        points += 1;
        currentStarveTime += Mathf.Min(currentStarveTime + timeGainPerFood, initStarveTime);
        target = button;
        relocateButton();
    }

    void relocateButton()
    {
        button.GetComponent<button>().foodEaten();
    }
}
