//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Unity.VisualScripting;
//using UnityEngine;

//public class charecterCreation : MonoBehaviour
//{
//    private List<GameObject> modls;
//    //defult index of the model
//    private int selectionIndex = 0;

//    // Start is called before the first frame update
//    void Start()
//    {
//        modls = new List<GameObject>();
//        foreach (Transform t in transform)
//        { 
//            modls.Add(t.gameObject);
//            t.gameObject.SetActive(false);
//        }
//        modls[selectionIndex].SetActive(true);
//    }



//    public void Select(int index)
//    {
//        if (index == selectionIndex)
//            return;

//        if (index < 0 || index >=modls.Count)
//            return;

//        modls[selectionIndex].SetActive(false);
//        selectionIndex = index;
//        modls[selectionIndex].SetActive(true) ;


//    }
//    // Update is called once per frame
//    void Update()
//    {
//        //if (Input.GetKeyDown(KeyCode.A))
//        //    Select(3);
//        if (Input.GetMouseButton(0))// if you need charecter to rotate with left mouse add this condition if you need to move it without click anything just follow mouse then remove this line
//            transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X"), 0.0f));
//    }
//}

//----------------------------------------------------------------------------------------------------------
//----------------- If we need to move charecter 360 on mouse click button---------------------------------
//----------------------------------------------------------------------------------------------------------
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CharacterCreation : MonoBehaviour
//{
//    private List<GameObject> models;
//    private int selectionIndex = 0;
//    private bool isRotating = false;  // Track if rotation is in progress
//    private float rotationSpeed = 120f;  // Adjust this value to control rotation speed

//    void Start()
//    {
//        models = new List<GameObject>();
//        foreach (Transform t in transform)
//        {
//            models.Add(t.gameObject);
//            t.gameObject.SetActive(false);
//        }
//        models[selectionIndex].SetActive(true);
//    }

//    public void Select(int index)
//    {
//        if (index == selectionIndex)
//            return;

//        if (index < 0 || index >= models.Count)
//            return;

//        models[selectionIndex].SetActive(false);
//        selectionIndex = index;
//        models[selectionIndex].SetActive(true);
//    }

//    public void RotateCharacter()
//    {
//        StartCoroutine(RotateCoroutine());
//    }

//    private IEnumerator RotateCoroutine()
//    {
//        isRotating = true;
//        float currentRotation = 0f;
//        float targetRotation = 360f;

//        while (currentRotation < targetRotation)
//        {
//            float rotationAmount = rotationSpeed * Time.deltaTime;
//            transform.Rotate(Vector3.up, rotationAmount);
//            currentRotation += rotationAmount;
//            yield return null;
//        }

//        isRotating = false;
//    }

//    void Update()
//    {
//        if (isRotating)
//            return;

//        if (Input.GetMouseButtonDown(0))
//        {
//            RotateCharacter();
//        }
//    }
//}
//-----------------------------------------------------------------------------------------------------------------------
//----------------- To Add charecter infromation when selecting charecter + charecter rotation 360 ----------------------
//-----------------------------------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterCreation : MonoBehaviour
{
    [System.Serializable]
    public class CharacterInfo
    {
        public string characterName;
        [TextArea]
        public string characterDescription;
        public AudioClip characterAudioClip; // Audio clip for each character
    }

    public List<CharacterInfo> characterInfos;

    private List<GameObject> models;
    private int selectionIndex = -1;
    private bool isRotating = false;
    private float initialRotationSpeed = 15f;
    private Coroutine rotateCoroutine;
    private float rotationSpeed;

    private Quaternion[] initialRotations; // Store the initial rotations of each character

    public TMP_Text infoText;
    private AudioSource audioSource;

    private void Start()
    {
        models = new List<GameObject>();
        initialRotations = new Quaternion[transform.childCount]; // Initialize the array

        for (int i = 0; i < transform.childCount; i++)
        {
            models.Add(transform.GetChild(i).gameObject);
            initialRotations[i] = models[i].transform.localRotation; // Store the initial rotation
            models[i].SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        UpdateInfoText();
    }

    public void Select(int index)
    {
        if (index == selectionIndex)
            return;

        if (index < 0 || index >= models.Count)
            return;

        if (selectionIndex >= 0)
        {
            models[selectionIndex].SetActive(false);
            ResetCharacterRotation(selectionIndex);
            StopCharacterSound(); // Stop the previous audio
        }

        selectionIndex = index;
        models[selectionIndex].SetActive(true);

        if (selectionIndex >= 0)
        {
            rotationSpeed = initialRotationSpeed;
            StartRotateCoroutine();
        }

        UpdateInfoText();

        PlayCharacterSound(index); // Play the character sound based on the selected index
    }

    private void PlayCharacterSound(int index)
    {
        if (index >= 0 && index < characterInfos.Count)
        {
            AudioClip characterAudioClip = characterInfos[index].characterAudioClip;
            if (characterAudioClip != null)
            {
                audioSource.clip = characterAudioClip;
                audioSource.Play();
            }
        }
    }

    private void StopCharacterSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void RotateCharacter()
    {
        if (selectionIndex >= 0)
        {
            StopRotateCoroutine();
            ResetCharacterRotation(selectionIndex); // Reset rotation before starting rotation again
            rotationSpeed = initialRotationSpeed;
            StartRotateCoroutine();
        }
    }

    private void StartRotateCoroutine()
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateCoroutine());
    }

    private void StopRotateCoroutine()
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }
    }

    private IEnumerator RotateCoroutine()
    {
        isRotating = true;
        float currentRotation = 0f;
        float targetRotation = 360f;
        float rotationAmount = rotationSpeed * Time.deltaTime;

        while (currentRotation < targetRotation)
        {
            models[selectionIndex].transform.Rotate(Vector3.up, rotationAmount);
            currentRotation += rotationAmount;
            yield return null;
        }

        isRotating = false;
    }

    private void ResetCharacterRotation(int index)
    {
        if (index >= 0 && index < models.Count)
        {
            models[index].transform.localRotation = initialRotations[index]; // Restore the initial rotation
        }
    }

    private void Update()
    {
        if (isRotating || selectionIndex < 0)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            RotateCharacter();
        }
    }

    private void UpdateInfoText()
    {
        if (infoText != null)
        {
            if (selectionIndex >= 0 && selectionIndex < characterInfos.Count)
            {
                string characterName = characterInfos[selectionIndex].characterName;
                string characterDescription = characterInfos[selectionIndex].characterDescription;

                string formattedCharacterName = $"<b><u>{characterName}</u></b>";
                string formattedText = $"{formattedCharacterName}\n\n{characterDescription}";

                infoText.text = formattedText;
            }
            else
            {
                infoText.text = "";
            }
        }
    }

    private void OnDisable()
    {
        StopRotateCoroutine();
    }

    private void OnEnable()
    {
        ResetCharacterRotation(selectionIndex);
    }
}