using System; 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MouthController : MonoBehaviour
{
    AudioSource audioSource;
    bool recording = false;

    public GameObject img;

    public Sprite spriteNeutral;
    public Sprite spriteA;
    public Sprite spriteI;
    public Sprite spriteU;
    public Sprite spriteE;
    public Sprite spriteO;

    public Button btnNeutral;
    public Button btnA;
    public Button btnI;
    public Button btnU;
    public Button btnE;
    public Button btnO;

    public Button btnVoiceRecording;
    public Button btnRestart;
    public Button btnReplay;

    DateTime[] dateTimes = new DateTime[30];
    double[] timeDiffs = new double[30];

    int i = 0;
    string[] v = { "A", "A", "U", "A", "I", "A", "U", "E", "A", "A", "A", "I", "A", "I", "A", "U", "U", "A", "A", "A", "U", "A", "U", "E", "E", "A" };
    // namaku nawi aku berasal dari bali aku suka makan rujak super pedas

    private void Start()
    {
        // ---- This is to change Image/Mouth by catch clicked mouth button from screen
        // btnNeutral.onClick.AddListener(delegate { ChangeImage(""); });
        // btnA.onClick.AddListener(delegate { ChangeImage("A"); });
        // btnI.onClick.AddListener(delegate { ChangeImage("I"); });
        // btnU.onClick.AddListener(delegate { ChangeImage("U"); });
        // btnE.onClick.AddListener(delegate { ChangeImage("E"); });
        // btnO.onClick.AddListener(delegate { ChangeImage("O"); });

        btnVoiceRecording.onClick.AddListener(delegate { VoiceRecording(); });
        btnRestart.onClick.AddListener(delegate { Restart(); });
        btnReplay.onClick.AddListener(delegate { StartCoroutine(Replay()); });
    }

    private void Update() {

        if (Input.GetKeyDown("space")) {
            if(v.Length > i){
                ChangeImage(v[i]);
                recordMouthChanges();

            }
            else{
                Restart();
            }
        }
    }

    private void recordMouthChanges()
    {
        DateTime now = System.DateTime.Now;
        if (i == 0)
        {
            timeDiffs[i] = (now - dateTimes[0]).TotalSeconds;
        }
        else
        {
            timeDiffs[i] = (now - dateTimes[i - 1]).TotalSeconds;
        }
        dateTimes[i] = now;
        i++;
    }

    private void Restart() {
        i = 0;
        ChangeImage("");
    }

    // ---- This is to change Image/Mouth by catch keyCode from keyBoard
    //private void OnGUI()
    //{
    //    event e = event.current;
    //    string[] vocals = { "a", "i", "u", "e", "o" };

    //    if (e.type == eventtype.keydown && e.keycode.tostring().length == 1 ) {  
    //         changeimage(e.keycode.tostring());
    //        recordMouthChanges();
    //    }
    //}

    private void ChangeImage(string keyCode)
    {
        Sprite newSprite;

        switch(keyCode) {
            case "A":
                newSprite = spriteA;
                break;
                
            case "I":
                newSprite = spriteI;
                break;
                
            case "U":
                newSprite = spriteU;
                break;
                
            case "E":
                newSprite = spriteE;
                break;
                
            case "O":
                newSprite = spriteO;
                break;
            
            default :
                newSprite = spriteNeutral;
                break;
        }
        
        img.GetComponent<Image>().sprite = newSprite;
    }

    private void VoiceRecording()
    {
        Debug.Log("VoiceRecording....");
        recording = !recording;

        if(recording){ 
            // start timer
            dateTimes[0] = System.DateTime.Now;
            timeDiffs[0] = 0;

            Debug.Log("recording....");
            audioSource = GetComponent<AudioSource> ();
            audioSource.clip = Microphone.Start("Microphone (High Definition Audio Device)", true, 60, 44100); // <-- windows 10 PC
            // audioSource.clip = Microphone.Start("Built-in Audio Analog Stereo", true, 10, 44100); // <-- linux laptop

            dateTimes[0] = System.DateTime.Now;

        }else{
            Microphone.End("Built-in Audio Analog Stereo");
            StartCoroutine(Replay());        
        }   
    }


    // Replay mouth
    private IEnumerator Replay(){
        Debug.Log("playing....");
        Restart();
        audioSource.Play();
        foreach (var timeDiff in timeDiffs) {
            Debug.Log((float)timeDiff);
            yield return new WaitForSeconds((float)timeDiff);

            if(i < v.Length){
                // Debug.Log(i);
                // Debug.Log(v[i]);
                // Debug.Log(timeDiff);
                // Debug.Log("____________________________");
                ChangeImage(v[i]);
            }
            i++;
        }
    }
}
