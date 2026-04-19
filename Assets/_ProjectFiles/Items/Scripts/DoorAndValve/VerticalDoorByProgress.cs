using UnityEngine;

public class VerticalDoorByProgress : MonoBehaviour
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private float openHeight = 3f;

    private Vector3 closedLocalPosition;

    private void Awake()
    {
        if (doorTransform == null)
            doorTransform = transform;

        closedLocalPosition = doorTransform.localPosition;
    }

    public void SetOpenProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);

        Vector3 targetPosition = closedLocalPosition + Vector3.up * openHeight * progress;
        doorTransform.localPosition = targetPosition;
    }
}