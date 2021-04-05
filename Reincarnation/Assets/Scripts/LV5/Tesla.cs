using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesla : MonoBehaviour,IPoolObject
{
    public bool isStart;
    public int StartNumber;
    public GameObject[] TeslaChild;

    public void OnObjectSpawn()
    {
        if (RunnerKingController.WinNumber <= 7)
        {
            TeslaChild[0].transform.localPosition = new Vector3(Random.Range(-12, -8), TeslaChild[0].transform.position.y, TeslaChild[0].transform.position.z);
            TeslaChild[1].transform.localPosition = new Vector3(Random.Range(-7, -3), TeslaChild[1].transform.position.y, TeslaChild[1].transform.position.z);
            TeslaChild[2].transform.localPosition = new Vector3(Random.Range(-2, 2), TeslaChild[2].transform.position.y, TeslaChild[2].transform.position.z);
            TeslaChild[3].transform.localPosition = new Vector3(Random.Range(3, 7), TeslaChild[3].transform.position.y, TeslaChild[3].transform.position.z);
            TeslaChild[4].transform.localPosition = new Vector3(Random.Range(8, 12), TeslaChild[4].transform.position.y, TeslaChild[4].transform.position.z);
        }
        else if (RunnerKingController.WinNumber > 7 && RunnerKingController.WinNumber <= 14)
        {
            TeslaChild[5].SetActive(true);
            TeslaChild[6].SetActive(true);

            TeslaChild[0].transform.localPosition = new Vector3(Random.Range(-12, -10), TeslaChild[0].transform.position.y, TeslaChild[0].transform.position.z);
            TeslaChild[1].transform.localPosition = new Vector3(Random.Range(-9, -6), TeslaChild[1].transform.position.y, TeslaChild[1].transform.position.z);
            TeslaChild[2].transform.localPosition = new Vector3(Random.Range(-5, -2), TeslaChild[2].transform.position.y, TeslaChild[2].transform.position.z);
            TeslaChild[3].transform.localPosition = new Vector3(Random.Range(-1, 1), TeslaChild[3].transform.position.y, TeslaChild[3].transform.position.z);
            TeslaChild[4].transform.localPosition = new Vector3(Random.Range(2, 5), TeslaChild[4].transform.position.y, TeslaChild[4].transform.position.z);
            TeslaChild[5].transform.localPosition = new Vector3(Random.Range(6, 9), TeslaChild[5].transform.position.y, TeslaChild[5].transform.position.z);
            TeslaChild[6].transform.localPosition = new Vector3(Random.Range(10, 12), TeslaChild[6].transform.position.y, TeslaChild[6].transform.position.z);
        }
        else if (RunnerKingController.WinNumber > 14 && RunnerKingController.WinNumber <= 20)
        {
            TeslaChild[7].SetActive(true);
            TeslaChild[8].SetActive(true);

            TeslaChild[0].transform.localPosition = new Vector3(Random.Range(-12, -11), TeslaChild[0].transform.position.y, TeslaChild[0].transform.position.z);
            TeslaChild[1].transform.localPosition = new Vector3(Random.Range(-10, -8), TeslaChild[1].transform.position.y, TeslaChild[1].transform.position.z);
            TeslaChild[2].transform.localPosition = new Vector3(Random.Range(-7, -5), TeslaChild[2].transform.position.y, TeslaChild[2].transform.position.z);
            TeslaChild[3].transform.localPosition = new Vector3(Random.Range(-4, -2), TeslaChild[3].transform.position.y, TeslaChild[3].transform.position.z);
            TeslaChild[4].transform.localPosition = new Vector3(Random.Range(-1, 1), TeslaChild[4].transform.position.y, TeslaChild[4].transform.position.z);
            TeslaChild[5].transform.localPosition = new Vector3(Random.Range(2, 4), TeslaChild[5].transform.position.y, TeslaChild[5].transform.position.z);
            TeslaChild[6].transform.localPosition = new Vector3(Random.Range(5, 7), TeslaChild[6].transform.position.y, TeslaChild[6].transform.position.z);
            TeslaChild[7].transform.localPosition = new Vector3(Random.Range(8, 10), TeslaChild[7].transform.position.y, TeslaChild[5].transform.position.z);
            TeslaChild[8].transform.localPosition = new Vector3(Random.Range(11, 12), TeslaChild[8].transform.position.y, TeslaChild[6].transform.position.z);
        }
        isStart = true;
        Invoke("End", 3f);
    }

    void End()
    {
        isStart = false;
    }
}
