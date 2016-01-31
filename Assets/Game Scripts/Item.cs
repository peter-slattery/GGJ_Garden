using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    public enum InteractionState { NoInteraction, PendingPickup , ReadyForPickup }
    //public enum ItemType { Tool , Fruit , Seed }
    
    public string PrefabName;
    public string Description;
    public Sprite ItemIcon;
    public AudioClip PickupSound;
    public int PickupProximity = 3;
    public InteractionState m_state = InteractionState.NoInteraction;
    //public ItemType m_itemType;
    bool isCoroutineRunning = false;

    void Update()
    {
        if (!isCoroutineRunning && m_state == InteractionState.PendingPickup)
        {
            StartCoroutine("ProximityCheck");
        }

        if (m_state == InteractionState.ReadyForPickup)
        {
            if (Inventory._instance.AddItem(this.gameObject))
            {
                Destroy(this.gameObject);
            }
            else
            {
                // Too many items in bag 
                Debug.Log("Too many items in bag!");
                m_state = InteractionState.NoInteraction;

                // TODO: play sound?
            }
        }
    }

    IEnumerator ProximityCheck()
    {
        for (;;)
        {
            isCoroutineRunning = true;
            PlayerProximityPickup();
            yield return new WaitForSeconds(.5f);
            isCoroutineRunning = false;
        }
    }

    // wait until the player is close enough and then pick up
    void PlayerProximityPickup()
    {
        Vector3 playerPosition = Object.FindObjectOfType<PlayerCharacterController>().transform.position;
        float distanceToPlayer = Vector3.Distance(this.transform.position, playerPosition);

        if (distanceToPlayer < PickupProximity)
        {
            m_state = InteractionState.ReadyForPickup;
        }
    }

    void OnMouseDown()
    {
        // Prevent Player from walking when doing inventory clicks
        InputHandler inputHandler = FindObjectOfType(typeof(InputHandler)) as InputHandler;

        // Don't pick up items if you're in inventory mode
        if (inputHandler.GetInputMode() == InputHandler.InputMode.IN_INVENTORY)
        {
            return;
        }

        m_state = InteractionState.PendingPickup;
    }
}
