using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

// 日本語対応
public class Boid : MonoBehaviour
{
    /// <summary>スクリプタブルオブジェクトから入力されたパラメーター</summary>
    public Parameter Parameter { get; set; }
    public List<Boid> Neighbers { get; set; }
    public Rigidbody Rigidbody => _rb;
    private List<Boid> _neighbers = new List<Boid>();
    private Vector3 _accel = Vector3.zero;
    private Rigidbody _rb = null;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * Parameter.initSpeed;
    }

    private void Update()
    {
        UpdateMove();
        UpdateNeighber();
        UpdateSeparation();
        UpdateAlignment();
        UpdateCohesion();
    }

    /// <summary>自身の動きを更新する</summary>
    private void UpdateMove()
    {
        float time = Time.deltaTime;

        _rb.velocity += _accel * time;
        //Vector3 dir = _rb.velocity.normalized;
        //float speed = _rb.velocity.magnitude;
        //_rb.velocity = Mathf.Clamp(speed, Parameter.minSpeed, Parameter.maxSpeed) * dir;
        Mathf.Clamp(_rb.velocity.magnitude, Parameter.minSpeed, Parameter.maxSpeed);
        transform.position += _rb.velocity * time;

        transform.rotation = Quaternion.LookRotation(_rb.velocity);

        _accel = Vector3.zero;
    }

    /// <summary>自身の視界範囲内に移る隣人を更新する</summary>
    private void UpdateNeighber()
    {
        _neighbers.Clear();

        float sightRad = Mathf.Cos(Parameter.neighborFov * Mathf.Deg2Rad);
        float searchNeighberDistance = Parameter.neighborDistance;

        foreach (var other in Neighbers)
        {
            if(other == this) continue;

            Vector3 directionOfNeighber = other.transform.position - transform.position;
            float distanceFromNeighbers = directionOfNeighber.magnitude;

            if (distanceFromNeighbers < searchNeighberDistance)
            {
                Vector3 dir = directionOfNeighber.normalized;
                Vector3 foward = _rb.velocity.normalized;
                float sight = Vector3.Dot(dir, foward);

                if (sight < sightRad)
                {
                    _neighbers.Add(other);
                }
            }
        }
    }

    private void UpdateSeparation()
    {
        if (_neighbers.Count <= 0) return;

        Vector3 leaveDirectionForce = Vector3.zero;

        foreach (var neighber in _neighbers)
        {
            leaveDirectionForce += (transform.position - neighber.transform.position).normalized;
        }
        leaveDirectionForce /= _neighbers.Count;
        _accel += leaveDirectionForce * Parameter.separationWeight;
    }

    private void UpdateAlignment()
    {
        if (_neighbers.Count <= 0) return;

        Vector3 averageVelocity = Vector3.zero;

        foreach (var neighber in _neighbers)
        {
            averageVelocity += neighber.Rigidbody.velocity;
        }
        averageVelocity /= _neighbers.Count;
        _accel += (averageVelocity - _rb.velocity) * Parameter.alignmentWeight;
    }

    private void UpdateCohesion()
    {
        if (_neighbers.Count <= 0) return;

        Vector3 averagePosition = Vector3.zero;

        foreach (var neighber in _neighbers)
        {
            averagePosition += neighber.transform.position;
        }
        averagePosition /= _neighbers.Count;
        _accel += (averagePosition - transform.position) * Parameter.cohesionWeight;
    }
}
