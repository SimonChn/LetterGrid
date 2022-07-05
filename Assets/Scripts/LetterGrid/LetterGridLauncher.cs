using UnityEngine;

public class LetterGridLauncher : MonoBehaviour
{
    //Pool
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private LetterGridController gridController;

    [Space(20)]
    [SerializeField] private InputController inputController;

    public void Launch()
    {
        gridGenerator.Init();
        gridController.Init(gridGenerator);

        inputController.Init(gridController);

        Destroy(this);
    }

    private void Start()
    {
        Launch();
    }
}
