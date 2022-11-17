using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ReadyPlayerMe;

public class LoadAvatar : MonoBehaviour
{
    [SerializeField]
    private string defaultAvatarURL = "https://api.readyplayer.me/v1/avatars/632d65e99b4c6a4352a9b8db.glb";

    [SerializeField] private GameObject playerAvatar;
    [SerializeField] private GameObject templateAvatar;
    private GameObject defaultAvatar;

    [SerializeField] private RPMAvatarLoader rpmLoader;
    private MotionTransfer motionTransfer;
    private TMP_InputField urlInput;

    private Vector3 defaultPos, defaultRot, defaultScl;

    void Start()
    {
        motionTransfer = templateAvatar.GetComponent<MotionTransfer>();
        urlInput = GetComponent<TMP_InputField>();
        
        defaultAvatar = playerAvatar;
        defaultPos = playerAvatar.transform.position;
        defaultRot = playerAvatar.transform.eulerAngles;
        defaultScl = playerAvatar.transform.localScale;

        OnLoadClick();
    }

    public void OnLoadClick()
    {
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.OnCompleted += OnAvatarLoaded;
        avatarLoader.LoadAvatar(defaultAvatarURL);
    }

    public void OnResetClick()
    {
        urlInput.text = "";
        if (playerAvatar != defaultAvatar) OnAvatarLoaded(defaultAvatar, null);
    }

    private void OnAvatarLoaded(object sender, CompletionEventArgs args)
    {
        GameObject avatar = args.Avatar;
        AvatarMetadata metaData = args.Metadata;

        Debug.Log($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");
        
        if (playerAvatar == defaultAvatar) 
        {
            avatar.GetComponent<Animator>().enabled = false;
            playerAvatar.transform.position = new Vector3(0, -999, 0);
        }
        else
        {
            Destroy(playerAvatar);
        }

        // set avatar as motion transfer target
        Transform spine = avatar.transform.Find("Armature/Hips/Spine/Spine1/Spine2");
        motionTransfer.tgtLArm = spine.Find("LeftShoulder/LeftArm");
        motionTransfer.tgtRArm = spine.Find("RightShoulder/RightArm");
        motionTransfer.tgtHead = spine.Find("Neck/Head");
        motionTransfer.tgtFace = avatar.transform.Find("Renderer_Head").GetComponent<SkinnedMeshRenderer>();
        motionTransfer.tgtTeeth = avatar.transform.Find("Renderer_Teeth").GetComponent<SkinnedMeshRenderer>();

        avatar.transform.position = defaultPos;
        avatar.transform.eulerAngles = defaultRot;
        avatar.transform.localScale = defaultScl;
        
        playerAvatar = avatar;
        if (playerAvatar != defaultAvatar) 
        {
            playerAvatar.name = "Custom Avatar";
            AvatarURL avatarURL = playerAvatar.AddComponent(typeof(AvatarURL)) as AvatarURL;
            avatarURL.url = defaultAvatarURL;
            DontDestroyOnLoad(playerAvatar);
        }
    }
}
