using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wyczysc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void wyczysc()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        KierownikGry instancjaKierownikaGry = (KierownikGry)FindObjectsOfType(typeof(KierownikGry))[0];
        foreach (var l in instancjaKierownikaGry.linieGame)
        {
            if (l.activeInHierarchy)
            {
                instancjaKierownikaGry.kliknietoLinie(l.name);
            }
        }
    }
}
