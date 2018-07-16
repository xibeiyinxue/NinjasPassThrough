using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : PersistentSingleton<AudioManager> {

    [SerializeField]
    private GameSetting t_GameSetting;
    [SerializeField]
    private AudioMixer t_Mixer;

    private float _musicValume;
    private float _effectValume;
    private float _mainValume;
    private bool _silent;

    protected override void Awake()
    {
        base.Awake();
        _musicValume = t_GameSetting.musicValume;
        _effectValume = t_GameSetting.effectValume;
        _mainValume = t_GameSetting.mainValume;

        t_Mixer.SetFloat("music", _musicValume);
        t_Mixer.SetFloat("effect",_effectValume);
        t_Mixer.SetFloat("main",_mainValume);
    }

    public float MusicValume
    {
        get { return _musicValume;}
        set
        {
            t_GameSetting.musicValume = value;
            _musicValume = value;
            t_Mixer.SetFloat("music", value);
        }
    }

    public float EffectValume
    {
        get { return _effectValume; }
        set
        {
            t_GameSetting.effectValume = value;
            _effectValume = value;
            t_Mixer.SetFloat("effect", value);
        }
    }

    public float MainValume
    {
        get { return _mainValume; }
        set
        {
            t_GameSetting.mainValume = value;
            _mainValume = value;
            t_Mixer.SetFloat("main", value);
        }
    }
}
