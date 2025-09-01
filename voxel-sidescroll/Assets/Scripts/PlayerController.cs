using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private Vector3 _playerRayOrigin;

    public LayerMask RaycastMask;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        // Block Interaction
        Plane playerPlane = new Plane(Vector3.back, transform.position);
        _playerRayOrigin = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit;

        float d;
        if(playerPlane.Raycast(mouseRay, out d))
        {
            Vector3 hitPoint = mouseRay.GetPoint(d);

            if (Input.GetMouseButtonDown(0))
            {
                hit = Physics2D.Raycast(_playerRayOrigin, (hitPoint - _playerRayOrigin).normalized, 100, RaycastMask);
                Debug.Log(hit.collider.name);
                if (hit.collider != null)
                {
                    TerrainUtils.SetBlock(hit, new BlockAir());
                }
            }

            if(Input.GetMouseButtonDown(1))
            {
                hit = Physics2D.Raycast(_playerRayOrigin, (hitPoint - _playerRayOrigin).normalized, 100, RaycastMask);
                if (hit.collider != null)
                {
                    TerrainUtils.SetBlock(hit, new Block(), true);
                }
            }
            Debug.DrawRay(_playerRayOrigin, (hitPoint - _playerRayOrigin).normalized, Color.red);
        }

        Debug.DrawRay(Camera.main.transform.position, mouseRay.direction, Color.yellow);
    }

    public List<WorldPos> OccupiedBlocks()
    {
        List<WorldPos> pos = new List<WorldPos>();
        pos.Add(TerrainUtils.GetBlockPos(_playerRayOrigin));
        pos.Add(new WorldPos(pos[0].x, pos[0].y - 1));
        return pos;
    }
}