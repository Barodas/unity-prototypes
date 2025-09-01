using UnityEngine;

public enum AgentOrder
{
    NOORDER,
    HOLD,
    MOVE,
    ATTACK
}

public class AgentMover : MonoBehaviour {

    private const float _movespeed = 5;

    public int Health { get; private set; }
    public Projectile Fireball;
    private Vector3 _targetPos;
    private Vector3 _targetDirection;
    public bool IsSelectable { get; private set; }
    private AgentOrder _currentOrder;
    private Transform _focusTarget;
    private Renderer _renderer;
    private float _holdDuration;
    private float _castTime;
    private bool _projectileFired;
    private float _cooldownTime;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        Health = 100;
    }

	void Update ()
    {
        switch(_currentOrder)
        {
            case AgentOrder.NOORDER:
                GameLogicController.Instance.AddToQueue(this);
                IsSelectable = true;
                break;
            case AgentOrder.MOVE:
                if(_targetPos != null && (_targetPos - transform.position).magnitude < 2)
                {
                    _currentOrder = AgentOrder.NOORDER;
                }
                else
                {
                    _targetDirection = (_targetPos - transform.position).normalized;
                    _targetDirection.y = 0;
                    transform.Translate(_targetDirection * _movespeed * Time.deltaTime);
                }
                break;
            case AgentOrder.HOLD:
                if (_holdDuration < 0)
                {
                    _currentOrder = AgentOrder.NOORDER;
                }
                else
                {
                    _holdDuration -= Time.deltaTime;
                }
                break;
            case AgentOrder.ATTACK:
                if(_projectileFired)
                {
                    if (_cooldownTime < 0)
                    {
                        _currentOrder = AgentOrder.NOORDER; 
                    }
                    else
                    {
                        _cooldownTime -= Time.deltaTime;
                    }
                }
                else
                {
                    if (_castTime < 0)
                    {
                        Projectile projectile = Instantiate(Fireball, transform.position, Quaternion.identity) as Projectile;
                        projectile.SetTargetDirection(_targetDirection);
                        _cooldownTime = 1;
                        _projectileFired = true;
                    }
                    else
                    {
                        _castTime -= Time.deltaTime;
                    }
                }
                break;
            default:
                Debug.Log("AgentMover: " + name + " - Update(): _currentOrder switch has defaulted.");
                break;
        }

        _renderer.material.color = IsSelectable ? Color.blue : Color.white;

        //if(targetPos != null && (targetPos - transform.position).magnitude > 2)
        //{
        //    targetDirection = (targetPos - transform.position).normalized;
        //    targetDirection.y = 0;
        //    transform.Translate(targetDirection * _movespeed * Time.deltaTime);
        //}
	}

    public void SetMoveTarget(Vector3 target)
    {
        _targetPos = target;
        _currentOrder = AgentOrder.MOVE;
        RemoveFromQueue();
    }

    public void SetAttackTarget(Vector3 target)
    {
        _targetDirection = target;
        _castTime = 2;
        _projectileFired = false;
        _currentOrder = AgentOrder.ATTACK;
        RemoveFromQueue();
    }

    public void SetHoldOrder(float duration)
    {
        _holdDuration = duration;
        _currentOrder = AgentOrder.HOLD;
        RemoveFromQueue();
    }

    private void RemoveFromQueue()
    {
        IsSelectable = false;
        GameLogicController.Instance.RemoveFromQueue(this);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
}
