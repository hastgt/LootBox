using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImgData
{
    [field: SerializeField] public string SlotType { get; set; }
    [field: SerializeField] public string Url { get; set; }
}
