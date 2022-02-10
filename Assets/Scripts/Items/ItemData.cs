using System;
using System.Collections.Generic;


[Serializable]
public class ItemData
{
    public string                   type { get; set; }
    public string                   slottype { get; set; }
    public string                   rarity { get; set; }
    public int                      level { get; set; }
    public int                      level_wmin { get; set; }
    public string                   itemkey { get; set; }
    public int                      value { get; set; }
    public SkillData                skill { get; set; }
    public CritData                 ext_vars { get; set; }
    public GemsData                 gems { get; set; }
    public int                      set_id { get; set; }

}

