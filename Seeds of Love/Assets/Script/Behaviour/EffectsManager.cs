using Script.Behaviour;
using UnityEngine;


[RequireComponent(typeof(NoteJudgement))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EffectsManager : MonoBehaviour
{
    // The Particle Prefab that will be created when the Note is hit
    public GameObject ParticleEffect;
    //The Sound Prefab that will be created when the Note is hit
    public GameObject SoundEffect;
    public GameObject HitEffect;
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
     * Creates the ParticleEffect and Note_Hit_SFX prefab on the note and stops the note from moving
     * Animates the Note for the Note hit
     */
    public void PlayEffect()
    {
        GameObject instance = (GameObject)Instantiate(ParticleEffect, transform.position, Quaternion.identity);
        GameObject instanceSound = (GameObject)Instantiate(SoundEffect, transform.position, Quaternion.identity);
        Destroy(instance, 1f);
        //vvv This part may need more post-merge adjustment
        Destroy(instanceSound, 1f);

        if (HitEffect)
        {
            GameObject instance2 = (GameObject)Instantiate(HitEffect, transform.position, Quaternion.identity);
            Destroy(instance2, 1f);
        }
        //^^^ This part may need more post-merge adjustment
        if (GetComponent<NoteMovement>())
        {
            GetComponent<NoteMovement>().moving = false;
        }
        //GetComponent<NoteMovement>().Moving = false;
        GetComponent<Animator>().Play("Note_Hit_Animation");
    }

}

