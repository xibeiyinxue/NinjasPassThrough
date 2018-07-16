using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinDoorTrigger : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && collider.GetComponent<IHealth>() != null)
        {
            LevelDirector.Instance.GameWin();
        }
    }
}
