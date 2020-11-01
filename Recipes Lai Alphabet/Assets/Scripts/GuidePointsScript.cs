using UnityEngine;
using UnityEngine.UI;
public class GuidePointsScript : MonoBehaviour
{

    WritingSequence writingSequence;
    PlayerBrush brush;

    public AudioClip dingSound;
    private AudioSource audioSource;
    private Image image;
    private PolygonCollider2D col;

    private void Awake()
    {
        writingSequence = GameObject.FindObjectOfType<WritingSequence>();
    }

    private void OnMouseOver()
    {
        brush = GameObject.FindObjectOfType<PlayerBrush>();
        if(this.gameObject == writingSequence.guidePointToClear && brush.BrushSelected)
        {
            col = this.GetComponent<PolygonCollider2D>();
            col.enabled = false;
            audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = dingSound;
            audioSource.volume = 0.5f;
            audioSource.Play();
            image = this.GetComponent<Image>();
            image.color = new Color(0, 0, 0, 0);

            writingSequence.MoveOnGuidePoints();
        }
    }
}
