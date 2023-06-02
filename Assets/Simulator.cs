using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// 日本語対応
public class Simulator : MonoBehaviour
{
    [SerializeField] Boid _boids = null;
    [SerializeField] Parameter _param = null;
    [SerializeField] float _interval = 1f;
    private List<Boid> _boidObjects = new List<Boid>();
    private float _timer = 0f;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _interval)
        {
            Boid boidObj = Instantiate(_boids, transform.position, Quaternion.identity, transform);
            boidObj.Parameter = _param;
            _boidObjects.Add(boidObj);
            boidObj.Neighbers = _boidObjects;
            _timer = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        if (!_param) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.zero, _param.wallScale);
    }
}
