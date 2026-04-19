using UnityEngine;

public class ValveInteractable : BaseInteractable
{
    [Header("Valve")]
    [SerializeField] private Transform valveHandle;
    [SerializeField] private ValveConfig config;

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
            currentAngle -= config.returnSpeed * Time.deltaTime;
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
        currentAngle += config.rotateSpeed * Time.deltaTime;
        currentAngle = Mathf.Min(currentAngle, config.maxRotationAngle);

        ApplyRotationAndDoorProgress();
    }

    public override void EndInteract()
    {
        isBeingHeld = false;
    }

    private void ApplyRotationAndDoorProgress()
    {
        valveHandle.localRotation = initialLocalRotation * Quaternion.Euler(0f, 0f, -currentAngle);

        float progress = currentAngle / config.maxRotationAngle;

        if (linkedDoor != null)
            linkedDoor.SetOpenProgress(progress);
    }
}