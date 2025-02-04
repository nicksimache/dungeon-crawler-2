using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifetime = 5f;

    private void Start() {
        Destroy(gameObject, bulletLifetime);
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.layer != LayerMask.NameToLayer("Gun")){
            Destroy(gameObject);
        }
    }
}
