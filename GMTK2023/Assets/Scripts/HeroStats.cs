using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(HeroAI))]
public class HeroStats : MonoBehaviour
{
    [SerializeField] private TMP_Text Health;
    [SerializeField] private TMP_Text Damage;
    [SerializeField] private TMP_Text Boredom;
    private HeroAI Hero;
    private void Start()
    {
        if (Health == null) throw new System.Exception("Health text reference missing");
        if (Damage == null) throw new System.Exception("Damage text reference missing");
        if (Boredom == null) throw new System.Exception("Boredom text reference missing");
        Hero = GetComponent<HeroAI>();
    }
    private void Update()
    {
        Health.text = $"{Hero.Health} HP";
        Damage.text = $"{Hero.Damage} Dmg";
        Boredom.text = $"{Hero.Boredom} Boredom";
    }
}