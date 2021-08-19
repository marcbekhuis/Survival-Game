using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GamePaused) return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPosition = Vector2Int.FloorToInt(mousePosition - new Vector2(0.5f, 0.5f));
            List<PathNode> path = PathFinding.instance.FindPath(0,0, gridPosition.x, gridPosition.y);

            if (path == null) return;
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].x + 0.5f, path[i].y + 0.5f, 0), new Vector3(path[i + 1].x + 0.5f, path[i + 1].y + 0.5f, 0), Color.red, 60);
            }
        }
    }
}
