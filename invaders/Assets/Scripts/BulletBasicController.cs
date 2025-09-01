using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBasicController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CircleCollider2D _col;

    private int _damage = 5;
    private Vector2 _moveDirection;
    private float _moveSpeed;

    public int Damage { get { return _damage; } }
    public Vector2 MoveDirection { get { return _moveDirection; } }
    public float MoveSpeed { get { return _moveSpeed; } }

	void Start ()
    {
        _col = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();

        _rb.AddForce(_moveDirection * _moveSpeed, ForceMode2D.Impulse);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetBulletParams(Vector2 directionVector, float speed, string tag)
    {
        _moveDirection = directionVector;
        _moveSpeed = speed;
        gameObject.tag = tag;
    }
}
