using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField]
    protected float speed = 0f; /*给予速度初始值*/
    [SerializeField]
    protected int power = 5; /*给予威力初始值*/
    protected string myTag = null; /*自身的标签*/

    protected void Update()
    {
        Move();
    }
    protected abstract void Move();/*由子类获取移动方式*/

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IHealth>() != null && !collision.CompareTag(myTag))/*如果 Collision 触碰到的该物体有生命的接口，并且标签与自己不相同*/
        {
            collision.GetComponent<IHealth>().Damage(power, this.gameObject);/*给触碰到的该物体造成基于威力值的伤害*/
            Destroy(this.gameObject);
        }
        else if(myTag == "Player" && collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}