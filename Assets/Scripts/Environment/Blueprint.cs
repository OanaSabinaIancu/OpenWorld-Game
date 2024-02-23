using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public string name;
    public string req1;
    public string req2;
    public string req3;

    public int req1Amount;
    public int req2Amount;
    public int req3Amount;

    public Blueprint(string name, int req1Amount, string req1, int req2Amount, string req2, int req3Amount, string req3)
    {
        this.name = name;
        this.req1 = req1;
        this.req2 = req2;
        this.req3 = req3;
        this.req1Amount = req1Amount;
        this.req2Amount = req2Amount;
        this.req3Amount = req3Amount;
    }
}
