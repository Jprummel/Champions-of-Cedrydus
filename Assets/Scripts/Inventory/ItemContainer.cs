using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using Inventory;

[XmlRoot("ItemCollection")]
public class ItemContainer
{
    //[XmlArray("Weapons")]
    //[XmlArrayItem("Weapon")]
    //public List<WeaponItem> weapons = new List<WeaponItem>();

    //[XmlArray("ConsumableItems")]
    //[XmlArrayItem("ConsumableItem")]
    //public List<ConsumableItem> consumableItems = new List<ConsumableItem>();

    //public static ItemContainer Load(string path)
    //{
    //    TextAsset _xml = Resources.Load<TextAsset>(path);

    //    XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer));

    //    StringReader reader = new StringReader(_xml.text);

    //    ItemContainer items = serializer.Deserialize(reader) as ItemContainer;

    //    reader.Close();

    //    return items;
    //}
}