using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTile : MonoBehaviour
{
    [SerializeField]
    private Text f;
    [SerializeField]
    private Text g;
    [SerializeField]
    private Text h;

    public Text F { get { f.gameObject.SetActive(true); return f; } set => f = value; }
    public Text G { get { g.gameObject.SetActive(true); return g; } set => g = value; }
    public Text H { get { h.gameObject.SetActive(true); return h; } set => h = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
