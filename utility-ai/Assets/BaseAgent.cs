using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BaseAgent : MonoBehaviour, IDamageable
{
    [SerializeField]
    [ShowOnly]
    private string _name;
    public string Name { get { return _name; } private set { _name = value; } }

    [SerializeField]
    [ShowOnly]
    private int _health;
    public int Health { get { return _health; } private set { _health = value < 0 ? 0 : value; } }

    [SerializeField]
    [ShowOnly]
    private int _strength;
    public int Strength { get { return _strength; } private set { _strength = value < 0 ? 0 : value; } }

    [SerializeField]
    [ShowOnly]
    private Action _action;
    public Action CurrentAction { get { return _action; } private set { _action = value; } }

    private int _destroyThreshold;
    private float timer;

    public BaseWeapon Weapon { get; private set; }
    
    private BaseAgent _target;

    public void Initialise(string name)
    {
        Name = name;
        Health = 100;
        Strength = 100;
        _destroyThreshold = 50;
    }

    private void Start()
    {
        AgentController.Instance.AIUpdate += UpdateAI;


        // TODO: create agent/assign stats.
    }

    private void Update()
    {
        // updateAI will set the current ai state
        // update will perform that ai states functionality in real time
        // attacks need a cooldown

        switch (CurrentAction)
        {
            case Action.SEARCH:
                if(timer > 0.5)
                {
                    FindTarget();
                    timer = 0;
                }
                break;
            case Action.MOVEAWAY:
                Move((_target != null ? (Vector2)_target.transform.position : Vector2.zero), false);
                break;
            case Action.MOVETO:
                Move((_target != null ? (Vector2)_target.transform.position : Vector2.zero), true);
                break;
            case Action.ATTACK:
                if (_target != null)
                {
                    if (timer > 1)
                    {
                        Attack(_target);
                        timer = 0;
                    }
                } 
                else
                    Debug.Log(Name + " Tried to attack with no target");
                break;
        }

        timer += Time.deltaTime;
    }

    private void UpdateAI()
    {
        // TODO: use  UtilityAI tables to determine best action to take

        //Dictionary<Action, int> scores = new Dictionary<Action, int>();

        // How do we handle targeting when target dies?
        if (_target != null)
        {
            if (_target.Health <= 0)
                _target = null;
        }

        // Behaviour tree for initial tests. Need to come up with a good way to implement UtilityAI
        if (_target == null)
        {
            CurrentAction = Action.SEARCH;
        }
        else if(Health < 10)
        {
            CurrentAction = Action.MOVEAWAY;
        }
        else if(Vector2.Distance(transform.position, _target.transform.position) > 1) // arbitrary melee range since no weapons exist yet
        {
            CurrentAction = Action.MOVETO;
        }
        else
        {
            CurrentAction = Action.ATTACK;
        }
    }

    public void AdjustStrength(int amount)
    {
        Strength += amount;
        if(Strength <= 0)
        {
            Strength = 0;
            // TODO: Agent should collapse/pass out/something when they lose all strength
        }
    }

    #region Actions

    private void Move(Vector2 targetPosition, bool moveToward)
    {
        Vector2 myPosition = transform.position;
        Vector2 direction;
        if (moveToward)
            direction = (targetPosition - myPosition).normalized;
        else
            direction = (myPosition - targetPosition).normalized;

        transform.position = (Vector2)transform.position + (direction * Time.deltaTime);
    }

    //private void ChargeAt(Vector2 targetPosition)
    //{
    //
    //}

    private void Attack(BaseAgent target)
    {
        Conflict.Attack(this, target);
    }

    private void FindWeapon()
    {
        // Check vision range for a weapon and move towards
        // If none check for a container that could contain a weapon
    }

    private void FindTarget()
    {
        _target = AgentController.Instance.FindTarget(this);
    }

    #endregion // Actions

    #region IDamageable Implementation

    public void AdjustHealth(int amount)
    {
        Health += amount;

        if(Health <= 0)
        {
            Die();

            if (amount >= _destroyThreshold)
                Destroy();                
        }
    }

    public void Die()
    {
        AgentController.Instance.AIUpdate -= UpdateAI;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    #endregion // IDamageable Implementation
}
