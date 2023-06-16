using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 日本語対応
public class LifeManager : MonoBehaviour
{
    [SerializeField] private int _life = 5;
    [SerializeField] private UnityEvent _onReduceLife = new UnityEvent();

    public void ReduceLife(int damage)
    {
        _life -= damage;
        _onReduceLife?.Invoke();
    }
}
