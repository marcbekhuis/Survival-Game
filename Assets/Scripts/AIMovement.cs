using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : Movement
{
    public Transform target;

    [SerializeField] private bool debugging = false;

    private Vector2Int lastRequestPosition = Vector2Int.down;
    private List<Vector2> path;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GamePaused)
        {
            Move(Vector2.zero, false);
            return;
        }

        Vector2Int targetGridPosition = new Vector2Int(Mathf.FloorToInt(target.position.x), Mathf.RoundToInt(target.position.y));
        Vector2Int aiGridPosition = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        if (lastRequestPosition != targetGridPosition)
        {
            path = PathFinding.instance.FindPath(aiGridPosition, targetGridPosition);
            lastRequestPosition = targetGridPosition;
            if (path != null && debugging)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x + 0.5f, path[i].y + 0.5f, 0), new Vector3(path[i + 1].x + 0.5f, path[i + 1].y + 0.5f, 0), Color.red, 60);
                }
            }
        }

        if (path != null && path.Count > 0)
        {
            Vector2 pointOne = path[0] + new Vector2(0.5f, 0f);
            Vector2 pointTwo = Vector2.zero;

            float distanceOne = Vector2.Distance((Vector2)transform.position, pointOne);
            float distanceTwo = 0;

            if (path.Count > 1)
            {
                pointTwo = path[1] + new Vector2(0.5f, 0f);
                distanceTwo = Vector2.Distance((Vector2)transform.position, pointTwo);
            }

            Vector2 direction = (pointOne - (Vector2)transform.position) * distanceOne + (pointTwo - (Vector2)transform.position) * distanceTwo;
            Move(direction.normalized, false);
            if (distanceOne < 0.1f || distanceTwo <= distanceOne) path.RemoveAt(0);
            return;
        }
        Move(Vector2.zero, false);
    }
}
