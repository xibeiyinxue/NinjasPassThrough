using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByTime : MonoBehaviour {
    [SerializeField]
    private float _destroyTime = 0f;/*删除掉该物体的时间*/
    private Animator anim;/*自身动画*/
    private AudioSource audioSource;/*自身音乐*/

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (anim)/*如果自身有动画*/
            _destroyTime = anim.GetCurrentAnimatorStateInfo(0).length;/*则删除掉该物体的时间设置为动画的时间*/
        if (audioSource)
            _destroyTime = audioSource.clip.length > _destroyTime ? audioSource.clip.length : _destroyTime;

        Destroy(this.gameObject, _destroyTime);
    }
}
