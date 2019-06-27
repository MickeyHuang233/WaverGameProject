using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyZAxis : MonoBehaviour
{
    void Start()
    {
        //根據自己Y軸計算Z軸的值
        transform.position = modifyZ();
    }

    // Update is called once per frame
    void Update()
    {
        //根據自己Y軸計算Z軸的值
        transform.position = modifyZ();
    }

    private Vector3 modifyZ()
    {
        return new Vector3(transform.position.x, transform.position.y, transform.position.y * PlayerMovement.multiplyZ);
    }
}
