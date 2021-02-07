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
    
    DateTime[] dateTimes = new DateTime[20];
    double[] timeDiffs = new double[20];

    int i = 0;
    string[] v = { "A", "A", "U", "A", "I", "A", "U", "E", "A", "A", "A", "I", "A", "I" };

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
    }

    private void Update() {

        if (Input.GetKeyDown("space")) {
            if(v.Length > i){
                ChangeImage(v[i]);

                //////
                DateTime now = System.DateTime.Now;
                double timeDiff = (now - dateTimes[i]).TotalMilliseconds;
                i++;
                timeDiffs[i] = timeDiff;
                dateTimes[i] = now;
            }else{
                Restart();
            }
        }
    }

    private void Restart() {
        i = 0;
        ChangeImage("");
    }

    // ---- This is to change Image/Mouth by catch keyCode from keyBoard
    // private void OnGUI() {
    //     Event e = Event.current;
    //     string[] vocals = { "A", "I", "U", "E", "O" };

    //     if (e.type == EventType.KeyDown && e.keyCode.ToString().Length == 1 ) {  
    //         ChangeImage(e.keyCode.ToString());
    //     }
    // }

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
            Debug.Log("recording....");
            audioSource = GetComponent<AudioSource> ();
            audioSource.clip = Microphone.Start("Built-in Audio Analog Stereo", true, 10, 44100); 

            // start timer
            dateTimes[0] = System.DateTime.Now;
            timeDiffs[0] = 0;

        }else{
            Debug.Log("playing....");
            audioSource.Play();
            Microphone.End("Built-in Audio Analog Stereo");  

            StartCoroutine(Replay());        
        }   
    }


    // Replay mouth
    private IEnumerator Replay(){
        Restart();
        foreach (var timeDiff in timeDiffs) {
            if(i < v.Length){
                ChangeImage(v[i]);
            }
            
            yield return new WaitForSeconds(1f);
            i++;
        }
    }
}
