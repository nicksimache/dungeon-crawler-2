using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image toolbar;
    public Image inventoryToolbar;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    
}
