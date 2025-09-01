using UnityEngine;
using System.Collections;

public class DiceRoller2D : MonoBehaviour
{
    [SerializeField] SpriteRenderer faceRenderer; // child renderer that shows the pips
    [SerializeField] Sprite[] faces;              
    [SerializeField] float rollSeconds = 0.6f;    // roll duration
    [SerializeField] float framesPerSecond = 24f; // swap speed

    public int LastRoll { get; private set; }
    public bool IsRolling { get; private set; }

    public void Roll()
    {
        if (IsRolling) return;
        StartCoroutine(RollRoutine());
    }

    IEnumerator RollRoutine()
    {
        IsRolling = true;

        float step = 1f / framesPerSecond;
        float t = 0f;

        while (t < rollSeconds)
        {
            faceRenderer.sprite = faces[Random.Range(0, 6)];
            transform.localScale = Vector3.one * (1f + 0.12f * Mathf.Sin(t * 28f));
            transform.Rotate(0f, 0f, -720f * step / rollSeconds);
            yield return new WaitForSeconds(step);
            t += step;
        }

        LastRoll = Random.Range(1, 7);
        faceRenderer.sprite = faces[LastRoll - 1];

        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
        IsRolling = false;
    }
}
