using UnityEngine;

public class PowerRun : MonoBehaviour
{
    [Header("Referências")]
    public PhysicsBasedLocomotion playerLocomotion;
    public GameObject powerIndicatorUI;
    public Footsteps playerFootsteps;

    [Header("Efeitos")]
    public AudioClip collectSoundClip;
    [Range(0f, 1f)]
    public float collectSoundVolume = 0.7f; // Volume padrão de 70%

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("Power-up de corrida apanhado!");

            if (playerLocomotion != null) playerLocomotion.canSprint = true;
            if (playerFootsteps != null) playerFootsteps.canSprint = true;
            if (powerIndicatorUI != null) powerIndicatorUI.SetActive(true);

            // Pede ao PowerSound para tocar o som
            if (collectSoundClip != null && PowerSound.instance != null)
            {
                PowerSound.instance.PlaySound(collectSoundClip, collectSoundVolume);
            }

            Destroy(gameObject);
        }
    }
}
