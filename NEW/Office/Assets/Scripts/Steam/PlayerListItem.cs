using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using TMPro;

public class PlayerListItem : MonoBehaviour
{
    public string PlayerName;
    public int ConnectionID;
    public ulong PlayerSteamID;
    private bool AvatarRecieved;

    public TMP_Text PlayerNameText;
    public TMP_Text PlayerReadyText;
    public bool Ready = false;

    public RawImage PlayerIcon;

    protected Callback<AvatarImageLoaded_t> ImageLoaded;

    private void Start(){
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }

    private void ChangeReadyStatus(){
        if(Ready){
            PlayerReadyText.text = "Ready";
            PlayerReadyText.color = Color.green;
        }
        else {
            PlayerReadyText.text = "Not Ready";
            PlayerReadyText.color = Color.red;
        }
    }
    
    private void OnImageLoaded(AvatarImageLoaded_t callback){
        if(callback.m_steamID.m_SteamID == PlayerSteamID){
            PlayerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
        }
    }

    private Texture2D GetSteamImageAsTexture(int iImage){
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        if(isValid){
            byte[] image = new byte[width * height * 4];
            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if(isValid){
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();

                texture = RotateTexture180(texture);
            }
        }
        AvatarRecieved = true;
        return texture;
    }

    private Texture2D RotateTexture180(Texture2D originalTexture){
        int width = originalTexture.width;
        int height = originalTexture.height;
        Texture2D rotatedTexture = new Texture2D(width, height);
        
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                rotatedTexture.SetPixel(x, y, originalTexture.GetPixel(width - x - 1, height - y - 1));
            }
        }
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    void GetPlayerIcon(){
        int ImageID = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamID);
        if(ImageID == -1) return;
        PlayerIcon.texture = GetSteamImageAsTexture(ImageID);
    }

    public void SetPLayerValues(){
        ChangeReadyStatus();
        PlayerNameText.text = PlayerName;

        if(!AvatarRecieved) GetPlayerIcon();
    }
}
