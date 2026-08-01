using UnityEngine;

public class PusherSong : Song
{
    [SerializeField] private ForceApplier forceApplier;
    [SerializeField] private float cooldownTime = 1f;
    [SerializeField] private float activeDuration = 0.2f;

    private float lastActivationTime = -999f;
    private float activationTimer = 0f;
    private bool isCurrentlyActive = false;

    private void Start()
    {
        if (forceApplier != null)
            forceApplier.Enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastActivationTime + cooldownTime)
        {
            lastActivationTime = Time.time;
            activationTimer = activeDuration;
            isCurrentlyActive = true;
        }

        if (isCurrentlyActive)
        {
            activationTimer -= Time.deltaTime;
            if (activationTimer <= 0f)
            {
                isCurrentlyActive = false;
            }
        }

        if (forceApplier != null)
        {
            forceApplier.Enabled = isCurrentlyActive;
        }
    }

    public override bool IsSinging()
    {
        return Input.GetMouseButtonDown(0);
    }

    public override bool IsEnabled()
    {
        return isCurrentlyActive;
    }
}