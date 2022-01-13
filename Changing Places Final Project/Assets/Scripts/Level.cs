using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CodeMonkey;
//using CodeMonkey.Utils;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    #region Instance
    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }
    #endregion

    #region Constants
    private const float CAMERA_ORTHO_SIZE = 50F;
    private const float PIPE_WIDTH = 10f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -85f;
    private const float PIPE_SPAWN_X_POSITION = +85f;
    private const float BIRD_X_POSITION = 0f;
    #endregion

    #region Variables
    /// <summary>
    /// A list of the pipes spawned
    /// </summary>
    private List<Pipe> pipeList;
    /// <summary>
    /// Number of pipes bird has passed -> used for score
    /// </summary>
    private int pipesPassedCount;
    /// <summary>
    /// Number of pipes spawned
    /// </summary>
    private int pipesSpawned;
    /// <summary>
    /// Counts the actual time between the pipe spawns
    /// </summary>
    private float pipeSpawnTimer;
    /// <summary>
    /// Interval at which pipes spawn
    /// </summary>
    private float pipeSpawnTimerMax;
    /// <summary>
    /// Size of the gap between the pipes
    /// </summary>
    private float gapSize;
    /// <summary>
    /// Current state of the bird
    /// </summary>
    private State state;
    /// <summary>
    /// Game over window
    /// </summary>
    public GameObject gameOverWindow;
    #endregion

    #region Enum
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
        MoreImpossible
    }

    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead
    }

    public enum Theme
    {
        EasterIsland,
        Venice,
        MosqueCityOfBagerha,
        Galapagos,
        StatueOfLiberty
    }
    #endregion

    #region UI
    public Text scoreText;
    public Text highScoreText;
    public Text gameOverScoreText;
    public Text gameOverHighScoreText;
    #endregion

    public Theme currentTheme;
    public List<SpriteRenderer> pipeBodySpriteRenderers = new List<SpriteRenderer>();
    private void Awake()
    {
        instance = this;
        pipeList = new List<Pipe>();
        pipeSpawnTimerMax = 1.3f;
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }

    private void Start()
    {
        SetTheme(Theme.EasterIsland);

        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;

        gameOverWindow.SetActive(false);
        highScoreText.text = "HIGHSCORE: " + Score.GetHighScore().ToString();

        StartCoroutine(ShowEasterIslandTitle());
    }

    private void Update()
    {
        if ((state == State.Playing) & (Bird.GetInstance().state != Bird.State.WindowShowing))
        {
            HandlePipeMovement();
            HandlePipeSpawning(GameAssets.GetInstance().pfBody);
        }
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
        UpdateTheme();
    }

    #region Event Functions
    private void Bird_OnStartedPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        state = State.BirdDead;
        GameAssets.GetInstance().txtCountdown.enabled = false;
        gameOverScoreText.text = scoreText.text;
        if (int.Parse(scoreText.text) >= Score.GetHighScore())
        {
            // New high score
            gameOverHighScoreText.text = "NEW HIGHSCORE";
        } else
        {
            gameOverHighScoreText.text = "HIGHSCORE:" + Score.GetHighScore();
        }
        gameOverWindow.SetActive(true);
    }
    #endregion

    #region GameOver
    public void OnRetryClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region Difficulty
    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 60f;
                pipeSpawnTimerMax = 1.2f;
                break;
            case Difficulty.Medium:
                gapSize = 55f;
                pipeSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Hard:
                gapSize = 50f;
                pipeSpawnTimerMax = 1.0f;
                break;
            case Difficulty.Impossible:
                gapSize = 45f;
                pipeSpawnTimerMax = 0.9f;
                break;
            case Difficulty.MoreImpossible:
                gapSize = 45f;
                pipeSpawnTimerMax = 0.9f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= DifficultyVariables.impossiblePipesSpawned) return Difficulty.MoreImpossible;
        if (pipesSpawned >= DifficultyVariables.hardPipesSpawned) return Difficulty.Impossible;
        if (pipesSpawned >= DifficultyVariables.mediumPipesSpawned) return Difficulty.Hard;
        if (pipesSpawned >= DifficultyVariables.easyPipesSpawned) return Difficulty.Medium;
        return Difficulty.Easy;
    }
    #endregion

    #region Theme
    public void UpdateTheme()
    {
        switch (currentTheme)
        {
            case (Theme.EasterIsland):
                GameAssets.GetInstance().imgEasterIslandBackground.SetActive(true);
                GameAssets.GetInstance().imgVeniceBackground.SetActive(false);
                GameAssets.GetInstance().imgMosqueCityBackground.SetActive(false);
                GameAssets.GetInstance().imgGalapagosBackground.SetActive(false);
                GameAssets.GetInstance().imgStatueOfLiberty.SetActive(false);

                UpdatePipes(GameAssets.GetInstance().imgMoaiHead);
                break;
            case (Theme.Venice):
                GameAssets.GetInstance().imgEasterIslandBackground.SetActive(false);
                GameAssets.GetInstance().imgVeniceBackground.SetActive(true);
                GameAssets.GetInstance().imgMosqueCityBackground.SetActive(false);
                GameAssets.GetInstance().imgGalapagosBackground.SetActive(false);
                GameAssets.GetInstance().imgStatueOfLiberty.SetActive(false);

                UpdatePipes(GameAssets.GetInstance().imgPole);
                break;
            case (Theme.MosqueCityOfBagerha):
                GameAssets.GetInstance().imgEasterIslandBackground.SetActive(false);
                GameAssets.GetInstance().imgVeniceBackground.SetActive(false);
                GameAssets.GetInstance().imgMosqueCityBackground.SetActive(true);
                GameAssets.GetInstance().imgGalapagosBackground.SetActive(false);
                GameAssets.GetInstance().imgStatueOfLiberty.SetActive(false);

                UpdatePipes(GameAssets.GetInstance().imgDome);
                break;
            case (Theme.Galapagos):
                GameAssets.GetInstance().imgEasterIslandBackground.SetActive(false);
                GameAssets.GetInstance().imgVeniceBackground.SetActive(false);
                GameAssets.GetInstance().imgMosqueCityBackground.SetActive(false);
                GameAssets.GetInstance().imgGalapagosBackground.SetActive(true);
                GameAssets.GetInstance().imgStatueOfLiberty.SetActive(false);

                UpdatePipes(GameAssets.GetInstance().imgPenguin);
                break;
            case (Theme.StatueOfLiberty):
                GameAssets.GetInstance().imgEasterIslandBackground.SetActive(false);
                GameAssets.GetInstance().imgVeniceBackground.SetActive(false);
                GameAssets.GetInstance().imgMosqueCityBackground.SetActive(false);
                GameAssets.GetInstance().imgGalapagosBackground.SetActive(false);
                GameAssets.GetInstance().imgStatueOfLiberty.SetActive(true);

                UpdatePipes(GameAssets.GetInstance().imgBuilding);
                break;
        }
    }

    public void SetTheme(Theme theme)
    {
        currentTheme = theme;
    }

    public Theme GetTheme()
    {
        return currentTheme;
    }

    public void CheckThemeChange()
    {
        if (pipesPassedCount == DifficultyVariables.easyPipesSpawned)
        {
            Bird.GetInstance().state = Bird.State.WindowShowing;
            GameAssets.GetInstance().windowEasterIslandInfo.SetActive(true);
            GameAssets.GetInstance().windowEasterIslandInfo.GetComponent<Fade>().FadeIn(true);
        }
        else if (pipesPassedCount == DifficultyVariables.mediumPipesSpawned)
        {
            Bird.GetInstance().state = Bird.State.WindowShowing;
            GameAssets.GetInstance().windowVeniceInfo.SetActive(true);
            GameAssets.GetInstance().windowVeniceInfo.GetComponent<Fade>().FadeIn(true);
        }
        else if (pipesPassedCount == DifficultyVariables.hardPipesSpawned)
        {
            Bird.GetInstance().state = Bird.State.WindowShowing;
            GameAssets.GetInstance().windowMosqueCityInfo.SetActive(true);
            GameAssets.GetInstance().windowMosqueCityInfo.GetComponent<Fade>().FadeIn(true);
        }
        else if (pipesPassedCount == DifficultyVariables.impossiblePipesSpawned)
        {
            Bird.GetInstance().state = Bird.State.WindowShowing;
            GameAssets.GetInstance().windowGalapagosInfo.SetActive(true);
            GameAssets.GetInstance().windowGalapagosInfo.GetComponent<Fade>().FadeIn(true);
        }
        else if (pipesPassedCount == DifficultyVariables.moreImpossiblePipesSpawned)
        {
            Bird.GetInstance().state = Bird.State.WindowShowing;
            GameAssets.GetInstance().windowStatueOfLibertyInfo.SetActive(true);
            GameAssets.GetInstance().windowStatueOfLibertyInfo.GetComponent<Fade>().FadeIn(true);
        }
    }

    public void UpdatePipes(Sprite sprite)
    {
        foreach(SpriteRenderer spriteRenderer in pipeBodySpriteRenderers)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    public IEnumerator ShowEasterIslandTitle()
    {
        GameAssets.GetInstance().txtCountdown.text = "Easter Island";
        GameAssets.GetInstance().txtCountdown.enabled = true;
        yield return new WaitForSeconds(3);
        GameAssets.GetInstance().txtCountdown.enabled = false;
    }

    #endregion

    #region Pipes
    private void HandlePipeSpawning(Transform inputPipeBody)
    {
        pipeSpawnTimer -= Time.deltaTime;
        if(pipeSpawnTimer < 0)
        {
            // Time to spawn another Pipe
            pipeSpawnTimer += pipeSpawnTimerMax;

            float heightEdgeLimit = 10f; 
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION, inputPipeBody);
        }
    }

    private void HandlePipeMovement()
    {
        for(int i=0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];
            bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            // Scoring logic
            if(isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION & pipe.IsBottom())
            {
                // Pipe passed bird
                pipesPassedCount++;

                // Check if theme should change
                CheckThemeChange();

                Debug.Log(GetDifficulty());
                SoundManager.PlaySound(SoundManager.Sound.soundScore);
            }
            // Destroy logic
            if(pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                // Destroy pipe
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                pipeBodySpriteRenderers.Remove(pipeBodySpriteRenderers[0]);
                i--;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gapY">Height of the gap</param>
    /// <param name="gapSize">Size of the gap</param>
    /// <param name="xPosition"></param>
    private void CreateGapPipes(float gapY, float gapSize, float xPosition, Transform inputPipeBody)
    {
        // Bottom Pipe
        CreatePipe(gapY - gapSize * .5f, xPosition, true, inputPipeBody);
        // Top Pipe
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false, inputPipeBody);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreatePipe(float height, float xPosition, bool createBottom, Transform inputPipeBody)
    {
        // Pipe Body
        Transform pipeBody = Instantiate(inputPipeBody);
        float pipeBodyYPosition;
        if (createBottom)
        {
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeBodyYPosition = +CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);

        // Pipe Body Box Collider
        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderers.Add(pipeBodySpriteRenderer);
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * 0.5f);

        Pipe pipe = new Pipe(pipeBody, createBottom);
        pipeList.Add(pipe);
    }

    public int GetPipesSpawned()
    {
        return pipesSpawned;
    }

    public int GetPipesPassedCount()
    {
        return pipesPassedCount;
    }

    // Represents a single pipe 
    private class Pipe
    {
        private Transform pipeBodyTransform;
        private bool isBottom;

        public Pipe(Transform pipeBodyTransform, bool isBottom)
        {
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }

        public void Move()
        {
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeBodyTransform.position.x;
        }

        public bool IsBottom()
        {
            return isBottom;
        }

        public void DestroySelf()
        {
            Destroy(pipeBodyTransform.gameObject);
        }
    }
    #endregion
}
