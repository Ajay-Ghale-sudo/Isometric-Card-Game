using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    private Vector3 lastPosition;
    private float swapDistanceThreshold;
    private static float moveDistanceThreshold = 0.0015f;
    private Vector3 visualPos;
    private Card _card;

    [Header("References")] 
    [SerializeField] private Transform _shakeParent;
    [SerializeField] private Transform _tiltParent;

    private bool _isDragging = false;
    private bool _isInitialized = false;

    [Header("Movement Parameters")] 
    [SerializeField] private float _followSpeed = 15.0f;
    private Vector3 _movementDelta;

    [Header("Rotation Parameters")] 
    [SerializeField] private float _rotationSpeed = 50.0f;
    [SerializeField] private float _rotationAmount = 30f;
    private Vector3 _rotationDelta;

    [Header("Card Data Elements")]
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardCost;
    public TextMeshProUGUI cardPowerRange;
    public Image cardImage;

    [Header("Scale Params")] 
    [SerializeField] private float _scaleOnHover = 1.15f;
    [SerializeField] private float _scaleOnSelect = 1.25f;
    [SerializeField] private float _scaleTransitionTime = 0.15f;
    [SerializeField] private Ease _scaleEase = Ease.OutBack;

    [Header("Hover Params.")] 
    [SerializeField] private float _hoverPunchAngle = 5.0f;
    [SerializeField] private float _hoverTransition = 0.15f;
    
    private bool _isSelected = false;
    private bool _scaleAnimations = true;

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
        if (_scaleAnimations)
        {
            transform.DOScale(_scaleOnSelect, _scaleTransitionTime);
        }
        
        _isDragging = true;
    }

    public void OnEndDrag(Card card)
    {
        _isDragging = false;
    }

    public void OnDrag(Card card)
    {

    }

    public void OnHover(Card card)
    {
        
        if (_scaleAnimations && !_isDragging)
        {
            print("Hovered");
            transform.DOScale(_scaleOnHover, _scaleTransitionTime).SetEase(_scaleEase);
            transform.DOPunchRotation(Vector3.forward * _hoverPunchAngle, _hoverTransition).SetEase(_scaleEase);
        }
    }

    public void OnEndHover(Card card)
    {
        if (_scaleAnimations)
        {
            transform.DOScale(1.0f, _scaleTransitionTime).SetEase(_scaleEase);
        }

        DOTween.Kill(2, true);
        
    }

    public void Select()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.localPosition = transform.localPosition + (Vector3.up * 50.0f);
        }
    }

    #region CardMovement
    private void MoveToTarget(Card card)
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = card.transform.position;
        if (Vector3.Distance(currentPosition, targetPosition) > moveDistanceThreshold)
        {
            Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * _followSpeed);
            transform.position = newPosition;
        }
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

    #endregion

}
