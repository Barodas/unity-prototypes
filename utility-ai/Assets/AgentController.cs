using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Action
{
    SEARCH,
    MOVETO,
    MOVEAWAY,
    ATTACK
}

public enum Scorer
{
    NOTARGET,
    OUTSIDEATTACKRANGE,
    INSIDEATTACKRANGE,
    LOWHP
}

public class AgentController : MonoBehaviour
{
    public static AgentController Instance;

    public delegate void UpdateAI();
    public UpdateAI AIUpdate;

    // TEMPORARY
    public List<BaseAgent> agents;
    public static int searchLimit;

    void Awake()
    {
        Instance = this;
        agents = new List<BaseAgent>();
    }

    void Start()
    {
        // TEMPORARY
        // Generate some agents
        agents.Add(AgentFactory.CreateAgent("Kyle"));
        agents.Add(AgentFactory.CreateAgent("Jason"));

        agents[0].transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        agents[1].transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
    }

    float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1)
        {
            if (AIUpdate != null)
            {
                AIUpdate();
            }
            timer = 0;
        }
        
    }

    // TEMPORARY
    public BaseAgent FindTarget(BaseAgent self)
    {
        // This is incredibly bad/inefficient and has the potential to get stuck in endless loops.
        BaseAgent target = agents[Random.Range(0, agents.Count)];
        if (target != self)
        {
            searchLimit = 0;
            return target;
        }    
        else if(searchLimit > 10)
        {
            searchLimit = 0;
            return null;
        }
        else
        {
            searchLimit++;
            return FindTarget(self);
        }
    }
}
