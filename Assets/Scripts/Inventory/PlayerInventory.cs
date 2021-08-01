using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject Inventory;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GamePaused) return;

        if (InputManager.instance.GetKeyDown(InputActions.OpenInventory))
        {
            ItemMovement.instance.movementActive = !Inventory.activeSelf;
            Inventory.SetActive(!Inventory.activeSelf);
            GameManager.instance.panelOpen = Inventory.activeSelf;
        }
    }
}
