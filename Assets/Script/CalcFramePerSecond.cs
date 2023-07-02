using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 日本語対応
public class CalcFramePerSecond : MonoBehaviour
{
    [SerializeField] private Text _text = null;
    private int _frameCount = 0;
    private float _prevTime = 0f;

    private void Update()
    {
        CalcFPS();
    }

    private void CalcFPS()
    {
        if (_text == null) return;

        _frameCount++;
        float time = Time.realtimeSinceStartup - _prevTime;

        if (time >= 0.1f)
        {
            _text.text = (_frameCount / time).ToString("F2");
            _frameCount = 0;
            _prevTime = Time.realtimeSinceStartup;
        }
    }
}
