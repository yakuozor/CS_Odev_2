using UnityEngine;

public class Fish : MonoBehaviour
{
    public int price; 

    void Start()
    {
        
        switch (gameObject.tag)
        {
            case "Fish_Common":
                price = 5;
                break;
            case "Fish_Rare":
                price = 15;
                break;
            case "Fish_Legendary":
                price = 30;
                break;
            default:
                price = 0; 
                break;
        }
    }
}
