using UnityEngine;

/// <summary>
/// Score Collider that will trigger the Score function for the assigned points.
/// </summary>
public class ScoreCollider : MonoBehaviour
{
    [Header("Log Mode Activated")]
    [Space(5)]
    [SerializeField] private bool logOn = true;

    [Space(10)]

    [Header("Score Settings")]
    [Space(5)]
    [SerializeField] private int objectScore = 20;

    private ObjectInteractable objectInteractable;

    private void OnTriggerEnter(Collider other)
    {
        if (logOn)
            Debug.Log($"Tomato hit - {objectScore}");

        if (other.transform.TryGetComponent(out objectInteractable))
        {
            GameManager.Instance.Score(objectScore);
            SoundManager.Instance.PlaySound(SoundManager.Instance.scoreSound);
        }
    }
}
