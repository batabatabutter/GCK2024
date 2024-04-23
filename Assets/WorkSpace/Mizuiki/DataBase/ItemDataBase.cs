
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "CreateDataBase/Item/ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
	public List<ItemData> item;
}
