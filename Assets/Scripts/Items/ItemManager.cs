using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private readonly string _jsonFileUrl = "https://api.npoint.io/40aa59e3ec6be0998fa6";

    private int _timeToShowItem;

    [SerializeField] private ItemDisplay _itemDisplay;

    private readonly JSONParser<ChestData> _jsonParser = new JSONParser<ChestData>();
    private ChestData _chestData = new ChestData();

    public async void ReceiveChestItems()
    {
        _chestData = await _jsonParser.RequestParsingAsync(_jsonFileUrl);
    }

    public void SpawnItem(int timeToShowItem, ChestData chestData)
    {
        for (int i = 0; i < chestData.chest_items.Count; i++)
        {
            timeToShowItem += 1;
            _itemDisplay.FillDataInSequenceOrder(i + timeToShowItem, chestData, i);
        }
    }
}

