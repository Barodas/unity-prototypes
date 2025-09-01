using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameLogicController : MonoBehaviour
{
    public static GameLogicController Instance;

    private const float TURNSTEP = 0.25f;

    private float _curStep;
    private bool _processingQueue;
    private GameObject _pausedAlert;

    private List<AgentMover> _agentQueue;

    private void Start()
    {
        Instance = this;
        _pausedAlert = GameObject.Find("Text_PausedAlert");
        _pausedAlert.SetActive(false);
    }

	private void Update ()
    {
        _curStep += Time.deltaTime;

        if(_curStep > TURNSTEP)
        {
            if(_agentQueue.Count > 0)
            {
                _processingQueue = true;
            }
            else
            {
                _processingQueue = false;
                _curStep = 0;
            }
        }

        ToggleTimeScale(_processingQueue);
        _pausedAlert.SetActive(_processingQueue);

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    ToggleTimeScale();
        //}
    }

    public void AddToQueue(AgentMover agent)
    {
        if(_agentQueue == null)
        {
            _agentQueue = new List<AgentMover>();
        }

        if(!_agentQueue.Contains(agent))
        {
            _agentQueue.Add(agent);
        }
    }

    public void RemoveFromQueue(AgentMover agent)
    {
        if(_agentQueue != null && _agentQueue.Contains(agent))
        {
            _agentQueue.Remove(agent);
        }
    }

    public void ToggleTimeScale()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void ToggleTimeScale(bool state)
    {
        Time.timeScale = state ? 0 : 1;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
