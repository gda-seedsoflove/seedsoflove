using Script.Behaviour;
using UnityEngine;

namespace Tests.Interactive.NoteTimingTest
{
    [RequireComponent(typeof(NoteJudgement))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChangeColorWhenHit : MonoBehaviour
    {
        public Color ColorAfterHit;

        private void Start()
        {
            GetComponent<NoteJudgement>().OnHit += note =>
                note.GetComponent<SpriteRenderer>().color = ColorAfterHit;
        }
    }
}
