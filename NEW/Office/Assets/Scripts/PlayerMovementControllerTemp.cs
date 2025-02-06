using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMovementControllerTemp : NetworkBehaviour
{
    public float Speed = 0.1f;
    public GameObject PlayerModel;

    private void Start()
    {
        PlayerModel.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "TempGameScene")
        {
            if (PlayerModel.activeSelf == false)
            {
                SetPosition();
                PlayerModel.SetActive(true);
            }

            if (isOwned)
            {
                Movement();
            }

        }
    }

    public void SetPosition()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(-5, 5));
    }


    public void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, 0.0f, y);

        transform.position += moveDir * Speed;
    }
}
