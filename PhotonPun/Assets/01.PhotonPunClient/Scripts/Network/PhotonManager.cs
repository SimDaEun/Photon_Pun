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

        public override void OnConnectedToMaster()   //������ ������ �����ߴ��� Ȯ���ϴ� �Լ� 
        {
            base.OnConnectedToMaster();

            //PhotonNetwork.AutonaticallySyncScene // ���� �����ִ� ���� ������ ���� ��ȯ�ϸ� ���� ��ȯ�ϴ� �ɼ� 
            //PhotonNetwork.NickName 
            Debug.Log($"[{nameof(PhotonManager)}] Connected to master server.");
            PhotonNetwork.JoinLobby();  //�κ� ���� 
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log($"{(nameof(PhotonManager))} Joined lobby");
        }
    }
}

