using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Ownership : MonoBehaviourPun, IPunOwnershipCallbacks
{

    StateManager theStateManager;
    private void Awake(){
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if(targetView != base.photonView)
            return;

        base.photonView.TransferOwnership(requestingPlayer);    
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if(targetView != base.photonView)
            return;
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player previousOwner){
        Debug.Log("Failed");
    }

    // Start is called before the first frame update
    void Start()
    {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
       
    }

    public void askforOwnership(){
        base.photonView.RequestOwnership();
    }

    // Update is called once per frame
    void Update()
    {
        
            //Debug.Log("Asking");
           // base.photonView.RequestOwnership();
        
    }
}
