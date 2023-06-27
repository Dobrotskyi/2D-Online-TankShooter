using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class ServerListing : MonoBehaviourPunCallbacks
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private ServerListItem _serverListItem;
    private List<ServerListItem> _listItems = new();


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            int index = _listItems.FindIndex(item => item.RoomInfo.Name == info.Name);
            if (info.RemovedFromList)
            {
                if (index != -1)
                {
                    Destroy(_listItems[index].gameObject);
                    _listItems.RemoveAt(index);
                }
            }
            else
            {
                if (index == -1)
                {
                    ServerListItem listItem = Instantiate(_serverListItem, _content);
                    listItem.SetInfo(info);
                    _listItems.Add(listItem);
                }
                else
                    _listItems[index].SetInfo(info);
            }
        }
    }

}
