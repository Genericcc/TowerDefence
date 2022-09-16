using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Tower towerPrefab;

    [SerializeField] bool isPlacable;
    public bool IsPlacable { get { return isPlacable; } }

    GridManager gridManager;
    Pathfinder pathfinder;
    Bank bank;
    Vector2Int coordinates = new Vector2Int();

    void Awake() {
        gridManager = FindObjectOfType<GridManager>();    
        pathfinder = FindObjectOfType<Pathfinder>(); 
        bank = FindObjectOfType<Bank>();
    }
    
    void Start() {
        if(gridManager != null) {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if(!isPlacable) {
                gridManager.BlockNode(coordinates);
            }
        }
    }
    
    public void OnPointerDown (PointerEventData eventData) {
        if(gridManager.GetNode(coordinates).isWalkable && isPlacable && !pathfinder.WillTowerBlockPath(coordinates)) {
            bool isBuildSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);  
            if(isBuildSuccessful) {
                gridManager.BlockNode(coordinates);
                pathfinder.NotifyReceivers();
            }                                                                       
        }
    } 
}
