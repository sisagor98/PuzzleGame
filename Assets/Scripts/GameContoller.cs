using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameContoller : MonoBehaviour
{
    [SerializeField]
    private Sprite BackgroundImage;

    public List<Button> btns = new List<Button>();

    public Sprite[] puzzles;


    public List<Sprite> gamePuzzles= new List<Sprite>(); 


    private bool firstGuess,secondGuess = false;
    private int countCorrectGuesses;
    private int countGuess;
    private int gameGuesses;
    private int firstGuessIndex,secondGuessIndex;
    private string firstGuessPuzzle,secondGuessPuzzle;

    public GameObject restart;

    void Awake(){
        puzzles = Resources.LoadAll<Sprite>("Script/pic");
    }


    void Start(){
        GetButtons();
        AddListener();
        AddGamePuzzle();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count/2;
    }
    void GetButtons(){
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        for(int i=0;  i<objects.Length; i++){
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = BackgroundImage;
        }
    }



    void AddGamePuzzle(){
        int looper = btns.Count;
        int index = 0;
        for(int i=0 ;i<looper;i++){
            if (index == looper/2){
                index=0;
            }
            gamePuzzles.Add(puzzles[index]);
            index ++;
        }
    }



    void AddListener(){
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle(){
       

        if(!firstGuess){
            firstGuess =true;
            firstGuessIndex= int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            firstGuessPuzzle=gamePuzzles[firstGuessIndex].name;


            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }else if(!secondGuess){
            secondGuess =true;
            secondGuessIndex= int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];


            if(firstGuessPuzzle == secondGuessPuzzle){
                Debug.Log("The Puzzle Match");
            }else{
                Debug.Log("The Puzzle don't Match");
            }
            countGuess++;

            StartCoroutine(ChecktIfPuzzleMatch());
        }
    }


    IEnumerator ChecktIfPuzzleMatch()
    {
        yield return new WaitForSeconds(1f);

        if(firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(.5f);

            btns [firstGuessIndex].interactable = false;
            btns [secondGuessIndex].interactable = false;


            btns [firstGuessIndex].image.color =new Color(0,0,0,0);
            btns [secondGuessIndex].image.color =new Color(0,0,0,0);

            ChackIfGameFinish();
        }
        else{
            yield return new WaitForSeconds(.5f);
            btns [firstGuessIndex].image.sprite =BackgroundImage;
            btns [secondGuessIndex].image.sprite =BackgroundImage;
        }

        yield return new WaitForSeconds(0.5f);
        firstGuess = secondGuess = false;
    }


    void ChackIfGameFinish(){
        countCorrectGuesses ++;

        if(countCorrectGuesses == gameGuesses){
            restart.SetActive(true);
            Debug.Log("GameFinish");
            Debug.Log("It took you" + countGuess +"Many guess to finish the game") ;
            
        }
    }

    void Shuffle(List<Sprite>list){
        for(int i=0;i< list.Count; i++){
            Sprite temp =list[i];
            int randomIndex= Random.Range(i,list.Count);
            list[i]=list[randomIndex];
            list[randomIndex]=temp;
        }
    }

    public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

}
