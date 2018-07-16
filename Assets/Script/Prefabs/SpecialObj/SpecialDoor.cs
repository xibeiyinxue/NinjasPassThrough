using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

public class SpecialDoor : MonoBehaviour {
    [SerializeField]
    private Transform m_Visuals;
    [SerializeField]
    private GameObject m_KeyObj = null;

    private float m_distance;
    private float m_DetectionRange = 30f;

    private bool m_Switch = false;
    private int count = 0;
    private float m_CD = 0;

    private Transform _player;
    private bool m_KeyBool = false;

    private void Update()
    {
        if (m_KeyObj == null)
        {
            NotKey();
        }
        else
        {
            if (m_KeyObj.activeSelf == true)
            {
                m_KeyBool = false;
            }
            else if(m_KeyObj.activeSelf == false && m_KeyBool == false)
            {
                m_KeyBool = true;
                Move();
            }
        }
    }

    private void NotKey()
    {
        m_CD += Time.deltaTime;
        if (GameManager.Instance.m_Player != null)
        {
            _player = GameManager.Instance.m_Player.transform;
            m_distance = Vector3.Distance(_player.position, transform.position);
            if (m_distance >= m_DetectionRange)
            {
                m_Switch = false;
            }
        }

        if (m_Switch && count == 1 && m_CD >= 1.2f)
        {
            count++;
            m_CD = 0f;
            Move();
        }
        else if (!m_Switch && count == 2 && m_CD >= 1.2f)
        {
            count = 0;
            m_CD = 0f;
            ReMove();
        }
    }

    private void Move()
    {
        m_Visuals.DOMoveY(10,1).SetRelative();
    }

    private void ReMove()
    {
        m_Visuals.DOMoveY(-10f, 1).SetRelative();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (count == 0)
            {
                count++;
            }
            m_Switch = true;
        }
    }
}
