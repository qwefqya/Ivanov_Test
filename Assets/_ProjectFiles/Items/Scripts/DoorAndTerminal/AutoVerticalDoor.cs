using UnityEngine;

public class AutoVerticalDoor : MonoBehaviour
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private HackConfig hackConfig;

    private Vector3 closedLocalPosition;
    private Vector3 openedLocalPosition;
    private bool isOpening = false;

    private void Awake()
    {
        if (doorTransform == null)
            doorTransform = transform;

        closedLocalPosition = doorTransform.localPosition;
        openedLocalPosition = closedLocalPosition + Vector3.up * hackConfig.openHeight;
    }

    private void Update()
    {
        Vector3 target = isOpening ? openedLocalPosition : closedLocalPosition;
        doorTransform.localPosition = Vector3.MoveTowards(
            doorTransform.localPosition,
            target,
            hackConfig.moveSpeed * Time.deltaTime
        );
    }

    public void OpenDoor()
    {
        isOpening = true;
    }

    public void CloseDoor()
    {
        isOpening = false;
    }
}