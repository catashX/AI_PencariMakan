using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    public bool isFoodSpawned;
    public bool relocate;
    public void spawnFood()
    {
        if (!isFoodSpawned) { isFoodSpawned = true; }
    }

    public void foodEaten()
    {
        if (isFoodSpawned) { isFoodSpawned = false; }
        if (!relocate) return;
        float lucky_x = Random.Range(-(1.3f), 1.3f);
        float lucky_z = Random.Range(-(1.3f), 1.3f);
        this.transform.localPosition = new Vector3(lucky_x, 0f, lucky_z);
    }

    public void resetButton()
    {
        isFoodSpawned = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFoodSpawned && other.gameObject.CompareTag("Player"))
        {
            if (!isFoodSpawned) { isFoodSpawned = true; }
            other.TryGetComponent<PlayerController>(out PlayerController target_player);
            other.TryGetComponent<WorkForFoodAgent>(out WorkForFoodAgent target_ai);
            if(target_ai != null) target_ai.OnAgentClickButton();
            if (target_player != null) target_player.OnAgentClickButton();
        }
    }
}
