using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttack : MonoBehaviour, IPoolObject
{
    public GameObject[] GroundAttackObject;
    public float lifetime = 2.0f;

    public void OnObjectSpawn()
    {
        Debug.Log("OK");
        if (RunnerKingController.WinNumber <= 7)
        {
            GroundAttackObject[0].transform.localPosition = new Vector3(4, 0.33f, GroundAttackObject[0].transform.position.z);
            GroundAttackObject[1].transform.localPosition = new Vector3(16, 0.33f, GroundAttackObject[1].transform.position.z);
            GroundAttackObject[2].transform.localPosition = new Vector3(-8, 0.33f, GroundAttackObject[2].transform.position.z);
            GroundAttackObject[3].transform.localPosition = new Vector3(-20, 0.33f, GroundAttackObject[3].transform.position.z);
        }
        else if (RunnerKingController.WinNumber > 7 && RunnerKingController.WinNumber <= 14)
        {
            GroundAttackObject[0].transform.localPosition = new Vector3(4, 0.33f, GroundAttackObject[0].transform.position.z);
            GroundAttackObject[1].transform.localPosition = new Vector3(15, 0.33f, GroundAttackObject[1].transform.position.z);
            GroundAttackObject[2].transform.localPosition = new Vector3(-7, 0.33f, GroundAttackObject[2].transform.position.z);
            GroundAttackObject[3].transform.localPosition = new Vector3(-18, 0.33f, GroundAttackObject[3].transform.position.z);
        }
        else if (RunnerKingController.WinNumber > 14)
        {
            GroundAttackObject[0].transform.localPosition = new Vector3(4, 0.33f, GroundAttackObject[0].transform.position.z);
            GroundAttackObject[1].transform.localPosition = new Vector3(14, 0.33f, GroundAttackObject[1].transform.position.z);
            GroundAttackObject[2].transform.localPosition = new Vector3(-6, 0.33f, GroundAttackObject[2].transform.position.z);
            GroundAttackObject[3].transform.localPosition = new Vector3(-16, 0.33f, GroundAttackObject[3].transform.position.z);
        }
        Invoke("Close", lifetime);
    }

    void Close()
    {
        gameObject.SetActive(false);
    }
}
