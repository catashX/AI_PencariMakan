using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.TryGetComponent<PlayerController>(out PlayerController target_player);
            collision.gameObject.TryGetComponent<SearchForFoodAgent>(out SearchForFoodAgent target_ai);
            collision.gameObject.TryGetComponent<WorkForFoodAgent>(out WorkForFoodAgent target_ai2);
            if (target_player != null)
            {
                target_player.killx();
            }
            else
            {
                if (target_ai != null) target_ai.killx();
                if (target_ai2 != null) target_ai2.killx();
            }
        }
    }
}
