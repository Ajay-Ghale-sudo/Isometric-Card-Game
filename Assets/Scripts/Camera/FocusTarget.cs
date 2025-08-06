using UnityEngine;

[CreateAssetMenu(fileName = "FocusTarget", menuName = "Camera/FocusTarget")]
public class FocusTarget : ScriptableObject
{
    [SerializeField] public Transform moveTransform;
    [SerializeField] public Transform focusTransform;
    [SerializeField] public GameObject target;
}
