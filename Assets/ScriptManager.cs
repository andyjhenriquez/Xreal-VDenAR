using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScriptManager : MonoBehaviour
{
    public Camera cam;
    public Canvas canvas;
    public TextMeshProUGUI text;

    public GameObject gun;
    private Component[] meshes;

    public float moveDuration = 8f;
    private bool movingGun = false;
    private float initialDistance;
    private Coroutine moveCoroutine;
    private bool rotatingGun = false;
    private Coroutine rotateCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        canvas.transform.LookAt(canvas.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }

    public void LookingAtComponent(int compID)
    {
        canvas.gameObject.SetActive(true);

        if (compID == 0)
            text.text = "Main Body";
        else if (compID == 1)
            text.text = "Front Sight";
        else if (compID == 2)
            text.text = "Magazine";
        else if (compID == 3)
            text.text = "Charging Handle";
        else if (compID == 4)
            text.text = "Rear Sight";
        else
            text.text = "Stock";
    }

    public void NotLookingAtComponent()
    {
        canvas.gameObject.SetActive(false);
    }

    public void ToggleHideGun()
    {
        if (gun.GetComponentInChildren<MeshRenderer>().enabled)
        {
            meshes = gun.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer mesh in meshes)
                mesh.enabled = false;
        }
        else
        {
            meshes = gun.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer mesh in meshes)
                mesh.enabled = true;
        }
    }

    public void ToggleMoveGun()
    {
        if (!movingGun)
        {
            Transform head = cam.transform;
            initialDistance = Vector3.Distance(head.position, gun.transform.position);

            movingGun = true;
            moveCoroutine = StartCoroutine(MoveGunToGaze());
        }
        else
        {
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            movingGun = false;
        }
    }

    private IEnumerator MoveGunToGaze()
    {
        float timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;

            Transform head = cam.transform;
            Vector3 gazeOrigin = head.position;
            Vector3 gazeDirection = head.forward;

            gun.transform.position = gazeOrigin + gazeDirection * initialDistance;

            yield return null;
        }

        movingGun = false;
    }

    public void ToggleRotateGun()
    {
        if (!rotatingGun)
        {
            rotatingGun = true;
            rotateCoroutine = StartCoroutine(RotateGunToGaze());
        }
        else
        {
            if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
            rotatingGun = false;
        }
    }

    private IEnumerator RotateGunToGaze()
    {
        float timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;

            gun.transform.LookAt(gun.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);

            yield return null;
        }

        rotatingGun = false;
    }
}
