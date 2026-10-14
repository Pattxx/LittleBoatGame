using UnityEngine;
using UnityEngine.UIElements;

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
       
       _sailAngleLabel = $"Sail Open: {_sailOpenLabel.SailAngle: 0}%";
       _sailOpenLabel.text = $"Sail Open: {_sailOpenLabel.OpenPrecentage: 0.0}°";
    }

}
