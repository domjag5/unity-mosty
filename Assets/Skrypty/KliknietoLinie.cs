using UnityEngine;

public class KliknietoLinie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        KierownikGry instancjaKierownikaGry = (KierownikGry)FindObjectsOfType(typeof(KierownikGry))[0];
        instancjaKierownikaGry.kliknietoLinie(gameObject.name);
    }
}

