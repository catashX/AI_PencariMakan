using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodSpawner : MonoBehaviour
{
    public GameObject food;
    public Vector3 foodSpawnBoundaries;
    public List<GameObject> foodList;
    public bool isTransformPlaced;
    public Transform[] placement;
    private void Awake()
    {
        //respawnFood();
    }
    public GameObject respawnFood() {
        if (foodList.Count > 0) {
            foreach (var food in foodList) if(food != null) food.GetComponent<food>().kill_f();
            foodList.Clear();
        }
        if (isTransformPlaced)
        {
            int lucky = Random.Range(0, placement.Length);
            var tempx = Instantiate(food, gameObject.transform.parent);
            tempx.transform.localPosition = placement[lucky].localPosition;
            foodList.Add(tempx);
            return tempx;
        }
        else
        {
            float lucky_x = Random.Range(-(transform.localPosition.x + foodSpawnBoundaries.x), transform.localPosition.x + foodSpawnBoundaries.x);
            float lucky_z = Random.Range(-(transform.localPosition.z + foodSpawnBoundaries.z), transform.localPosition.z + foodSpawnBoundaries.z);
            var temp = Instantiate(food, gameObject.transform.parent);
            temp.transform.localPosition = new Vector3(lucky_x, 0.25f, lucky_z);
            foodList.Add(temp);
            return temp;
        }
    }

    public void deSpawnFood()
    {
        if (foodList.Count > 0)
        {
            foreach (var food in foodList) if (food != null) food.GetComponent<food>().kill_f();
            foodList.Clear();
        }
    }
}
