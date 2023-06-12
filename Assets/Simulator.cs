using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// 日本語対応
public class Simulator : MonoBehaviour
{
    [SerializeField] private Boid[] _boids = null;
    [SerializeField] private Parameter _param = null;
    [SerializeField] private float _count = 1f;
    [SerializeField] private Color _color = Color.white;
    private List<Boid> _boidObjects = new List<Boid>();

    private void OnEnable()
    {
        for (int i = 0; i < _count; i++)
        {
            int n = Random.Range(0, _boids.Length);
            Boid boidObj = Instantiate(_boids[n], Random.insideUnitSphere, Random.rotation, transform);
            boidObj.Parameter = _param;
            _boidObjects.Add(boidObj);
            boidObj.Neighbers = _boidObjects;
        }
    }

    private void OnDrawGizmos()
    {
        if (!_param) return;

        Gizmos.color = _color;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one * _param.wallScale);
    }
}
