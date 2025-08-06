using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] private FocusTarget _focusTarget;
    [SerializeField] private Transform _camLookTarget;
    [SerializeField] private Transform _camMoveTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _focusTarget.moveTransform = _camMoveTarget;
        _focusTarget.focusTransform = _camLookTarget;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
