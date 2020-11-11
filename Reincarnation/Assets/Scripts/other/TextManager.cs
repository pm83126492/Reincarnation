using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text text;
    private TextWriter.TextWriterSingle textWriterSingle;
    int TextNumber;
    string[] messageArray = new string[]
                {
                "傳說，人死後靈魂會到地獄受審",
                "善良之人，將轉世成人",
                "罪惡之人，將打入十殿地獄受刑......",
                };
    // Start is called before the first frame update

    private void Start()
    {
        textWriterSingle = TextWriter.AddWriter_Static(text, messageArray[0], .1f, true, true);
        TextNumber += 1;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textWriterSingle != null && textWriterSingle.IsActive())
            {
                textWriterSingle.WriteAllAndDestroy();
            }
            else
            {
                string message = messageArray[TextNumber];
                TextNumber += 1;
                textWriterSingle = TextWriter.AddWriter_Static(text, message, .1f, true, true);
            }
        }
    }
}