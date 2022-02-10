using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemDisplay : MonoBehaviour
{
    [field: SerializeField] private int TimeToHideCanvas { get; set; }
    [field: SerializeField] public Canvas ItemUICanvas { get; private set; }
    [field: SerializeField] public Image ItemSprite { get; private set; }
    
    [field: SerializeField] private List<ImgData> imgData = new List<ImgData>();

    [SerializeField] private Text _slottypeText = null;
    [SerializeField] private Text _rarityText = null;
    [SerializeField] private Text _levelText = null;
    [SerializeField] private Text _levelWminText = null;
    [SerializeField] private Text _valueText = null;
    [SerializeField] private Text _skillKeyText = null;
    [SerializeField] private Text _skillPrcntText = null;
    [SerializeField] private Text _skillEffectText = null;
    [SerializeField] private List<Text> _critChanceText;
    [SerializeField] private List<Text> _critDamageText;
    [SerializeField] private List<Text> _gemNameText = new List<Text>();

    #region Item Parced Data

    private ItemData _itemParcedData { get; set; }
    public string Type { get; set; }
    public string SlotType { get; set; }
    public string Rarity { get; set; }
    public int Level { get; set; }
    public int LevelWmin { get; set; }
    public string ItemKey { get; set; }
    public int Value { get; set; }
    public SkillData Skill { get; set; }
    public CritData CritData { get; set; }
    public GemsData Gems { get; set; }
    public int SetID { get; set; }

    private ItemManager itemManager;
    private WebImageParser<Sprite> imgParcer = new WebImageParser<Sprite>();

    #endregion

    private void Awake()
    {
        ItemUICanvas.enabled = false;
    }
    public void InitData(ItemData itemParcedData, float timeToShowItem)
    {
        Debug.Log("We are now initializing parced data and assign it to the class itself \n" +
            "in order to set desired sprite to our spawned item, we use dictionary that holds\n" +
            "data about url links of item's images that need to be set and keys which we compare to slot type of item\n" +
            "if key and slot type of item are equal we send a request to parse the desired image using value (which is url) of key");

        _itemParcedData = itemParcedData;
        Type = itemParcedData.type;
        SlotType = itemParcedData.slottype;
        Rarity = itemParcedData.rarity;
        Level = itemParcedData.level;
        LevelWmin = itemParcedData.level_wmin;
        ItemKey = itemParcedData.itemkey;
        Value = itemParcedData.value;
        Skill = itemParcedData.skill;
        CritData = itemParcedData.ext_vars;
        Gems = itemParcedData.gems;
        SetID = itemParcedData.set_id;

        ItemSprite.name = ItemKey;

        for (int i = 0; i < imgData.Count; i++)
        {
            if(SlotType == imgData[i].SlotType)
            {
                imgParcer.RequestParcing(imgData[i].Url, ReceiveImg);
            }
        }

        SetItemSprite(timeToShowItem, true);
        SetItemSprite(timeToShowItem + timeToShowItem, true);
    }

    public void SetItemInfo(float timeToHideItem)
    {
        Debug.Log("Here we proceed to assign parced data \n" +
            "to UI text in order to show item's name, stats and buffs");

        SetStringToText($"Type: {SlotType}", _slottypeText);
        SetStringToText($"Rarity: {Rarity}", _rarityText);
        SetStringToText($"Level: {Level}", _levelText);
        SetStringToText($"LevelWmin: {LevelWmin}", _levelWminText);
        SetStringToText($"Price: {Value}", _valueText);
        SetStringToText($"Skill: {Skill.key}", _skillKeyText);
        SetStringToText($"Percent: {Skill.prcnt}", _skillPrcntText);
        SetStringToText($"Effect: {Skill.effect}", _skillEffectText);

        for (int i = 0; i < CritData.count; i++)
        {
            if (_critChanceText.Count > CritData.count)
            {
                SetStringToText(" ", _critChanceText[i + CritData.count]);
                SetStringToText(" ", _critDamageText[i + CritData.count]);
            }
            SetStringToText($"Name: {CritData.vars[i].name}", _critChanceText[i]);
            SetStringToText($"Variable: {CritData.vars[i].value}", _critDamageText[i]);
            
        }

        for (int i = 0; i < Gems.count; i++)
        {
            if (_gemNameText.Count > Gems.count)
            {
                SetStringToText(" ", _gemNameText[i + Gems.count]);
            }

            SetStringToText($"Buff Name: {Gems.vars[i].name}", _gemNameText[i]);
        }
    }


    private void SetStringToText(string givenString, Text givenText)
    {
        givenText.text = givenString;
    }
    public void ReceiveImg(Sprite receiveImg)
    {
        Debug.Log("Once we retrived the desired image as a sprite\n" +
            "we override current sprite to retrived one and also we set canvas to true\n" +
            "because we now have all needed data to show spawned item");
        ItemSprite.overrideSprite = receiveImg;
        if(!ItemUICanvas.enabled)
            ItemUICanvas.enabled = true;
    }
    public async void SetItemSprite(float timeDelay, bool isActive)
    {
        await SetObjectActiveAsync(timeDelay, isActive);
    }
    private async Task SetObjectActiveAsync(float timeDelay, bool isActive)
    {
        await Task.Delay(TimeSpan.FromSeconds(timeDelay));

        if(ItemSprite != null)
            ItemSprite.enabled = isActive;


        return;
    }
    
    #region FillData methods
    public async void FillDataInSequenceOrder(int timeToShowItem, ChestData chestDat, int indexer)
    {
        Debug.Log("Async method lets to fill data to our desired class within time interval\n" +
            "we fill desired class with data, then display it in UI, then we do that again and override\n" +
            "previous data with new one");
        await FillDataInSequenceOrderAsync(timeToShowItem, chestDat, indexer);
    }
    private async Task FillDataInSequenceOrderAsync(int timeToShowItem, ChestData chestData, int indexer)
    {
        await Task.Delay(TimeSpan.FromSeconds(timeToShowItem));
        InitData(chestData.chest_items[indexer], timeToShowItem);
        SetItemInfo(timeToShowItem);
        if (indexer == chestData.chest_items.Count - 1)
        {
            await Task.Delay(TimeSpan.FromSeconds(TimeToHideCanvas));
            ItemUICanvas.enabled = false;
        }

        return;
    }
    #endregion

}

