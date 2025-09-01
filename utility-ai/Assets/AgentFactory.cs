using UnityEngine;

public class AgentFactory : MonoBehaviour
{
    protected static AgentFactory Instance;

    public GameObject AgentPrefab;

    void Start()
    {
        Instance = this;
    }

    public static BaseAgent CreateAgent(string name)
    {
        BaseAgent agent = Instantiate(Instance.AgentPrefab).GetComponent<BaseAgent>();
        agent.Initialise(name);
        return agent;
    }
}
