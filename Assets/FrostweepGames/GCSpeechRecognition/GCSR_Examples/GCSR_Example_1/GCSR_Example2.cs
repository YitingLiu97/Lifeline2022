using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Collecting user repsonses 
/// put that in a list 
/// following questions 
/// getting the value - sending it to bubble data 
/// based on mic input - once it is stopped, instantiate the bubble 
/// </summary>

namespace FrostweepGames.Plugins.GoogleCloud.SpeechRecognition.Examples
{
    public class GCSR_Example2 : MonoBehaviour
    {
        private GCSpeechRecognition _speechRecognition;
        public Text _resultText;
        public string fullSentence;
        public List<string> userResponses; 
        private void Start()
        {
            _speechRecognition = GCSpeechRecognition.Instance;
            _speechRecognition.RecognizeSuccessEvent += RecognizeSuccessEventHandler;
            _speechRecognition.RecognizeFailedEvent += RecognizeFailedEventHandler;

            _speechRecognition.FinishedRecordEvent += FinishedRecordEventHandler;
            _speechRecognition.StartedRecordEvent += StartedRecordEventHandler;
            _speechRecognition.RecordFailedEvent += RecordFailedEventHandler;

            _speechRecognition.EndTalkigEvent += EndTalkigEventHandler;

            _speechRecognition.RequestMicrophonePermission(null);

            // select first microphone device
            if (_speechRecognition.HasConnectedMicrophoneDevices())
            {
                _speechRecognition.SetMicrophoneDevice(_speechRecognition.GetMicrophoneDevices()[0]);
            }
        }

        private void OnDestroy()
        {
            _speechRecognition.RecognizeFailedEvent -= RecognizeFailedEventHandler;
            _speechRecognition.FinishedRecordEvent -= FinishedRecordEventHandler;
            _speechRecognition.StartedRecordEvent -= StartedRecordEventHandler;
            _speechRecognition.RecordFailedEvent -= RecordFailedEventHandler;
            _speechRecognition.EndTalkigEvent -= EndTalkigEventHandler;
        }

        // start record when user starts to talk 
        public void StartRecordButtonOnClickHandler()
        {
            _speechRecognition.StartRecord(true);

            Debug.Log("start recording");
        }

        // replace this with out of collision, or click of a button, or long pause such as 2-3 seconds of no user input 
        public void StopRecordButtonOnClickHandler()
        {

            _speechRecognition.StopRecord();
        }

        private void StartedRecordEventHandler()
        {
            //_speechRecognitionState.color = Color.red;
            Debug.Log("recording started.");
           
        }

        private void RecordFailedEventHandler()
        {
           Debug.LogWarning("record failed");
        }

        private void EndTalkigEventHandler(AudioClip clip, float[] raw)
        {
            FinishedRecordEventHandler(clip, raw);
        }

        private void FinishedRecordEventHandler(AudioClip clip, float[] raw)
        {
          
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