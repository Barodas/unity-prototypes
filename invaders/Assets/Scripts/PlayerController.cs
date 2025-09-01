using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject BulletSpawnPoint;
    private Rigidbody2D _rb;

    private int _health = 15;
    private float _moveSpeed = 30f;
    private float _shootTimer;
    private float _shootCooldown = 0.2f;

    public float MoveSpeed { get { return _moveSpeed; } }

	void Start ()
    {
        _rb = GetComponent<Rigidbody2D>();
	}
	
    void Update()
    {
        if(_shootTimer <= 0 && Input.GetKey(KeyCode.Space))
        {
            GameObject bullet = Instantiate(BulletPrefab, BulletSpawnPoint.transform, false);
            bullet.GetComponent<BulletBasicController>().SetBulletParams(Vector2.up, 10, "BulletFriendly");
            _shootTimer = _shootCooldown;
        }

        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }
    }

	void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        _rb.AddForce(movement * _moveSpeed);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("BulletEnemy"))
        {
            _health -= col.gameObject.GetComponent<BulletBasicController>().Damage;
            Destroy(col.gameObject);
        }
    }
}
