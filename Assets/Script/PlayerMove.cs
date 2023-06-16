using Unity.VisualScripting;
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
        _rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown) _rigidbody.isKinematic = false;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 moveDir = input.normalized * _speed;
        moveDir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = moveDir;
    }
}
