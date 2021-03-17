using UnityEngine;
using Photon.Pun;

public class LagPlayerSync : MonoBehaviourPun, IPunObservable
{
    public Vector3 SpawnPosition;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(SpawnPosition);
        }
        else
        {
            SpawnPosition = (Vector3)stream.ReceiveNext();
        }
    }
}