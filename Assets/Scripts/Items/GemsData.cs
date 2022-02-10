using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GemsData
{
    [field: SerializeField] public int              count { get; set; }
    [field: SerializeField] public List<VariableData>  vars { get; set; }

}


