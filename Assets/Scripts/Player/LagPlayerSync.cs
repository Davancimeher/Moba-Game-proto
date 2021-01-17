using UnityEngine;
using Photon.Pun;

public class LagPlayerSync : MonoBehaviourPun, IPunObservable
{
    //Values that will be synced over network
    Vector3 latestPos;
    Quaternion latestRot;
    //Lag compensation
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;
    public bool InRespawn;
    public Vector3 SpawnPosition;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(InRespawn);
            stream.SendNext(SpawnPosition);
        }
        else
        {
            //Network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            InRespawn = (bool)stream.ReceiveNext();
            SpawnPosition = (Vector3)stream.ReceiveNext();

            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            if (!InRespawn)
            {
                //Lag compensation
                double timeToReachGoal = currentPacketTime - lastPacketTime;
                currentTime += Time.deltaTime;

                //Update remote player
                transform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
                transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, (float)(currentTime / timeToReachGoal));
            }
            else
            {
                transform.position = latestPos;
                transform.rotation = latestRot;
            }
        }
    }
}