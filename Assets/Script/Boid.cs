using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Boid : MonoBehaviour
{
    public Parameter Param { get; set; }
    public Vector3 Center { get; set; }
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }

    private Vector3 _accel = Vector3.zero;
    private Collider[] _surroundingCollider = null;
    private float _timer = 0f;

    private void Start()
    {
        Position = transform.position;
        Velocity = transform.forward * Param.initSpeed;
        _surroundingCollider = new Collider[Param.maxNeighborsToSearch];
    }

    private void Update()
    {
        LeaveWall();
        UpdateMove(Time.deltaTime);
    }

    private void UpdateMove(float deltaTime)
    {
        Velocity += _accel * deltaTime;
        Velocity = Mathf.Clamp(Velocity.magnitude, Param.minSpeed, Param.maxSpeed) * Velocity.normalized;
        Position += Velocity * deltaTime;

        transform.position = Position;
        if (Velocity != Vector3.zero) transform.rotation = Quaternion.LookRotation(Velocity);

        _accel = Vector3.zero;
    }

    private void LeaveWall()
    {
        Vector3 toCenter = Center - transform.position;
        float distance = Param.wallScale - toCenter.magnitude;

        if (distance < Param.wallDistance)
        {
            _accel += toCenter.normalized * (Param.wallWeight / Mathf.Abs(distance / Param.wallDistance));
        }
    }

    private void UpdateNeighbor()
    {
        int n = Physics.OverlapSphereNonAlloc(
            transform.position, Param.searchNeighborRadius, _surroundingCollider, LayerMask.GetMask("Ignore Raycast"));

        for (int i = 0; i < n; i++)
        {
            if (_surroundingCollider[i].TryGetComponent(out Boid boid))
            {
                
            }
        }
    }

    /// <summary>一定時間が経過したかを計測し、結果を返す</summary>
    /// <param name="interval"></param>
    /// <param name="timer"></param>
    /// <param name="deltaTime"></param>
    /// <returns>経過した -> true | 経過していない -> false</returns>
    private bool CheckIntervalTimer(float interval, ref float timer,  float deltaTime)
    {
        timer += deltaTime;

        if (timer > interval)
        {
            timer = 0f;
            return true;
        }
        else return false;
    }
}
