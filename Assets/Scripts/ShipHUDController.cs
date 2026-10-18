using UnityEngine;
using UnityEngine.UIElements;
//Updates the UI toolKit HUD elements
[DisallowMultipleComponent]
[RequireComponent(typeof(UIDocument))]
public sealed class ShipHUDController : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private SailController _sailController;

    private Label _sailOpenLabel;
    private Label _sailAngleLabel;

    private void Awake()
    {
        if (_uiDocument == null)
        {
            _uiDocument = GetComponent<UIDocument>();
        }
    }

    private void OnEnable()

    {
        VisualElement root = _uiDocument.rootVisualElement;
        _sailOpenLabel = root.Q<Label>("sailOpenLabel");
        _sailAngleLabel = root.Q<Label>("sailAngleLabel");
    }



    private void Update()
    {
        if (_sailController == null)
        { 
            return;
        }

        //Keep HUD text updated with current simulation values
        if (_sailAngleLabel != null)
        {
            _sailAngleLabel.text = $"Sail Angle: {_sailController.Angle:0.0}°";
        }

        if (_sailOpenLabel != null)
        {
            _sailOpenLabel.text = $"Sail Open: {_sailController.OpenPrecentafe:0}%";
        }
    }

}
