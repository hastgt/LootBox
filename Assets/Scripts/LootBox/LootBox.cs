using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;


public class LootBox : MonoBehaviour
{
    [field: SerializeField] private SpriteRenderer SpriteRenderer { get; set; }
    
    [SerializeField] private ItemManager _itemManager;

    private bool _isChestReceived;

    private void OnMouseDown()
    {
        if (!_isChestReceived)
        {
            _itemManager.ReceiveChestItems();
            _isChestReceived = true;

            SpriteRenderer.enabled = false;
        }
        else
        {
            Debug.Log("User have already opened chest");
        }
    }
}
