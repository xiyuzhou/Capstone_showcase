using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    //private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    public int InteractableLayerIndex = 3;
    private string CurString;
    private string PastString;
    [SerializeField]
    private AshesToInk.PlayerActions playerInput;
    public TextMeshProUGUI interactText;
    //public PlayerController controller;  
    public PlayerStatManager playerStatManager;


    [SerializeField]
    private List<GameObject> itemList = new List<GameObject>();

    void Start()
    {
        //cam = controller.cam;
        CurString = string.Empty;
        PastString = string.Empty;
        UpdateInteractText(CurString);
        //playerInput.Interact.performed += ctx => Interact();
    }
    void Update()
    {
        PastString = CurString;
        CurString = string.Empty;
        //Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction * distance);
        //RaycastHit hitInfo;
        /*if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (interactable.InteractionType == InteractionMethod.Raycast)
                {
                    CurString = interactable.promptMessage;
                    if (BasicCtrl.Interact.triggered)
                    {
                        interactable.BaseInteract();
                    }
                }
                    
            }
        }*/
        if (itemList.Count > 0)
        {
            GameObject lastObject = itemList[itemList.Count - 1];
            Interactable interactable = lastObject.GetComponent<Interactable>();
            CurString = interactable.promptMessage;
            /*if (BasicCtrl.Interact.triggered)
            {
                PerformInteraction(interactable);
                //itemList.Remove(lastObject);
            }*/
        }

        
        if (PastString != CurString)
            UpdateInteractText(CurString);
    }
    private void OnControllerColliderHit(ControllerColliderHit collision)//collision interaction
    {
        //Debug.Log(collision.collider.name);
        if (collision.gameObject.layer == InteractableLayerIndex)
        {
            if (collision.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = collision.collider.GetComponent<Interactable>();
                if (interactable.InteractionType == InteractionMethod.Collision)
                {
                    PerformInteraction(interactable);
                }
            }
        }
    }
    private void PerformInteraction(Interactable interactable)
    {
        if (interactable.addBuff)
        {
            var buffInfo = interactable.getBuff();
            if (buffInfo != null)
            {
                //add buff to buff manager
                playerStatManager.AddBuff(buffInfo);
            }
        }
        interactable.BaseInteract();
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer);
        if (other.gameObject.layer == InteractableLayerIndex)
        {
            //Debug.Log(other.gameObject.name);
            if (other.GetComponent<Interactable>() != null)
            {
                itemList.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interactable>() != null)
        {
            itemList.Remove(other.gameObject);
        }
    }

    public void Interact()
    {
        //Debug.Log("interact");
        if (itemList.Count > 0)
        {
            //if(playerInput.Interact.triggered)
            
            GameObject lastObject = itemList[itemList.Count - 1];
            PerformInteraction(lastObject.GetComponent<Interactable>());
            itemList.Remove(lastObject);
        }
    }
    void UpdateInteractText(string text)
    {
        interactText.text = text;
    }
}
