using UnityEngine;

[CreateAssetMenu(fileName = "_BaseObject", menuName = "Scriptable Objects/_BaseObject")]
public class _BaseItem : ScriptableObject
{
    [SerializeField] private Sprite itemSprite;

    public Sprite GetItemSprite(){
        return itemSprite;
    }
}
