using UnityEngine;

public class AutoVerticalDoor : MonoBehaviour
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private float openHeight = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 closedLocalPosition;
    private Vector3 openedLocalPosition;
    private bool isOpening = false;

    private void Awake()
    {
        if (doorTransform == null)
            doorTransform = transform;

        closedLocalPosition = doorTransform.localPosition;
        openedLocalPosition = closedLocalPosition + Vector3.up * openHeight;
    }

    private void Update()
    {
        Vector3 target = isOpening ? openedLocalPosition : closedLocalPosition;
        doorTransform.localPosition = Vector3.MoveTowards(
            doorTransform.localPosition,
            target,
            moveSpeed * Time.deltaTime
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