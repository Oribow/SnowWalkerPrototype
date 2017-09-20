using System.Collections.Generic;
using UnityEngine;

public class RythmGame : IFocusable
{
    public static RythmGame rythmGame;

    public delegate void GameEndCallback(bool success);

    public GameObject notePrefab;
    public UnityEngine.UI.Text errorCountText;
    public GameObject root;
    public Camera camera;
    public float spawnMinX;
    public float spawnMaxX;
    public float spawnY;
    public float touchAreaStartY;
    public float touchAreaEndY;
    public GameDifficulty easy;
    public GameDifficulty medium;
    public GameDifficulty hard;

    bool isGameRunning;
    List<Note> spawnedNotes;
    int errorCount;
    GameDifficulty currentGameDiff;
    GameEndCallback callback;
    float timeTillNextNote;
    int noteCount;
    bool spawnNotes;


    void Awake()
    {
        rythmGame = this;
    }

    void Update()
    {
        if (!isGameRunning)
            return;

        for (int i = 0; i < spawnedNotes.Count; i++)
        {
            var n = spawnedNotes[i];
            n.Update();
            if (n.gameObject.transform.localPosition.y < touchAreaEndY)
            {
                errorCount++;
                UpdateErrorCountText();
                Destroy(n.gameObject);
                spawnedNotes.RemoveAt(i);
                i--;
                if (errorCount >= currentGameDiff.maxErrors)
                    EndGame(false);
                else if (!spawnNotes && spawnedNotes.Count == 0)
                {
                    EndGame(true);
                }
            }
            else if (n.gameObject.transform.localPosition.y < touchAreaStartY)
            {
                foreach (Touch t in Input.touches)
                {
                    if (t.phase != TouchPhase.Began)
                        continue;

                    Vector2 touchWorld = transform.InverseTransformPoint(camera.ScreenToWorldPoint(t.position));
                    if (touchWorld.y > touchAreaStartY || touchWorld.y < touchAreaEndY)
                        continue;
                    DebugExtension.DebugCircle(touchWorld, Vector3.forward, Color.green, 0.4f, 1);
                    float screenXMin = camera.WorldToScreenPoint(n.gameObject.GetComponent<Renderer>().bounds.min).x;
                    float screenXMax = camera.WorldToScreenPoint(n.gameObject.GetComponent<Renderer>().bounds.max).x;
                    if (t.position.x > screenXMin && t.position.x < screenXMax)
                    {
                        Destroy(n.gameObject);
                        spawnedNotes.RemoveAt(i);
                        i--;
                        if (!spawnNotes && spawnedNotes.Count == 0)
                        {
                            EndGame(true);
                        }
                        break;
                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 touchWorld = transform.InverseTransformPoint(camera.ScreenToWorldPoint(Input.mousePosition));
                    if (touchWorld.y > touchAreaStartY || touchWorld.y < touchAreaEndY)
                        continue;
                    DebugExtension.DebugCircle(touchWorld, Vector3.forward, Color.green, 0.4f, 1);
                    float screenXMin = camera.WorldToScreenPoint(n.gameObject.GetComponent<Renderer>().bounds.min).x;
                    float screenXMax = camera.WorldToScreenPoint(n.gameObject.GetComponent<Renderer>().bounds.max).x;
                    if (Input.mousePosition.x > screenXMin && Input.mousePosition.x < screenXMax)
                    {
                        Destroy(n.gameObject);
                        spawnedNotes.RemoveAt(i);
                        i--;
                        if (!spawnNotes && spawnedNotes.Count == 0)
                        {
                            EndGame(true);
                        }
                        break;
                    }
                }
            }
        }

        if (spawnNotes)
        {
            timeTillNextNote -= Time.deltaTime;

            if (timeTillNextNote <= 0)
            {
                SpawnNote(currentGameDiff);
                timeTillNextNote = currentGameDiff.GetSpawnIntervall();
                noteCount++;

                if (noteCount >= currentGameDiff.noteCount)
                    spawnNotes = false;
            }
        }
    }

    public void StartGame(int difficulty, GameEndCallback callback)
    {
        FocusMe();
        this.callback = callback;
        isGameRunning = true;
        spawnedNotes = new List<Note>(4);
        errorCount = 0;
        noteCount = 0;
        spawnNotes = true;
        root.SetActive(true);
        WorldTime.Pause();

        if (difficulty == 1)
        {
            currentGameDiff = easy;
        }
        else if (difficulty == 2)
        {
            currentGameDiff = medium;
        }
        else
        {
            currentGameDiff = hard;
        }

        SpawnNote(currentGameDiff);
        timeTillNextNote = currentGameDiff.GetSpawnIntervall();
        UpdateErrorCountText();
    }

    void EndGame(bool success)
    {
        if (!success)
        {
            foreach (var n in spawnedNotes)
            {
                Destroy(n.gameObject);
            }
        }
        isGameRunning = false;
        root.SetActive(false);
        callback(success);
        WorldTime.Resume();
        DeFocusMe();
    }

    void UpdateErrorCountText()
    {
        string text = "";
        for (int i = 0; i < currentGameDiff.maxErrors - errorCount; i++)
        {
            text += "[!] ";
        }
        if(text != "")
        text.Remove(text.Length - 1);
        errorCountText.text = text;
    }

    void SpawnNote(GameDifficulty gameDiff)
    {
        Vector3 pos = new Vector3(Random.Range(spawnMinX, spawnMaxX), spawnY, 3);
        float velocity = gameDiff.GetNoteVelocity();
        GameObject clone = Instantiate<GameObject>(notePrefab, this.transform);
        clone.transform.localPosition = pos;

        spawnedNotes.Add(new Note(clone, velocity));
    }

    public override void OnGainFocus()
    {
        
    }

    public override void OnLoseFocus()
    {
        if (isGameRunning)
        {
            throw new System.Exception();
        }
    }

    [System.Serializable]
    public class GameDifficulty
    {
        [SerializeField]
        float maxVelocity;
        [SerializeField]
        float minVelocity;

        [SerializeField]
        public int noteCount;

        [SerializeField]
        float maxSpawnIntervall;
        [SerializeField]
        float minSpawnIntervall;

        [SerializeField]
        public int maxErrors;

        public float GetNoteVelocity()
        {
            return Random.Range(minVelocity, maxVelocity);
        }

        public float GetSpawnIntervall()
        {
            return Random.Range(minSpawnIntervall, maxSpawnIntervall);
        }
    }

    class Note
    {
        public float velocity;
        public GameObject gameObject;

        public Note(GameObject gameObject, float velocity)
        {
            this.velocity = velocity;
            this.gameObject = gameObject;
        }

        public void Update()
        {
            gameObject.transform.Translate(Vector3.down * velocity * Time.deltaTime);
        }
    }
}
