using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CityCheckPointDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.gameObject.name);
        if(other.GetComponent<ThirdPersonController>())
        {
            if (other.GetComponent<ThirdPersonController>().GetComponent<PhotonView>().IsMine)
            {

                this.gameObject.SetActive(false);
                

                    MissionSystem.Instance.MissionTaskComplete();

                

            }
        }
        else if(other.GetComponentInParent<PrometeoCarController>())
        {
            if(other.GetComponentInParent<PrometeoCarController>().TryGetComponent<PhotonView>(out PhotonView pv))
            {
                if (pv.IsMine)
                {
                    this.gameObject.SetActive(false);


                    

                    MissionSystem.Instance.MissionTaskComplete();
                }
            }
        }
    }

   
}
