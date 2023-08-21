using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugReportOpen : MonoBehaviour
{
    public void OpenbugReport()
    {
        Application.OpenURL("https://github.com/Gooseman03/RedGiant/issues");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8)) { OpenbugReport(); }
    }
}
