using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour {
    [SerializeField] private Text _TimerText = null;
    [SerializeField] private GameObject[] _LifeIcons;

    private LevelDirector direvtor;

    private int t, m, s;
    
    private void Start()
    {
        direvtor = LevelDirector.Instance;
    }

    void Update () {
        t = direvtor.t;
        m = direvtor.m;
        s = direvtor.s;
        _TimerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t, m, s);

        for (int i = 0; i < _LifeIcons.Length; i++)
        {
            _LifeIcons[i].SetActive(i < direvtor.PlayerLifeCount);
        }
    }
}
