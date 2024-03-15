using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MainMenuUiManager : MonoBehaviour
{
    public Transform triggerPoint;
    private int movingState = 0;

    private GameObject currentSelection;
    private GameObject movingObject;

    public float moveToObjectSpeed = 2f;  // Speed to move towards the trigger point
    public float angularSpeed = 1;

    public int CamState = 0;
    public Camera cam;
    public Animator CamAnimator;
    public bool onCamAnimation = false;

    public Transform EndPoint;
    public UIPageManager pageManager;
    public SceneLoader sceneManager;

    private bool readyToInteract = false;
    public Transform defaultCamPos;
    public Transform camParentPos;
    public Transform camRotation;
    public Transform camStartScenePos;

    public float CamFollowCursorStrength = 1f;

    public int MainMenuState = 0;

    public bool EnableCamFollowCursor = true;

    public GameObject ContinueText;
    public float TitlefadeSpeed;

    void Update()
    {
        HandleMouseSelection();
        HandleMovingState();
        HandleCameraState();
        HandleMainMenuState();
        HandleInteraction();
    }
    public void OnBackMenu()
    {
        if (movingObject != null)
        {
            movingState = 2;
            CamState = 2;
            MainMenuState = 4;
            pageManager.ReturnToMenu();
            Animator bookAnimation = movingObject.GetComponentInChildren<Animator>();
            bookAnimation.SetTrigger("CloseBookTrigger");
        }
    }
    void HandleMovingState()
    {
        switch (movingState)
        {
            case 1:
                MoveObjectToFinalPoint(movingObject);
                break;
            case 2:
                MoveObjectToEndPoint(movingObject);
                break;
            case 3:
                // the book opened
                break;
        }
    }
    void HandleMouseSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("MenuBook"))
        {
            currentSelection = hit.collider.gameObject;
            currentSelection.GetComponent<BookShaderManager>().HandleSelect();

        }
        else
        {
            currentSelection = null;
        }
    }
    void HandleCameraState()
    {
        switch (CamState)
        {
            case 0:
                if (Vector3.Distance(camParentPos.position, defaultCamPos.position) <= 5f)
                {
                    readyToInteract = true;
                }
                break;
            case 1:
                if (!onCamAnimation)
                {
                    CamState = 3;
                    break;
                }
                float targetFov = movingObject.GetComponent<MenuBookInfo>().camFovAdjust;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, 5 * Time.deltaTime);
                CamAnimator.SetInteger("CamStateChange", 1);
                break;
            case 2:
                CamAnimator.SetInteger("CamStateChange", 2);
                cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, 35, 20 * Time.deltaTime);
                break;
            case 3:
                targetFov = movingObject.GetComponent<MenuBookInfo>().camFovAdjust;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, 5 * Time.deltaTime);
                break;
            case 4:
                CamAnimator.SetInteger("CamStateChange", 2);
                cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, 35, 20 * Time.deltaTime);
                break;


        }
    }
    void HandleMainMenuState()
    {
        bool CamRotateMode = true;
        bool camRotate = false;

        switch (MainMenuState)
        {
            case 0:
                if (Vector3.Distance(camParentPos.position, camStartScenePos.position) <= 2f)
                {
                    ContinueText.SetActive(true);
                    MainMenuState = 1;
                }
                camParentPos.position = LerpCamPos(camParentPos.position, camStartScenePos.position, 0.5f);
                break;
            case 1:
                camRotate = false;
                CamRotateMode = true;
                if (Input.anyKey)
                {
                    MainMenuState = 2;
                    CamState = 0;
                    ContinueText.SetActive(false);
                    break;
                }
                camParentPos.position = LerpCamPos(camParentPos.position, camStartScenePos.position, 0.5f);
                break;
            case 2:
                camRotate = true;
                CamRotateMode = true;
                camParentPos.position = LerpCamPos(camParentPos.position, defaultCamPos.position, 1f);
                break;
            case 3:
                CamRotateMode = false;
                camRotate = true;
                camParentPos.position = LerpCamPos(camParentPos.position, Vector3.zero, 5f);
                break;
            case 4:
                CamRotateMode = true;
                camRotate = true;
                camParentPos.position = LerpCamPos(camParentPos.position, defaultCamPos.position, 0.5f);
                break;
            case 5:
                CamRotateMode = false;
                camRotate = true;
                readyToInteract = false;
                camParentPos.position = LerpCamPos(camParentPos.position, camStartScenePos.position, 1f);
                if (Vector3.Distance(camParentPos.position, camStartScenePos.position) <= 0.5f)
                {
                    MainMenuState = 0;
                }
                break;
        }
        if (camRotate)
            RotateCameraWithMouse(CamRotateMode);
    }

    Vector3 LerpCamPos(Vector3 a, Vector3 target, float speed)
    {
        Vector3 k = Vector3.Lerp(a, target, speed * Time.deltaTime);
        return k;
    }
    public void HandleBackToTitleScreen()
    {
        if (MainMenuState != 0 || MainMenuState != 1)
        {
            OnBackMenu();
            MainMenuState = 5;
            CamState = 4;
        }
    }
    void HandleInteraction()
    {
        if (Input.GetMouseButtonDown(0) && movingState == 0 && currentSelection != null && readyToInteract == true)
        {
            movingObject = currentSelection;
            movingState = 1;
            CamState = 1;
            MainMenuState = 3;
            onCamAnimation = true;
            Vector3 buffer = currentSelection.transform.position;
            Animator bookAnimation = currentSelection.GetComponentInChildren<Animator>();

            if (currentSelection.GetComponent<MenuBookInfo>().menuSelection == MenuSelection.Play)
            {
                StartCoroutine(DelayAnimation(bookAnimation, 0.3f));
                //bookAnimation.SetTrigger("OpenBookTrigger");
                StartCoroutine(DelayedShowScreens(MenuSelection.Play, 1.2f));
                currentSelection.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                currentSelection.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                buffer.y = 3.5f;
                StartCoroutine(DelayAnimation(bookAnimation, 0.2f));
            }

            EndPoint.transform.position = buffer;
            EndPoint.transform.rotation = currentSelection.transform.rotation;
        }
    }
    void MoveObjectToFinalPoint(GameObject BookObject)//book and camera moving to setted position, then fade in the UI
    {
        Vector3 targetPosition = new Vector3(triggerPoint.position.x, BookObject.transform.position.y, triggerPoint.position.z);
        float step = moveToObjectSpeed * Time.deltaTime;
        BookObject.transform.position = Vector3.MoveTowards(BookObject.transform.position, targetPosition, step);

        Quaternion targetRotation = Quaternion.Euler(0f, triggerPoint.rotation.eulerAngles.y, 0f);
        BookObject.transform.rotation = Quaternion.Lerp(BookObject.transform.rotation, targetRotation, angularSpeed*Time.deltaTime);

        if (Vector2.Distance(new Vector2(BookObject.transform.position.x, BookObject.transform.position.z), new Vector2(targetPosition.x, targetPosition.z)) < 0.01f)
        {
            //move to exact position
            movingState = 3;
            Debug.Log("Finish");

            // Trigger open book animation here
            var selection = movingObject.GetComponent<MenuBookInfo>().menuSelection;
            Debug.Log("Selection= "+ selection);
            if (selection != MenuSelection.Play)
            {
                StartCoroutine(DelayedShowScreens(selection, 0.25f));
            }
            else
                currentSelection.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    IEnumerator DelayAnimation(Animator animator, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        animator.SetTrigger("OpenBookTrigger");

    }
    IEnumerator DelayedShowScreens(MenuSelection selection, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        pageManager.ShowScreens(selection);
    }
    void MoveObjectToEndPoint(GameObject BookObject)
    {
        //Trigger close book animation here
        BookObject.transform.position = Vector3.MoveTowards(BookObject.transform.position, EndPoint.position, 10 * Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(EndPoint.rotation.eulerAngles.x, EndPoint.rotation.eulerAngles.y, EndPoint.rotation.eulerAngles.z);
        BookObject.transform.rotation = Quaternion.Lerp(BookObject.transform.rotation, targetRotation, Time.deltaTime * 16);

        if (Vector3.Distance(BookObject.transform.position,EndPoint.position) <= 0.01)
        {
            readyToInteract = false;
            StartCoroutine(DelaySetMovingStateZero());
            movingState = 0;

            if (movingObject.GetComponent<MenuBookInfo>().menuSelection != MenuSelection.Play)
            {
                var rb = BookObject.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                rb.isKinematic = false;
            }
            BookObject.transform.rotation = EndPoint.rotation; //add random rotation here
            movingObject = null;

        }
    }
    IEnumerator DelaySetMovingStateZero()
    {
        yield return new WaitForSeconds(0.5f);
        readyToInteract = true;
    }
    void OnBookMenu(GameObject BookObject)
    {
        MenuBookInfo currentMenu = BookObject.GetComponent<MenuBookInfo>();
        var menuInfo = currentMenu.menuSelection;
    }
    void RotateCameraWithMouse(bool mode)
    {
        if (!mode)
        {
            camRotation.transform.rotation = Quaternion.Lerp(camRotation.transform.rotation, Quaternion.identity, Time.deltaTime * 5);
            return;
        }
        float mouseX = Input.mousePosition.x / Screen.width;
        float mouseY = Input.mousePosition.y / Screen.height;

        // Adjust the rotation angles based on mouse movement
        float rotationX = (-mouseY + 0.5f) * CamFollowCursorStrength;
        float rotationY = (mouseX - 0.5f) * CamFollowCursorStrength;

        rotationX = Mathf.Clamp(rotationX,-0.5f * CamFollowCursorStrength, 0.5f* CamFollowCursorStrength);
        rotationY = Mathf.Clamp(rotationY, -0.5f * CamFollowCursorStrength, 0.5f * CamFollowCursorStrength);
        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0f);

        camRotation.transform.rotation = Quaternion.Lerp(camRotation.transform.rotation, targetRotation, Time.deltaTime * 1);
    }
}
public enum MenuSelection
{
    HowToPlay,
    Settings,
    TheMakingOf,
    Play,
    NewOrLoad,
    ExitToDesktop,
}