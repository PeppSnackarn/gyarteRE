using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    private void Update()
    { 
        if(PlayerData.Instance && speedText)
            speedText.text = Mathf.RoundToInt(PlayerData.Instance.playerMovement.rb.velocity.magnitude).ToString();
    }
}
