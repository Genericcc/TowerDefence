using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoords;
    public Vector2Int StartCoords { get { return startCoords; } }

    [SerializeField] Vector2Int destinationCoords;
    public Vector2Int DestinationCoords { get { return destinationCoords; } }
    Node startNode;
    Node destinationNode;
    Node currentSearchNode;
    Vector2Int[] directions = { Vector2Int.right,
                                Vector2Int.left,
                                Vector2Int.up,
                                Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();
    Queue<Node> frontier = new Queue<Node>();
    
    void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null) {
            grid = gridManager.Grid;
            startNode = grid[startCoords];
            destinationNode = grid[destinationCoords]; 
        } 
    }

    void Start() {
        GetNewPath();
    }

    public List<Node> GetNewPath() {
        return GetNewPath(startCoords);
    }

    public List<Node> GetNewPath(Vector2Int coordinates) {
        gridManager.ResetNodes();
        BreadthFirstSerach(coordinates);
        return BuildPath();
    }

    void BreadthFirstSerach(Vector2Int coordinates) {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        gridManager.ResetNodes();
        frontier.Clear();
        reached.Clear();
        
        bool foundDestination = false;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while(frontier.Count > 0 && !foundDestination) {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if(currentSearchNode.coordinates == destinationCoords) {
                foundDestination = true;
            }
        }
    }

    void ExploreNeighbors() {
        List<Node> neighbors = new List<Node>();

        //Checks if there are neighbors, and if there are, adds them to the neighbors list
        foreach(Vector2Int direction in directions) {                                        
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction; 
            if(grid.ContainsKey(neighborCoords) && grid[neighborCoords].isWalkable) {
                neighbors.Add(grid[neighborCoords]);
            }
        }

        //Enqueues the new neighbors from the list into frontier queue if they're eligible for it and adds them into reached
        foreach(Node neighbor in neighbors) {
            if(!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable) {
                neighbor.connectedTo = currentSearchNode;       //creates connections from the next nodes back to the current one 
                reached.Add(neighbor.coordinates, neighbor);    //marks the node as reached 
                frontier.Enqueue(neighbor);                     //adds into the queue which will check if it's the end
            }
        }       
    }    

    List<Node> BuildPath() {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while(currentNode.connectedTo != null) {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }
        path.Reverse();
        return path;
    }   

    public bool WillTowerBlockPath(Vector2Int coordinates) {        //checks if placing the tower will block the paths
        if(grid.ContainsKey(coordinates)) {
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if(newPath.Count <= 1) {
                GetNewPath();
                return true;
            }
        }
        return false;
    }

    public void NotifyReceivers() {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
