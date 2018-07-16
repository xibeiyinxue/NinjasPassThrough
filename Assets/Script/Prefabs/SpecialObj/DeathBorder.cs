using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBorder : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (collider.GetComponent<IHealth>() != null)
            {
                GameManager.Instance.m_Player.DestroySelf();
            }
            else
            {
                Destroy(collider.gameObject);
            }
        }
    }
}
