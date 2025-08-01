using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardVisual : MonoBehaviour
{
    private Vector3 lastPosition;
    private float distanceThreshold;
    private Vector3 visualPos;
    public Image image;
    private Card _card;

    private bool _isDragging = false;
    private bool _isInitialized = false;

    [Header("Movement Parameters")] [SerializeField]
    private float _followSpeed = 15.0f;

    private Vector3 _movementDelta;

    [Header("Rotation Parameters")] [SerializeField]
    private float _rotationSpeed = 50.0f;

    [SerializeField] private float _rotationAmount = 30f;
    private Vector3 _rotationDelta;

    public void Initialize(Card card)
    {
        _card = card;
        _isInitialized = true;
    }

    private void Update()
    {
        if (!_isInitialized) return;
        MoveToTarget(_card);
        if (!_isDragging) return;
    }

    public void OnBeginDrag(Card card)
    {
        _isDragging = true;
    }

    public void OnEndDrag(Card card)
    {
        _isDragging = false;
    }

    public void OnDrag(Card card)
    {

    }

    private void MoveToTarget(Card card)
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = card.transform.position;
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * _followSpeed);
        transform.position = newPosition;

        FollowRotation(card);
    }


    
    private void FollowRotation(Card card)
    {
        Vector3 movement = (transform.position - card.transform.position);
        float movementLerpSpeed = _isDragging ? 10f : 20f;
        _movementDelta = Vector3.Lerp(_movementDelta, movement, movementLerpSpeed * Time.deltaTime);
        float effectingRotationAmount = _isDragging ? _rotationAmount * 0.025f : _rotationAmount * 0.05f;
        Vector3 movementRotation = (_isDragging ? _movementDelta : movement) * effectingRotationAmount;
        _rotationDelta = Vector3.Lerp(_rotationDelta, movementRotation, _rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
            Mathf.Clamp(_rotationDelta.x, -60, 60));
    }
}
