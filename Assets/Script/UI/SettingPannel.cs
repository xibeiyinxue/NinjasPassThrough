using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPannel : MonoBehaviour
{
    [SerializeField]
    private Slider _musicSlider;
    [SerializeField]
    private Slider _effectSlider;
    [SerializeField]
    private Toggle _silent;
    [SerializeField]
    private Text _left, _right, _jump, _attack;
    [SerializeField]
    private Text m_Prompt;
    [SerializeField]
    private Image _rewriteKeyBG;
    
    private KeyCode _leftKey, _rightKey, _jumpKey, _attackKey;
    private bool _leftCan, _rightCan, _jumpCan, _attackCan;
    private bool _returnNewKey;

    private float _currentValume;
    private KeyCode _currentKeyCode;

    void Start() {
        _rewriteKeyBG.gameObject.SetActive(false);
        AudioInit();

        InputKeyInit();
    }

    public void OnMusicSilder(float valume) {
        AudioManager.Instance.MusicValume = valume;
    }
    public void OnEffectSilder(float valume) {
        AudioManager.Instance.EffectValume = valume;
    }
    public void OnMainToggle(bool valume) {
        if (valume)
        {
            AudioManager.Instance.MainValume = -80;
        }
        else
        {
            AudioManager.Instance.MainValume = _currentValume;
        }
    }
    public void OnReAudio() {
            _musicSlider.value = 0;
            _effectSlider.value = 0;
            _silent.isOn = false;
            OnMusicSilder(0f);
            OnEffectSilder(0f);
            OnMainToggle(false);
    }
    private void AudioInit() {
        _musicSlider.value = AudioManager.Instance.MusicValume;
        _effectSlider.value = AudioManager.Instance.EffectValume;
        _currentValume = AudioManager.Instance.MainValume;
    }

    public void OnLeftNewKey() {
        _leftCan = true;
        PromptSetActiceTrue(_left);
        OnGUI();
    }
    public void OnRightNewKey() {
        _rightCan = true;
        PromptSetActiceTrue(_right);
        OnGUI();
    }
    public void OnJumpNewKey() {
        _jumpCan = true;
        PromptSetActiceTrue(_jump);
        OnGUI();
    }
    public void OnAttackNewKey() {
        _attackCan = true;
        PromptSetActiceTrue(_attack);
        OnGUI();
    }
    public void ReTureBool() {
        _returnNewKey = true;
    }
    public void CloseRewriteKey() {
        _returnNewKey = true;
        _currentKeyCode = KeyCode.None;
    }
    public void OnReInputKey() {
        InputManager.Instance.Left = _leftKey = KeyCode.LeftArrow; 
        InputManager.Instance.Right = _rightKey = KeyCode.RightArrow;
        InputManager.Instance.Jump = _jumpKey = KeyCode.Space;
        InputManager.Instance.Attack = _attackKey = KeyCode.Z;
        _left.text = _leftKey.ToString();
        _right.text = _rightKey.ToString();
        _jump.text = _jumpKey.ToString();
        _attack.text = _attackKey.ToString();
    }
    private void OnGUI()
    {
        if (Input.anyKeyDown)
        {
            Event e = Event.current;

            if (e != null && e.isKey && e.keyCode != KeyCode.None)
            {
                m_Prompt.text = "您确定要用" + e.keyCode.ToString() + "来替换原来的操作方法吗";
                _currentKeyCode = e.keyCode;
            }
        }
        else if (_returnNewKey)
        {
            if (_currentKeyCode != KeyCode.None)
            {
                if (_leftCan)
                {
                    _leftKey = _currentKeyCode;
                    _left.text = _leftKey.ToString();
                    _leftCan = false;
                    PromptSetActiceFalse();
                    _currentKeyCode = KeyCode.None;
                    InputManager.Instance.Left = _leftKey;
                }
                else if (_rightCan)
                {
                    _rightKey = _currentKeyCode;
                    _right.text = _rightKey.ToString();
                    _rightCan = false;
                    PromptSetActiceFalse();
                    _currentKeyCode = KeyCode.None;
                    InputManager.Instance.Right = _rightKey;
                }
                else if (_jumpCan)
                {
                    _jumpKey = _currentKeyCode;
                    _jump.text = _jumpKey.ToString();
                    _jumpCan = false;
                    PromptSetActiceFalse();
                    _currentKeyCode = KeyCode.None;
                    InputManager.Instance.Jump = _jumpKey;
                }
                else if (_attackCan)
                {
                    _attackKey = _currentKeyCode;
                    _attack.text = _attackKey.ToString();
                    _attackCan = false;
                    PromptSetActiceFalse();
                    _currentKeyCode = KeyCode.None;
                    InputManager.Instance.Attack = _attackKey;
                }
            }
            else if (_currentKeyCode == KeyCode.None)
            {
                if (_leftCan)
                {
                    _leftCan = false;
                    _left.text = _leftKey.ToString();
                    PromptSetActiceFalse();
                    InputManager.Instance.Left = _leftKey;
                }
                else if (_rightCan)
                {
                    _rightCan = false;
                    _right.text = _rightKey.ToString();
                    PromptSetActiceFalse();
                    InputManager.Instance.Right = _rightKey;
                }
                else if (_jumpCan)
                {
                    _jumpCan = false;
                    _jump.text = _jumpKey.ToString();
                    PromptSetActiceFalse();
                    InputManager.Instance.Jump = _jumpKey;
                }
                else if (_attackCan)
                {
                    _attackCan = false;
                    _attack.text = _attackKey.ToString();
                    PromptSetActiceFalse();
                    InputManager.Instance.Attack = _attackKey;
                }
            }
        }
    }
    private void PromptSetActiceTrue(Text text) {
        text.text = "";
        m_Prompt.text = "请输入任意键用以来更改您习惯的操作方法";
        _rewriteKeyBG.gameObject.SetActive(true);
    }
    private void PromptSetActiceFalse() {
        _returnNewKey = false;
        _rewriteKeyBG.gameObject.SetActive(false);
    }
    private void InputKeyInit() {
        _leftKey = InputManager.Instance.Left;
        _rightKey = InputManager.Instance.Right;
        _jumpKey = InputManager.Instance.Jump;
        _attackKey = InputManager.Instance.Attack;

        _left.text = _leftKey.ToString();
        _right.text = _rightKey.ToString();
        _jump.text = _jumpKey.ToString();
        _attack.text = _attackKey.ToString();
    }
}
