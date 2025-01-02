using Photon.Pun;
using Photon.Realtime;
using Practices.UGUI_Management.UI;
using Practices.UGUI_Management.Utilities;
using TMPro;
using UnityEngine.UI;

namespace Practices.PhotonPunClient.UI
{
    public class UI_CreateRoomOption : UI_Popup
    {
        [Resolve] TMP_InputField _roomName;
        [Resolve] TMP_InputField _roomMaxPlayers;
        [Resolve] Button _confirm;
        [Resolve] Button _cancel;

        const int ROOM_MAX_PLAYERS_LIMIT = 8;
        const int ROOM_MIN_PLAYERS_LIMIT = 1;
        protected override void Start()
        {
            base.Start();

            _roomMaxPlayers.onValueChanged.AddListener(value =>
            {
                if (int.TryParse(value, out int paresd))
                {
                    if (paresd > ROOM_MAX_PLAYERS_LIMIT)
                      _roomMaxPlayers.SetTextWithoutNotify(ROOM_MAX_PLAYERS_LIMIT.ToString()); //text property 쓰면 무한루프 걸림
                    if (paresd < ROOM_MIN_PLAYERS_LIMIT)
                        _roomMaxPlayers.SetTextWithoutNotify(ROOM_MIN_PLAYERS_LIMIT.ToString());
                }
                else
                {
                    _roomMaxPlayers.SetTextWithoutNotify(ROOM_MIN_PLAYERS_LIMIT.ToString());
                }
            });
            _confirm.onClick.AddListener(() =>
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = int.Parse(_roomMaxPlayers.text);
                PhotonNetwork.CreateRoom(_roomName.text, roomOptions);
            });
            _cancel.onClick.AddListener(Hide);
        }
    }
}