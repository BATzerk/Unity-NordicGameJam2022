using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPopup : MonoBehaviour {
    // Components
    [SerializeField] private TextMeshProUGUI t_headerL;
    [SerializeField] private TextMeshProUGUI t_headerR;
    [SerializeField] private TextMeshProUGUI t_headerTie;
    [SerializeField] private TextMeshProUGUI t_numGhoulsSlainLVal;
    [SerializeField] private TextMeshProUGUI t_numGhoulsSlainRVal;
    // References
    [SerializeField] private GameController gameController;

    private int numGhoulsL { get { return gameController.NumGhoulsSlainL; } }
    private int numGhoulsR { get { return gameController.NumGhoulsSlainR; } }


    public void Hide() {
        this.gameObject.SetActive(false);
    }
    public void Show() {
        this.gameObject.SetActive(true);

        t_numGhoulsSlainLVal.text = numGhoulsL.ToString();
        t_numGhoulsSlainRVal.text = numGhoulsR.ToString();

        t_headerL.enabled = false;
        t_headerR.enabled = false;
        t_headerTie.enabled = false;
        if (numGhoulsL == numGhoulsR) t_headerTie.enabled = true;
        else if (numGhoulsL > numGhoulsR) t_headerL.enabled = true;
        else t_headerR.enabled = true;
    }
}
