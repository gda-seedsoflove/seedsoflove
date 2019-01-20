using Script.Behaviour;
using UnityEngine;


[RequireComponent(typeof(NoteJudgement))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EffectsManager : MonoBehaviour
{
    // The Particle Prefab that will be created when the Note is hit
    public GameObject ParticleEffect;
    public GameObject HeldEffect;

    private void Start()
    {
        // The OnHit event. Calls PlayEffect() when the note is Hit
        GetComponent<NoteJudgement>().OnHit += note => PlayEffect();
    }

    void Update()
    {
        if (GetComponent<NoteJudgement>().Note.Holding && Random.Range(0,4) == 1)
        {
            GameObject instance = (GameObject)Instantiate(HeldEffect, transform.position, Quaternion.identity);
            Destroy(instance, 1f);
        }
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
            GetComponent<NoteMovement>().moving = false;
        }
        //GetComponent<NoteMovement>().Moving = false;
        GetComponent<Animator>().Play("Note_Hit_Animation");
    }

}

