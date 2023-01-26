using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteractable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        SoundManager.Instance.PlaySound(SoundManager.Instance.grabSound);
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        GameManager.Instance.IncreaseDropThrowCount();
        SoundManager.Instance.PlaySound(SoundManager.Instance.dropSound);
    }

    public void Throw(int throwForce)
    {
        objectRigidbody.useGravity = true;
        objectRigidbody.AddForce(objectGrabPointTransform.transform.forward * throwForce, ForceMode.Impulse);
        this.objectGrabPointTransform = null;
        GameManager.Instance.IncreaseDropThrowCount();
        SoundManager.Instance.PlaySound(SoundManager.Instance.throwSound);
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);
        }
    }
}
