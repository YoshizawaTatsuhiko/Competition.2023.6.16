using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
// 日本語対応
public class Leader : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    public Rigidbody Rigidbody => _rb;
    private Rigidbody _rb = null;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rb.velocity = new Vector3(
            Input.GetAxisRaw("Horizontal"), _rb.velocity.y, Input.GetAxisRaw("Vertical")) * _speed;
    }
}
