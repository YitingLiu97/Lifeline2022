using System;
using UnityEngine;
using UnityEngine.UI;

namespace FrostweepGames.Plugins.GoogleCloud.SpeechRecognition.Examples
{
    public class GCSR_Example2 : MonoBehaviour
    {
        private GCSpeechRecognition _speechRecognition;
        public Text _resultText;
        public string fullSentence;

        //private Image _speechRecognitionState;

        // replace it with other VR buttons
        /*	private Button _startRecordButton,
						   _stopRecordButton;*/
        /*
				private InputField _commandsInputField;


				private Dropdown _languageDropdown;

				private RectTransform _objectForCommand;*/

        private void Start()
        {
            _speechRecognition = GCSpeechRecognition.Instance;
            _speechRecognition.RecognizeSuccessEvent += RecognizeSuccessEventHandler;
            _speechRecognition.RecognizeFailedEvent += RecognizeFailedEventHandler;

            _speechRecognition.FinishedRecordEvent += FinishedRecordEventHandler;
            _speechRecognition.StartedRecordEvent += StartedRecordEventHandler;
            _speechRecognition.RecordFailedEvent += RecordFailedEventHandler;

            _speechRecognition.EndTalkigEvent += EndTalkigEventHandler;

          //  _resultText = transform.Find("Canvas/Text_Result").GetComponent<Text>();

            /*_startRecordButton = transform.Find("Canvas/Button_StartRecord").GetComponent<Button>();
			_stopRecordButton = transform.Find("Canvas/Button_StopRecord").GetComponent<Button>();

			_speechRecognitionState = transform.Find("Canvas/Image_RecordState").GetComponent<Image>();


			_commandsInputField = transform.Find("Canvas/InputField_Commands").GetComponent<InputField>();

			_languageDropdown = transform.Find("Canvas/Dropdown_Language").GetComponent<Dropdown>();

			_objectForCommand = transform.Find("Canvas/Panel_PointArena/Image_Point").GetComponent<RectTransform>();

			_startRecordButton.onClick.AddListener(StartRecordButtonOnClickHandler);
			_stopRecordButton.onClick.AddListener(StopRecordButtonOnClickHandler);

			_startRecordButton.interactable = true;
			_stopRecordButton.interactable = false;
			_speechRecognitionState.color = Color.yellow;

			_languageDropdown.ClearOptions();*/

            _speechRecognition.RequestMicrophonePermission(null);

            /*for (int i = 0; i < Enum.GetNames(typeof(Enumerators.LanguageCode)).Length; i++)
			{
				_languageDropdown.options.Add(new Dropdown.OptionData(((Enumerators.LanguageCode)i).Parse()));
			}

			_languageDropdown.value = _languageDropdown.options.IndexOf(_languageDropdown.options.Find(x => x.text == Enumerators.LanguageCode.en_GB.Parse()));
*/

            // select first microphone device
            if (_speechRecognition.HasConnectedMicrophoneDevices())
            {
                _speechRecognition.SetMicrophoneDevice(_speechRecognition.GetMicrophoneDevices()[0]);
            }
        }

        private void OnDestroy()
        {
           // _speechRecognition.RecognizeSuccessEvent -= RecognizeSuccessEventHandler;
            _speechRecognition.RecognizeFailedEvent -= RecognizeFailedEventHandler;

            _speechRecognition.FinishedRecordEvent -= FinishedRecordEventHandler;
            _speechRecognition.StartedRecordEvent -= StartedRecordEventHandler;
            _speechRecognition.RecordFailedEvent -= RecordFailedEventHandler;

            _speechRecognition.EndTalkigEvent -= EndTalkigEventHandler;
        }

        // replace this with other onclick handler in Bina? or collision - sending the boolean 
        public void StartRecordButtonOnClickHandler()
        {
            /*_startRecordButton.interactable = false;
			_stopRecordButton.interactable = true;
			_resultText.text = string.Empty;
*/
            _speechRecognition.StartRecord(true);

            Debug.Log("start recording");
        }

