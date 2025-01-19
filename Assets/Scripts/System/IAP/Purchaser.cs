using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour
{
    public void OnPurchaseCompleted(Product product)
    {
        switch(product.definition.id)
        {
            case "com.serbull.iaptutorial.500coins":
                AddCoins();
                break;
        }
    }

    private void AddCoins()
    {
        ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, 500);
    }
}