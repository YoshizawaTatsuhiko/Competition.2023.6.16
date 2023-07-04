using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Simulator : MonoBehaviour
{
    [SerializeField] Parameter _param = null;
    [SerializeField] LayerMask _notIgnoreLayer = 0;
    [SerializeField] private BoidBase[] _boids = null;
    [SerializeField] private int _generateCount = 10;

    private void Start()
    {
        if (_boids.Length <= 0) return;
        
        Vector3 generatePos = transform.position + Random.insideUnitSphere;
        
        for (int i = 0, random = 0; i < _generateCount; i++, random = Random.Range(0, _boids.Length))
        {
            var boid = Instantiate(_boids[random], generatePos, Random.rotation, transform);
            boid.Param = _param;
            boid.CenterOfSimulationArea = transform.position;
            boid.LayerMask = _notIgnoreLayer;
        }
    }

    private void OnDrawGizmos()
    {
        if (!_param) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _param.wallScale);
    }
}
