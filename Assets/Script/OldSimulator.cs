using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class OldSimulator : MonoBehaviour
{
    [SerializeField] private Rigidbody _commanderRb = null;
    [SerializeField] private GameObject[] _boids = null;
    [SerializeField] private Parameter _param = null;
    [SerializeField] private int _count = 1;
    [SerializeField] private Color _color = Color.white;
    private List<OldBoid> _boidObjects = new List<OldBoid>();

    private void OnEnable()
    {
        List<Vector3> instantiatePos = new List<Vector3>();
        instantiatePos.Add(_commanderRb.position);

        while (instantiatePos.Count <= _count)
        {
            Vector3 randomVec = Random.insideUnitSphere * _param.wallScale;
            randomVec.z = randomVec.y;
            randomVec.y = 0f;
            bool isOverlapObject = false;

            for (int i = 0; i < instantiatePos.Count; i++)
            {
                if (Physics.CheckSphere(instantiatePos[i], 0.5f, LayerMask.GetMask("Ignore Raycast")))
                {
                    isOverlapObject = true;
                    break;
                }
            }

            if (isOverlapObject) continue;

            Vector3 generatePos = _commanderRb.position + randomVec;
            int n = Random.Range(0, _boids.Length);
            GameObject go = Instantiate(_boids[n], generatePos, Quaternion.LookRotation(_commanderRb.position), transform);
            instantiatePos.Add(generatePos);
            OldBoid boidObj = null;

            if (go.TryGetComponent(out OldBoid boid)) boidObj = boid;
            else boidObj = go.AddComponent<OldBoid>();

            boidObj.Parameter = _param;
            boidObj.Neighbors = _boidObjects;
            boidObj.CommanderRb = _commanderRb;
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
