using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private BoxCollider2D _col;
    private Rigidbody2D _rb;
    public GameObject EnemyPrefab;

    public int EnemyRows;
    public int EnemiesPerRow;

    public float MoveSpeed;
    public float MoveDownTimer;

    private float timer;
    private bool movingDown;
    private Vector2[] directions = new Vector2[] { new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, -1) };
    private int directionPos = 0;

    void Start ()
    {
        _col = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();

        Vector2 origin = transform.position;
        Vector2 range = new Vector2(_col.bounds.extents.x, _col.bounds.extents.y);

        float xIncrement = (range.x * 2) / EnemiesPerRow;
        float yIncrement = (range.y * 2) / EnemyRows;

        Vector2 spawnStart = new Vector2(-range.x + 1, range.y - 1);

        Debug.Log(origin + " - " + range + " - " + spawnStart);

        for (int i = 0; i < EnemyRows; i++)
        {
            for(int j = 0; j < EnemiesPerRow; j++)
            {
                GameObject enemy = Instantiate(EnemyPrefab, transform, false);
                Vector2 pos = spawnStart;
                pos.x = pos.x + j * xIncrement;
                pos.y = pos.y - i * yIncrement;
                enemy.transform.localPosition = pos;
            }
        }
	}

	void Update ()
    {
		if(movingDown)
        {
            if(timer < 0)
            {
                UpdateDirection();
                movingDown = false;
            }
            timer -= Time.deltaTime;
        }

        _rb.AddForce(directions[directionPos] * MoveSpeed);
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if(!movingDown && col.gameObject.tag == "ScreenEdge")
        {
            UpdateDirection();
            movingDown = true;
            timer = MoveDownTimer;
        }
    }

    private void UpdateDirection()
    {
        directionPos++;
        if (directionPos > 3)
        {
            directionPos = 0;
        }
        _rb.velocity = Vector3.zero;
    }
}
