using UnityEngine;

[CreateAssetMenu(fileName = "FocusTarget", menuName = "Camera/FocusTarget")]
public class FocusTarget : ScriptableObject
{
    public Transform transform;
    public GameObject target;
}
