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

    bool cleared = false;

    private void Awake()
    {
        writingSequence = GameObject.FindObjectOfType<WritingSequence>();
    }

    private void Update()
    {
	#if UNITY_EDITOR
        if (Input.GetMouseButton(0) && !cleared)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 50f;
            Vector3 pointPos = this.transform.position;
            pointPos.z = 50f;

            float distance = Vector2.Distance(pointPos, mousePos);

            if (distance <= 0.85f)
            {
                ClearPoint();
            }
        }
	#endif

	#if (PLATFORM_IOS || PLATFORM_ANDROID)
                if (Input.touchCount > 0 && !cleared)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                    mousePos.z = 50f;
                    Vector3 pointPos = this.transform.position;
                    pointPos.z = 50f;

                    float distance = Vector2.Distance(pointPos, mousePos);

                    if (distance <= 0.85f)
                    {
                        ClearPoint();
                    }
                }
        #endif
    }

    private void OnMouseOver()
    {
        ClearPoint();
    }

    private void ClearPoint()
    {
        cleared = true;
        brush = GameObject.FindObjectOfType<PlayerBrush>();
        if (this.gameObject == writingSequence.guidePointToClear && brush.BrushSelected)
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
