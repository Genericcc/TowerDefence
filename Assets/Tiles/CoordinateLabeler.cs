using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [Header("Pathfinding colors")]
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.grey;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f, 0.5f, 0f);

    Vector2Int coordinates = new Vector2Int();
    TextMeshPro label;
    GridManager gridManager;

    void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        DisplayCoordinates();
        
    }

    void Update() {
        if(!Application.isPlaying) {
            DisplayCoordinates();   
            UpdateObjectName();
        }
        SetLabelColor();
        ToggleLabels();
    }

    void DisplayCoordinates() {
        if(gridManager == null) { return; }
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);
        label.text = $"{coordinates.x},{coordinates.y}";
    }

    void UpdateObjectName() {
        transform.parent.name = coordinates.ToString();
    }

    void SetLabelColor() {
        if(gridManager == null) { return; }

        Node node = gridManager.GetNode(coordinates);

        if(node == null) { return; } 

        if(!node.isWalkable) {
            label.color = blockedColor;
        }
        else if(node.isPath) {
            label.color = pathColor;
        }
        else if(node.isExplored) {
            label.color = exploredColor;
        }
        else {
            label.color = defaultColor;
        }
    }

    void ToggleLabels() {
        if(Keyboard.current.cKey.wasPressedThisFrame) {
            label.enabled = !label.enabled;
        }
    }
}
