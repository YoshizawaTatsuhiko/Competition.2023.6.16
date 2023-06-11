using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Simulator : MonoBehaviour
{
    [Tooltip("居なくても問題なく動く")]
    [SerializeField] private Leader _leader = null;
    [SerializeField] private bool _isFollowLeader = true;
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
            Vector3 instantiatePos = Random.insideUnitSphere;

            if (_leader && _isFollowLeader) instantiatePos.y = 3f;
            Boid boidObj = Instantiate(_boids[n], instantiatePos, Random.rotation, transform);
            boidObj.Parameter = _param;
            _boidObjects.Add(boidObj);
            boidObj.Neighbers = _boidObjects;
            boidObj.Leader = _leader;
        }
    }

    private void OnDrawGizmos()
    {
        if (!_param) return;

        Gizmos.color = _color;
        Gizmos.DrawWireCube(transform.position, Vector3.one * _param.wallScale);
    }
}
