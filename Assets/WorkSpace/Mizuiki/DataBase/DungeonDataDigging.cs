using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData_", menuName = "CreateDataBase/Dungeon/DungeonDataDig")]
public class DungeonDataDigging : DungeonData
{
	[Header("��")]

	[Header("�����̃T�C�Y�̍ŏ��l�ƍő�l")]
	[SerializeField] private MyFunction.MinMax roomRange;
	[Header("���̒����͈̔�")]
	[SerializeField] private MyFunction.MinMax pathRange;
	[Header("�����̍ő吔"), Min(0)]
	[SerializeField] private int maxCount = 100;
	[Header("�����̐�����")]
	[SerializeField, Range(0.0f, 1.0f)] private float roomGenerateRate = 0.5f;
	[Header("�ʘH�����܂��Ă��銄��")]
	[SerializeField, Range(0.0f, 1.0f)] private float pathFillRate = 0.5f;

	public MyFunction.MinMax RoomRange => roomRange;
	public MyFunction.MinMax PathRange => pathRange;
	public int MaxCount => maxCount;
	public float RoomGenerateRate => roomGenerateRate;
	public float PathFillRate => pathFillRate;

}