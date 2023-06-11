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
    public Leader Leader { private get; set; }
    private List<Boid> _neighbers = new List<Boid>();
    private Rigidbody _rb = null;
    private Rigidbody _leaderRb = null;
    private Vector3 _accel = Vector3.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * Parameter.initSpeed;
    }

    private void Update()
    {
        LeaveWall();
        UpdateNeighber();
        UpdateDirectionOfTravel();
        UpdateMove();
    }

    private void UpdateMove()
    {
        float time = Time.deltaTime;

        _rb.velocity += _accel * time;
        Mathf.Clamp(_rb.velocity.magnitude, Parameter.minSpeed, Parameter.maxSpeed);
        transform.position += _rb.velocity * time;

        transform.rotation = Quaternion.LookRotation(_rb.velocity);

        _accel = Vector3.zero;
    }

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

    private void LeaveWall()
    {
        float scale = Parameter.wallScale * 0.5f;

        _accel +=
        CalcWallAvoidanceVector(-scale - transform.position.x, Vector3.right) +
        CalcWallAvoidanceVector(-scale - transform.position.y, Vector3.up) +
        CalcWallAvoidanceVector(-scale - transform.position.z, Vector3.forward) +
        CalcWallAvoidanceVector(scale - transform.position.x, Vector3.left) +
        CalcWallAvoidanceVector(scale - transform.position.y, Vector3.down) +
        CalcWallAvoidanceVector(scale - transform.position.z, Vector3.back);

        Vector3 CalcWallAvoidanceVector(float distance, Vector3 direction)
        {
            if (distance < Parameter.wallDistance)
            {
                return direction * (Parameter.wallWeight / Mathf.Abs(distance / Parameter.wallDistance));
            }
            return Vector3.zero;
        }
    }

    /// <summary>個体の分離・整列・結合を司る</summary>
    private void UpdateDirectionOfTravel()
    {
        if (_neighbers.Count <= 0) return;

        Vector3 leaveDirectionForce = Vector3.zero; // 集団から離れる方向のベクトル
        Vector3 averageVelocity = Vector3.zero;     // 集団の進行方向のベクトル
        Vector3 averagePosition = Vector3.zero;     // 集団の中心に近づくベクトル

        foreach (var neighber in _neighbers)
        {
            if(neighber == this) continue;

            leaveDirectionForce += (transform.position - neighber.transform.position).normalized;
            averageVelocity += Leader ? Leader.Rigidbody.velocity : neighber.Rigidbody.velocity;
            averagePosition += Leader ? Leader.transform.position : neighber.transform.position;
        }
        leaveDirectionForce /= _neighbers.Count;

        if (!Leader)
        {
            averageVelocity /= _neighbers.Count;
            averagePosition /= _neighbers.Count;
        }

        _accel +=
            leaveDirectionForce * Parameter.separationWeight +
            (averageVelocity - _rb.velocity) * Parameter.alignmentWeight +
            (averagePosition - transform.position) * Parameter.cohesionWeight;
    }
}
