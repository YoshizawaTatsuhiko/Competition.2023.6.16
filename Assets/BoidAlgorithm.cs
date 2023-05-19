using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

// 日本語対応
public class BoidAlgorithm : MonoBehaviour
{
    public Parameter Parameter { get; set; }
    private Vector3 _accel = Vector3.zero;
    private List<BoidAlgorithm> neighber = new List<BoidAlgorithm>();
    private Rigidbody _rb = null;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void UpdateMove()
    {
        float time = Time.deltaTime;

        _rb.velocity = _accel * time;
        Vector3 dir = _rb.velocity.normalized;
        float speed = _rb.velocity.magnitude;
        _rb.velocity = Mathf.Clamp(speed, Parameter.minSpeed, Parameter.maxSpeed) * dir;
    }
}
