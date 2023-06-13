using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
// 日本語対応
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    private Rigidbody _rigidbody = null;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rigidbody.velocity = new Vector3(
            Input.GetAxisRaw("Horizontal"), _rigidbody.velocity.y, Input.GetAxisRaw("Vertical")) * _speed;
    }
}
