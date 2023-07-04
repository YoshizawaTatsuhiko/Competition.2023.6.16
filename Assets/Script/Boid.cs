using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Boid : BoidBase
{
    private Collider[] _surroundingCollider = null;
    private List<BoidBase> _neighbors = new List<BoidBase>();
    private float _timer = 0f;

    private void Start()
    {
        Position = transform.position;
        Velocity = transform.up * Param.initSpeed;
        _surroundingCollider = new Collider[Param.maxNeighborsToSearch];
    }

    private void Update()
    {
        LeaveWall();

        if (CheckInterval(Param.intervalToSimulate, ref _timer, Time.deltaTime))
        {
            UpdateNeighbor();
            UpdateGroup();
        }
        UpdateMove(Time.deltaTime);
    }

    private void UpdateMove(float deltaTime)
    {
        Velocity += Accel * deltaTime;
        Velocity = Mathf.Clamp(Velocity.magnitude, Param.minSpeed, Param.maxSpeed) * Velocity.normalized;
        Position += Velocity * deltaTime;

        transform.position = Position;
        if (Velocity != Vector3.zero) transform.rotation = Quaternion.LookRotation(Velocity);

        Accel = Vector3.zero;
    }

    private void LeaveWall()
    {
        Vector3 toCenter = CenterOfSimulationArea - transform.position;
        float distance = Param.wallScale - toCenter.magnitude;

        if (distance < Param.wallDistance)
        {
            Accel += toCenter.normalized * (Param.wallWeight / Mathf.Abs(distance / Param.wallDistance));
        }
    }

    /// <summary>近傍にいる仲間を探索する</summary>
    private void UpdateNeighbor()
    {
        _neighbors.Clear();

        int n = Physics.OverlapSphereNonAlloc(
            transform.position, Param.neighborDistance, _surroundingCollider, LayerMask);

        for (int i = 0; i < n; i++)
        {
            if (_surroundingCollider[i].gameObject == this) continue;

            Collider othersObj = _surroundingCollider[i];

            if (othersObj.TryGetComponent(out BoidBase boid))
            {
                float sightRad = Param.neighborFov * Mathf.Deg2Rad;
                Vector3 otherPos = othersObj.transform.position;
                Vector3 neighborDir = (otherPos - Position).normalized;
                Vector3 forward = Velocity.normalized;
                float sight = Vector3.Dot(neighborDir, forward);

                if (sight < sightRad)
                {
                    _neighbors.Add(boid);
                }
            }
        }
    }

    /// <summary>集団に対する 分離・整列・結合 を制御する</summary>
    private void UpdateGroup()
    {
        if (_neighbors.Count <= 0) return;

        Vector3 leaveNeighbor = Vector3.zero;
        Vector3 averageVelocity = Vector3.zero;
        Vector3 averagePosition = Vector3.zero;

        foreach (var neighbor in _neighbors)
        {
            leaveNeighbor += (transform.position - neighbor.Position).normalized;
            averageVelocity += neighbor.Velocity;
            averagePosition += neighbor.Position;
        }
        leaveNeighbor /= _neighbors.Count;
        averageVelocity /= _neighbors.Count;
        averagePosition /= _neighbors.Count;

        Accel +=
            leaveNeighbor * Param.separationWeight +
            (averageVelocity - Velocity) * Param.alignmentWeight +
            (averagePosition - Position) * Param.cohesionWeight;
    }

    /// <summary>一定時間が経過したかを計測し、結果を返す</summary>
    /// <param name="interval"></param>
    /// <param name="timer"></param>
    /// <param name="deltaTime"></param>
    /// <returns>経過した -> true | 経過していない -> false</returns>
    private bool CheckInterval(float interval, ref float timer, float deltaTime)
    {
        if (timer > interval)
        {
            timer = 0f;
            return true;
        }
        timer += deltaTime;
        return false;
    }
}
