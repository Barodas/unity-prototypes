using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject BulletSpawnPoint;

    private int _health = 1;
    private float _shootTimer;
    private float _shootCooldown = 5f;
    private float _shootFrequency = 0.05f;

    void Start ()
    {

	}
	
	void Update ()
    {
        if(_health < 0)
        {
            Destroy(gameObject);
        }

        if (_shootTimer <= 0 && Random.Range(0f,1f) < _shootFrequency)
        {
            GameObject bullet = Instantiate(BulletPrefab, BulletSpawnPoint.transform, false);
            bullet.GetComponent<BulletBasicController>().SetBulletParams(Vector2.down, 10, "BulletEnemy");
            _shootTimer = _shootCooldown;
        }

        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "BulletFriendly")
        {
            _health -= col.gameObject.GetComponent<BulletBasicController>().Damage;
            Destroy(col.gameObject);
        }
    }
}
