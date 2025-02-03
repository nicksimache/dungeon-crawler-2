using UnityEngine;

[CreateAssetMenu(fileName = "_BaseObject", menuName = "Scriptable Objects/_BaseObject")]
public class _BaseItem : ScriptableObject
{
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private GameObject itemPrefab;

    public Sprite GetItemSprite(){
        return itemSprite;
    }

    public GameObject GetItemPrefab(){
        return itemPrefab;
    }
}
