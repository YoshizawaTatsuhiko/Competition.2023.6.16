using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;

// 日本語対応
public class GameManager : MonoBehaviour
{
    [SerializeField] private float _timeLimit = 60f;
    [SerializeField] private Text _timeText = null;
    [SerializeField] private UnityEvent _onGameClear = new UnityEvent();
    [SerializeField] private UnityEvent _onGameOver = new UnityEvent();
    public static GameManager Instance => _instance;
    private static GameManager _instance = null;
    private bool _isGameStart = false;

    private void Awake()
    {
        if (!_instance) _instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if (Input.anyKeyDown) _isGameStart = true;
        if(_isGameStart) _timeLimit = Mathf.Clamp(_timeLimit - Time.deltaTime, 0f, _timeLimit);
        if(_timeText) _timeText.text = _timeLimit.ToString("F2");

        if (_timeLimit == 0f)
        {
            _onGameClear?.Invoke();
        }
    }
}
