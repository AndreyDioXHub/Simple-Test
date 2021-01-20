using UnityEngine;

public class HideUpIpad : MonoBehaviour
{
    public GameObject IpadModel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            IpadModel.SetActive(true);
        }
    }
    
}
