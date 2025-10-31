using UnityEngine;
using System.Collections; 
using TMPro;

public class PowerRun : MonoBehaviour
{
[Header("Referências")]
    public PhysicsBasedLocomotion playerLocomotion;
    public GameObject powerIndicatorUI;
    public Footsteps playerFootsteps;

    [Header("Feedback para o Jogador")]
    public GameObject tutorialMessageObject;
    public float tutorialMessageDuration = 2.0f;

    [Header("Efeitos")]
    public AudioClip collectSoundClip;
    [Range(0f, 1f)]
    public float collectSoundVolume = 0.5f;

    private bool hasBeenCollected = false;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenCollected || !other.transform.root.CompareTag("Player"))
        {
            return;
        }
        hasBeenCollected = true;

        Debug.Log("Power-up de corrida apanhado!");

        if (playerLocomotion != null) playerLocomotion.canSprint = true;
        if (playerFootsteps != null) playerFootsteps.canSprint = true;
        if (powerIndicatorUI != null) powerIndicatorUI.SetActive(true);

        if (tutorialMessageObject != null)
        {
        
            playerLocomotion.StartCoroutine(ShowTutorialMessage());
        }

        
        if (collectSoundClip != null)
        {
            AudioSource.PlayClipAtPoint(collectSoundClip, transform.position, collectSoundVolume);
        }

        // Desativa o objeto do power-up
        gameObject.SetActive(false);
    }

    private IEnumerator ShowTutorialMessage()
    {
        tutorialMessageObject.SetActive(true);
        yield return new WaitForSeconds(tutorialMessageDuration);
        tutorialMessageObject.SetActive(false);
    }
}
     

