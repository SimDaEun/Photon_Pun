using Photon.Pun;
using Photon.Realtime;
using Practices.UGUI_Management.UI;
using Practices.UGUI_Management.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Practices.PhotonPunClient.UI
{
    public class UI_Lobby : UI_Screen, ILobbyCallbacks, IMatchmakingCallbacks
    {
        [Resolve] RectTransform _roomListSlotContent;
        [Resolve] RoomListSlot _roomListSlot;
        [Resolve] Button _createRoom;
        [Resolve] Button _joinRoom;
        List<RoomListSlot> _roomListSlots = new List<RoomListSlot>(10);
        List<RoomInfo> _roomInfosCached = new List<RoomInfo>(10);
        int _roomIdSelected;

        protected override void Start()
        {
            base.Start();

            playerInputActions.UI.Click.performed += OnClick;
            _createRoom.onClick.AddListener(() =>
            {
                UI_CreateRoomOption createRoomOption = UI_Manager.instance.Resolve<UI_CreateRoomOption>();
                createRoomOption.Show();
            });
            _joinRoom.onClick.AddListener(() =>
            {
                UI_ConfirmWindow confirmWindow = UI_Manager.instance.Resolve<UI_ConfirmWindow>();
                RoomInfo roomInfo = _roomInfosCached[_roomIdSelected];

                if (!roomInfo.IsOpen)
                {
                    confirmWindow.Show("The room is closed.");
                    return;
                }

                if (roomInfo.PlayerCount >= roomInfo.MaxPlayers)
                {
                    confirmWindow.Show("The room is fulled.");
                    return;
                }

                PhotonNetwork.JoinRoom(roomInfo.Name);
            });
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }
        public override void Show()
        {
            base.Show();
            PhotonNetwork.AddCallbackTarget(this);
        }
        public override void Hide()
        {
            base.Hide();
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        public void OnJoinedLobby()
        {
            UI_ConfirmWindow confirmWindow = UI_Manager.instance.Resolve<UI_ConfirmWindow>();
            confirmWindow.Show("Joined lobby");
        }

        public void OnLeftLobby()
        {
            UI_ConfirmWindow confirmWindow = UI_Manager.instance.Resolve<UI_ConfirmWindow>();
            confirmWindow.Show("Left lobby");
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            RefreshSlots(roomList);
        }
        /// <summary>
        /// Refresh room list slots.
        /// </summary>
        /// <param name="roomList"></param>
        void RefreshSlots(List<RoomInfo> roomList)
        {
            RoomListSlot slotSelected = _roomListSlots.Find(slot => slot.roomId == _roomIdSelected);
            string selectedRoomName = slotSelected?.name;
            _joinRoom.interactable = false;

            //for 루프를 돌면서 기존 RoomListSlot들 파괴하기 
            for (int i = 0; i < _roomListSlots.Count; i++)
            {
                Destroy(_roomListSlots[i].gameObject);
            }

            _roomListSlots.Clear();
            _roomInfosCached.Clear();

            for (int i = 0; i < roomList.Count; i++)
            {
                RoomListSlot slot = Instantiate(_roomListSlot, _roomListSlotContent);
                slot.roomId = i;
                slot.roomName = roomList[i].Name;
                slot.roomPlayerCount = roomList[i].PlayerCount;
                slot.roomMaxPlayers = roomList[i].MaxPlayers;
                _roomListSlots.Add(slot);
                _roomInfosCached.Add(roomList[i]);

                if (roomList[i].Name.Equals(selectedRoomName))
                {
                    _roomIdSelected = i;
                    _joinRoom.interactable = true;
                }
            }
        }

        void OnClick(InputAction.CallbackContext context)
        {
            if (TryGraphicRaycast(Mouse.current.position.ReadValue(), out RoomListSlot slot))
            {
                SelectRoom(slot.roomId);
            }
        }

        void SelectRoom(int roomId)
        { 
            UI_ConfirmWindow confirmWindow = UI_Manager.instance.Resolve<UI_ConfirmWindow>();

            RoomInfo roomInfo = _roomInfosCached[roomId];

            if (!roomInfo.IsOpen)
            {
                _joinRoom.interactable = false;
                return;
            }
            if (roomInfo.PlayerCount >= roomInfo.MaxPlayers)
            {
                _joinRoom.interactable = false;
                return;
            }

        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinedRoom()
        {
            //TODO : show room UI
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            UI_ConfirmWindow confirmWindow = UI_Manager.instance.Resolve<UI_ConfirmWindow>();
            confirmWindow.Show(message);
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            UI_ConfirmWindow confirmWindow = UI_Manager.instance.Resolve<UI_ConfirmWindow>();
            confirmWindow.Show(message);
        }

        public void OnLeftRoom()
        {
        }
    }
}

