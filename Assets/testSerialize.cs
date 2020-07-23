using System.Collections;
using System.Collections.Generic;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers;
using UnityEngine;
using UnityEngine.UI;

public class testSerialize : MonoBehaviour , IGsLiveSerializable
{
    public Text Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGsLiveRead(GsReadStream readStream)
    {
        Text.text += readStream.ReadNext() + "\r\n";
        Text.text += readStream.ReadNext() + "\r\n";
    }

    public void OnGsLiveWrite(GsWriteStream writeStream)
    {
        writeStream.WriteNext(new Quaternion(1.1f,2.2f,3.3f,4.4f));
        writeStream.WriteNext(new Matrix4x4());
    }
}
