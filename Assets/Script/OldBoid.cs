using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

// 日本語対応
public class OldBoid : MonoBehaviour
{
    /// <summary>スクリプタブルオブジェクトから入力されたパラメーター</summary>
    public Parameter Parameter { private get; set; }
    public List<OldBoid> Neighbors { private get; set; }
    public Rigidbody Rigidbody => _rb;
    public Rigidbody CommanderRb { private get; set; }
    private List<OldBoid> _neighbors = new List<OldBoid>();
    private Rigidbody _rb = null;
    private Vector3 _accel = Vector3.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * Parameter.initSpeed;
    }

    private void Update()
    {
        UpdateNeighbor();
        LeaveWall();
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

    private void UpdateNeighbor()
    {
        _neighbors.Clear();

        float sightRad = Mathf.Cos(Parameter.neighborFov * Mathf.Deg2Rad);

        foreach (var other in Neighbors)
        {
            if(other == this) continue;

            Vector3 toNeighbor = other.transform.position - transform.position;
            float distanceToNeighbor = toNeighbor.magnitude;

            if (distanceToNeighbor < Parameter.neighborDistance)
            {
                Vector3 dir = toNeighbor.normalized;
                Vector3 forward = _rb.velocity.normalized;
                float sight = Vector3.Dot(dir, forward);

                if (sight > sightRad)
                {
                    _neighbors.Add(other);
                }
            }
        }
    }

    private void LeaveWall()
    {
        Vector3 toCommander = CommanderRb.position - transform.position;
        toCommander.y = 0;
        float distance = Parameter.wallScale - toCommander.magnitude;

        if (distance < Parameter.wallDistance)
        {
            _accel +=
                toCommander.normalized * (Parameter.wallWeight / Mathf.Abs(distance / Parameter.wallDistance));
        }
    }

    /// <summary>個体の分離・整列・結合を司る</summary>
    private void UpdateDirectionOfTravel()
    {
        //if (_neighbors.Count <= 0) return;

        Vector3 leaveDirectionForce = Vector3.zero; // 集団から離れる方向のベクトル
        //Vector3 averageVelocity = Vector3.zero;     // 集団の進行方向のベクトル
        Vector3 averagePosition = Vector3.zero;     // 集団の中心に近づくベクトル

        foreach (var neighber in _neighbors)
        {
            if(neighber == this) continue;

            leaveDirectionForce += (transform.position - neighber.transform.position).normalized;
            //averageVelocity += neighber.Rigidbody.velocity;
            averagePosition += neighber.transform.position;
        }
        //leaveDirectionForce /= _neighbors.Count;
        //averageVelocity /= _neighbors.Count;
        //averagePosition /= _neighbors.Count;
        leaveDirectionForce /= Neighbors.Count;
        averagePosition /= Neighbors.Count;

        _accel +=
            leaveDirectionForce * Parameter.separationWeight +
            //(averageVelocity - _rb.velocity) * Parameter.alignmentWeight +
            (CommanderRb.velocity - _rb.velocity) * Parameter.alignmentWeight +
            (averagePosition - transform.position) * Parameter.cohesionWeight;
        //(CommanderRb.position - transform.position) * Parameter.cohesionWeight;
        _accel.y = 0f;
    }
}
