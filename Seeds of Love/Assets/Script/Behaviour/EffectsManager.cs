using Script.Behaviour;
using UnityEngine;


[RequireComponent(typeof(NoteJudgement))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EffectsManager : MonoBehaviour
{
    // The Particle Prefab that will be created when the Note is hit
    public GameObject ParticleEffect;

    private void Start()
    {
        // The OnHit event. Calls PlayEffect() when the note is Hit
        GetComponent<NoteJudgement>().OnHit += note => PlayEffect();
    }

    /*
     * Creates the ParticleEffect prefab on the note and stops the note from moving
     * Animates the Note for the Note hit
     */
    public void PlayEffect()
    {
        GameObject instance = (GameObject)Instantiate(ParticleEffect, transform.position, Quaternion.identity);
        Destroy(instance, 1f);
        if(GetComponent<NoteMovement>())
        {
            GetComponent<NoteMovement>().enabled = false;
        }
        //GetComponent<NoteMovement>().Moving = false;
        GetComponent<Animator>().Play("Note_Hit_Animation");
    }

}

