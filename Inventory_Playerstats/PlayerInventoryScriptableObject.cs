using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/PlayerInventory", order = 1)]
public class PlayerInventoryScriptableObject : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] private float maxHealth;
    [SerializeField] private float CurHealth;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
