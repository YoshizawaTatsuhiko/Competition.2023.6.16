using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Simulator : MonoBehaviour
{
    [SerializeField] private Transform _commander = null;
    [SerializeField] private GameObject[] _boids = null;
    [SerializeField] private Parameter _param = null;
    [SerializeField] private float _count = 1f;
    [SerializeField] private Color _color = Color.white;
    private List<Boid> _boidObjects = new List<Boid>();

    private void OnEnable()
    {
        for (int i = 0; i < _count; i++)
        {
            int n = Random.Range(0, _boids.Length);
            Vector3 instantiatePos = transform.position + Random.insideUnitSphere;
            GameObject go = Instantiate(_boids[n], instantiatePos, Random.rotation, transform);
            Boid boidObj = null;

            if(go.TryGetComponent(out Boid boid)) boidObj = boid;
            else boidObj = go.AddComponent<Boid>();

            boidObj.Parameter = _param;
            boidObj.Neighbors = _boidObjects;
            boidObj.Commander = transform;
            _boidObjects.Add(boidObj);
        }
    }

    private void OnDrawGizmos()
    {
        if (!_param) return;

        Gizmos.color = _color;
        Gizmos.DrawWireSphere(transform.position, _param.wallScale);
    }
}
