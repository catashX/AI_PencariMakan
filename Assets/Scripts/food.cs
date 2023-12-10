using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour
{
    public void kill_f() {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            other.TryGetComponent<PlayerController>(out PlayerController target_player);
            other.TryGetComponent<SearchForFoodAgent>(out SearchForFoodAgent target_ai);
            other.TryGetComponent<WorkForFoodAgent>(out WorkForFoodAgent target_ai2);
            if (target_player != null)
            {
                target_player.feed();
            }
            else {
                if (target_ai != null) target_ai.feed();
                if (target_ai2 != null) target_ai2.feed();
            }
            Destroy(gameObject);
        }
    }
}
