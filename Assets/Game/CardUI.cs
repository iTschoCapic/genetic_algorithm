using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    private RectTransform canvasRect;

    public void Initialize(Card card)
    {
        //actionText.text = card.Action.ToString();
        Sprite sprite = null;
        switch (card.Action)
        {
            case ActionType.NormalAttack:
                sprite = Resources.Load<Sprite>("Images/Carte_Attaque_Legere");
                break;
            case ActionType.HeavyAttack:
                sprite = Resources.Load<Sprite>("Images/Carte_Attaque_Lourde");
                break;
            case ActionType.Dodge:
                sprite = Resources.Load<Sprite>("Images/Carte_Esquive");
                break;
            case ActionType.Shield:
                sprite = Resources.Load<Sprite>("Images/Carte_Parade");
                break;
            case ActionType.Heal:
                sprite = Resources.Load<Sprite>("Images/Carte_Soin");
                break;
            default:
                sprite = null;
                break;
        }

        iconImage.sprite = sprite;

    }

}