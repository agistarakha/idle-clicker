using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{

    [SerializeField] private Button _localButton;
    [SerializeField] private Button _cloudButton;
    // Start is called before the first frame update
    void Start()
    {

        //load local
        _localButton.onClick.AddListener(() =>

       {

           SetButtonInteractable(false);

           UserDataManager.LoadFromLocal();

           SceneManager.LoadScene(1);

       });


        //Load cloud
        _cloudButton.onClick.AddListener(() =>

       {

           SetButtonInteractable(false);

           StartCoroutine(UserDataManager.LoadFromCloud(() => SceneManager.LoadScene(1)));

       });

    }



    // Mendisable button agar tidak bisa ditekan
    private void SetButtonInteractable(bool interactable)
    {

        _localButton.interactable = interactable;

        _cloudButton.interactable = interactable;

    }

}
