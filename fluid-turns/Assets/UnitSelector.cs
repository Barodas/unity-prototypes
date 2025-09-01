using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitSelector : MonoBehaviour
{
    private const string NoSelection = "No Selection";
    private AgentMover _currentSelection;
    private Text _text;
    private GameObject _agentOrderPanel; 

    private AgentOrder _currentOrder;

	void Start ()
    {
        _text = GameObject.Find("Text_CurrentSelection").GetComponent<Text>();
        _text.text = NoSelection;
        _agentOrderPanel = GameObject.Find("Panel_AgentOrders");
        _agentOrderPanel.SetActive(false);
    }
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.tag == "Selectable" && hit.transform.GetComponent<AgentMover>().IsSelectable)
                {
                    _currentSelection = hit.transform.gameObject.GetComponent<AgentMover>();
                    _text.text = _currentSelection.name;
                }

                if (hit.transform.gameObject.tag == "Ground")
                {
                    ClearSelection();
                }
            }
        }

        if(_currentSelection != null)
        {
            _agentOrderPanel.SetActive(true);

            switch (_currentOrder)
            {
                case AgentOrder.NOORDER:
                    break;
                case AgentOrder.MOVE:
                    Vector3 move = MoveOrder();
                    if (move != Vector3.zero)
                    {
                        _currentSelection.SetMoveTarget(move);
                        ClearSelection();
                    }
                    break;
                case AgentOrder.HOLD:
                    _currentSelection.SetHoldOrder(3);
                    ClearSelection();
                    break;
                case AgentOrder.ATTACK:
                    Vector3 attack = AttackOrder();
                    if (attack != Vector3.zero)
                    {
                        _currentSelection.SetAttackTarget(attack);
                        ClearSelection();
                    }
                    break;
                default:
                    Debug.Log("UnitSelector - Update(): _currentOrder switch has defaulted.");
                    break;
            }
        }
        else
        {
            _agentOrderPanel.SetActive(false);
        }

        //if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        //{
        //    RaycastHit hit;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.transform.gameObject.tag == "Ground" && _currentSelection != null)
        //        {
        //            _currentSelection.SetMoveTarget(hit.point);
        //        }
        //    }
        //}
    }

    private void ClearSelection()
    {
        _currentOrder = AgentOrder.NOORDER;
        _currentSelection = null;
        _text.text = NoSelection;
    }

    private Vector3 MoveOrder()
    {
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Ground" && _currentSelection != null)
                {
                    return hit.point;
                }
            }
        }

        return Vector3.zero;
    }

    private Vector3 AttackOrder()
    {
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (_currentSelection != null)
                {
                    return (hit.point - _currentSelection.transform.position).normalized;
                }
            }
        }

        return Vector3.zero;
    }

    public void SelectMoveOrder()
    {
        _currentOrder = AgentOrder.MOVE;
    }

    public void SelectHoldOrder()
    {
        _currentOrder = AgentOrder.HOLD;
    }

    public void SelectAttackOrder()
    {
        _currentOrder = AgentOrder.ATTACK;
    }
}
