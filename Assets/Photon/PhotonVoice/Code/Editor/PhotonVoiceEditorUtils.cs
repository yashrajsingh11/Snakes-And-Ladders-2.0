using System;
using System.IO;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;

namespace Photon.Voice.Unity.Editor
{
    public static class PhotonVoiceEditorUtils
    {
        /// <summary>True if the ChatClient of the Photon Chat API is available. If so, the editor may (e.g.) show additional options in settings.</summary>
        public static bool HasChat
        {
            get
            {
                return Type.GetType("Photon.Chat.ChatClient, Assembly-CSharp") != null || Type.GetType("Photon.Chat.ChatClient, Assembly-CSharp-firstpass") != null || Type.GetType("Photon.Chat.ChatClient, PhotonChat") != null;
            }
        }

        /// <summary>True if the PhotonNetwork of the PUN is available. If so, the editor may (e.g.) show additional options in settings.</summary>
        public static bool HasPun
        {
            get
            {
                return Type.GetType("Photon.Pun.PhotonNetwork, Assembly-CSharp") != null || Type.GetType("Photon.Pun.PhotonNetwork, Assembly-CSharp-firstpass") != null || Type.GetType("Photon.Pun.PhotonNetwork, PhotonUnityNetworking") != null;
            }
        }
        
        #if PHOTON_UNITY_NETWORKING
        [MenuItem("Window/Photon Voice/Remove PUN", false, 1)]
        private static void RemovePun()
        {
            if (!HasPun)
            {
                Debug.LogWarning("PUN already removed!");
                return;
            }
            DeleteDirectory("Assets/Photon/PhotonVoice/Demos/DemoProximityVoiceChat");
            DeleteDirectory("Assets/Photon/PhotonVoice/Demos/DemoVoicePun");
            DeleteDirectory("Assets/Photon/PhotonVoice/Code/PUN");
            DeleteDirectory("Assets/Photon/PhotonUnityNetworking");
            PhotonEditorUtils.CleanUpPunDefineSymbols();
        }
        #endif

        [MenuItem("Window/Photon Voice/Remove Photon Chat", false, 2)]
        private static void RemovePhotonChat()
        {
            if (!HasChat)
            {
                Debug.LogWarning("Photon Chat already removed!");
                return;
            }
            DeleteDirectory("Assets/Photon/PhotonChat");
        }

        [MenuItem("Window/Photon Voice/Leave a review", false, 3)]
        private static void OpenAssetStorePage()
        {
            Application.OpenURL("https://assetstore.unity.com/packages/tools/audio/photon-voice-2-130518");
        }

        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                if (!FileUtil.DeleteFileOrDirectory(path))
                {
                    Debug.LogWarningFormat("Directory \"{0}\" not deleted.", path);
                }
                DeleteFile(string.Concat(path, ".meta"));
            }
            else
            {
                Debug.LogWarningFormat("Directory \"{0}\" does not exist.", path);
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                if (!FileUtil.DeleteFileOrDirectory(path))
                {
                    Debug.LogWarningFormat("File \"{0}\" not deleted.", path);
                }
            }
            else
            {
                Debug.LogWarningFormat("File \"{0}\" does not exist.", path);
            }
        }
        
        public static bool IsInTheSceneInPlayMode(GameObject go)
        {
            return Application.isPlaying && !PhotonEditorUtils.IsPrefab(go);
        }
    }
}