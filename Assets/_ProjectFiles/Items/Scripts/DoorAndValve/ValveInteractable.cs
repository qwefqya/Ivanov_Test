using UnityEngine;

public class ValveInteractable : BaseInteractable
{
    [Header("Valve")]
    [SerializeField] private Transform valveHandle;
    [SerializeField] private float maxRotationAngle = 180f;
    [SerializeField] private float rotateSpeed = 120f;
    [SerializeField] private float returnSpeed = 90f;

    [Header("Door")]
    [SerializeField] private VerticalDoorByProgress linkedDoor;

    private float currentAngle = 0f;
    private bool isBeingHeld = false;
    private Quaternion initialLocalRotation;

    private void Awake()
    {
        promptText = "крутить";
        interactionType = InteractionType.Hold;

        if (valveHandle == null)
            valveHandle = transform;

        initialLocalRotation = valveHandle.localRotation;
    }

    private void Update()
    {
        if (!isBeingHeld && currentAngle > 0f)
        {
            currentAngle -= returnSpeed * Time.deltaTime;
            currentAngle = Mathf.Max(currentAngle, 0f);

            ApplyRotationAndDoorProgress();
        }
    }

    public override void BeginInteract()
    {
        isBeingHeld = true;
    }

    public override void UpdateInteract(float holdTime)
    {
        currentAngle += rotateSpeed * Time.deltaTime;
        currentAngle = Mathf.Min(currentAngle, maxRotationAngle);

        ApplyRotationAndDoorProgress();
    }

    public override void EndInteract()
    {
        isBeingHeld = false;
    }

    private void ApplyRotationAndDoorProgress()
    {
        valveHandle.localRotation = initialLocalRotation * Quaternion.Euler(0f, 0f, -currentAngle);

        float progress = currentAngle / maxRotationAngle;

        if (linkedDoor != null)
            linkedDoor.SetOpenProgress(progress);
    }
}