        // replace this with out of collision, or click of a button, or long pause such as 2-3 seconds of no user input 
        public void StopRecordButtonOnClickHandler()
        {
            /*_stopRecordButton.interactable = false;
			_startRecordButton.interactable = true;*/

            _speechRecognition.StopRecord();
        }

        private void StartedRecordEventHandler()
        {
            //_speechRecognitionState.color = Color.red;
           
        }

        private void RecordFailedEventHandler()
        {
            /*_speechRecognitionState.color = Color.yellow;
			_resultText.text = "<color=red>Start record Failed. Please check microphone device and try again.</color>";

			_stopRecordButton.interactable = false;
			_startRecordButton.interactable = true;*/

            Debug.LogWarning("record failed");
        }

        private void EndTalkigEventHandler(AudioClip clip, float[] raw)
        {
            FinishedRecordEventHandler(clip, raw);
        }

        private void FinishedRecordEventHandler(AudioClip clip, float[] raw)
        {
            /*if (_startRecordButton.interactable)
			{
				_speechRecognitionState.color = Color.yellow;
			}*/

            if (clip == null)
                return;

            RecognitionConfig config = RecognitionConfig.GetDefault();
            //config.languageCode = ((Enumerators.LanguageCode)_languageDropdown.value).Parse();
            config.audioChannelCount = clip.channels;
            // configure other parameters of the config if need

            GeneralRecognitionRequest recognitionRequest = new GeneralRecognitionRequest()
            {
                audio = new RecognitionAudioContent()
                {
                    content = raw.ToBase64()
                },
                //audio = new RecognitionAudioUri() // for Google Cloud Storage object
                //{
                //	uri = "gs://bucketName/object_name"
                //},
                config = config
            };

            _speechRecognition.Recognize(recognitionRequest);
        }

        private void RecognizeFailedEventHandler(string error)
        {
            _resultText.text = "Recognize Failed: " + error;
        }

        private void RecognizeSuccessEventHandler(RecognitionResponse recognitionResponse)
        {

            _resultText.text = "Recognize Success.";
            InsertRecognitionResponseInfo(recognitionResponse);


        }

        private void GetOperationSuccessEventHandler(Operation operation)
        {
            _resultText.text = "Get Operation Success.\n";
            _resultText.text += "name: " + operation.name + "; done: " + operation.done;

            if (operation.done && (operation.error == null || string.IsNullOrEmpty(operation.error.message)))
            {
                InsertRecognitionResponseInfo(operation.response);
            }
        }


        /*Texts*/
        private void InsertRecognitionResponseInfo(RecognitionResponse recognitionResponse)
        {
            if (recognitionResponse == null || recognitionResponse.results.Length == 0)
            {
                _resultText.text = "\nWords not detected.";
                return;
            }

            _resultText.text += "\n" + recognitionResponse.results[0].alternatives[0].transcript;

            fullSentence = recognitionResponse.results[0].alternatives[0].transcript;

/*            GetComponent<saveToJson>().SaveToJson();
*/
            var words = recognitionResponse.results[0].alternatives[0].words;

            if (words != null)
            {
                string times = string.Empty;

                foreach (var item in recognitionResponse.results[0].alternatives[0].words)
                {
                    times += "<color=green>" + item.word + "</color> -  start: " + item.startTime + "; end: " + item.endTime + "\n";
                }

                _resultText.text += "\n" + times;
            }


            string other = "\nDetected alternatives: ";

            foreach (var result in recognitionResponse.results)
            {
                foreach (var alternative in result.alternatives)
                {
                    if (recognitionResponse.results[0].alternatives[0] != alternative)
                    {
                        other += alternative.transcript + ", ";
                    }
                }
            }

            _resultText.text += other;
        }
    }




}