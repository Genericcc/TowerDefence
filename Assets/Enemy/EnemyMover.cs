using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0f,5f)] float speed = 1f;

    List<Node> path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    Pathfinder pathfinder;

    void Awake() {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void OnEnable() {
        ReturnToStart();
        RecalculatePath(true);
    }

    void RecalculatePath(bool resetPathToStart) {
        Vector2Int coordinates = new Vector2Int();

        if(resetPathToStart) {
            coordinates = pathfinder.StartCoords;
        }
        else {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();

        path.Clear();
        path = pathfinder.GetNewPath(coordinates);

        StartCoroutine(FollowPath());
    }

    void ReturnToStart() {
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoords);
    }

    IEnumerator FollowPath() {
        for(int i = 1; i < path.Count; i++) {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while(travelPercent < 1f) {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        FinishPath();
    }

    void FinishPath() {
        enemy.StealGold();
        gameObject.SetActive(false);
    }
}
