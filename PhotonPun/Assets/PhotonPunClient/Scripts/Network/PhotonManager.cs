using Photon.Pun;
using UnityEngine;

namespace Practices.PhotonPunClient.Network
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        public static PhotonManager instance
        {
            get
            {
                if (instance == null)
                {
                    s_instance = new GameObject(nameof(PhotonManager)).AddComponent<PhotonManager>();
                }

                return s_instance;
            }
        }

        static PhotonManager s_instance;

        private void Awake()
        {
            if (s_instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                s_instance = this;
            }

            if (PhotonNetwork.IsConnected == false)
            {
                bool isConnected = PhotonNetwork.ConnectUsingSettings();
                Debug.Assert(isConnected, $"[{nameof(PhotonManager)}] Failed to connet to photon pun server.");
            }

            DontDestroyOnLoad(gameObject);
        }

        public override void OnConnectedToMaster()   //마스터 서버에 접속했는지 확인하는 함수 
        {
            base.OnConnectedToMaster();

            //PhotonNetwork.AutonaticallySyncScene // 현재 속해있는 방의 방장이 씬을 전환하면 따라서 전환하는 옵션 
            //PhotonNetwork.NickName 
            Debug.Log($"[{nameof(PhotonManager)}] Connected to master server.");
            PhotonNetwork.JoinLobby();  //로비에 진입 
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log($"{(nameof(PhotonManager))} Joined lobby");
        }
    }
}

