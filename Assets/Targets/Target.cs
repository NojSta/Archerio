using UnityEngine;

public class Target : MonoBehaviour
{
    public delegate void TargetHit(Target target);
    public static event TargetHit OnTargetHit;

    [SerializeField]
    private Color hitColor = Color.red;

    private Renderer targetRenderer;
    private bool isHit = false; // Track whether this target has been hit already

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow") && !isHit)
        {
            ChangeColorToHitColor();
            OnTargetHit?.Invoke(this); // Notify the GameManager that this target was hit
        }
    }

    private void ChangeColorToHitColor()
    {
        targetRenderer.material.color = hitColor;
    }

    public bool IsHit()
    {
        return isHit;
    }

    public void SetHitStatus(bool status)
    {
        isHit = status;
    }
}
