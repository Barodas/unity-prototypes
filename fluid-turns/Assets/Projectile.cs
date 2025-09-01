using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _armTime = 0.5f;
    private int _damage = 50;
    private float _movespeed = 10;
    private Vector3 _targetDirection;

	void Start ()
    {
        GetComponent<Renderer>().material.color = Color.red;
	}
	
	void Update ()
    {
        if(_armTime > 0)
        {
            _armTime -= Time.deltaTime;
        }
        
        transform.Translate(_targetDirection * _movespeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_armTime < 0)
        {
            AgentMover target = other.GetComponent<AgentMover>();
            if (target != null)
            {
                target.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }

    public void SetTargetDirection(Vector3 targetDirection)
    {
        _targetDirection = targetDirection;
    }
}
