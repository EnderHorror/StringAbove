/* MicrophonePitchDetector.cs
*
* Copyright 2017 Outloud Oy
* Written by: mikko.k.koivisto@outloud.fi
* 
* Analyzes pitch of Microphone input. After analysis, onPitchDetected event is invoked.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using MusicXml.Domain;
using UnityEngine;
using UnityEngine.Events;
using PitchDetector;

namespace PitchDetector {


    [System.Serializable]
    public class PitchEvent : UnityEvent<List<float>, int, float> {
    }

    public class MicrophonePitchDetector : MonoBehaviour {

        public PitchEvent onPitchDetected;
        public int micSampleRate = 16000;
        public float minVlolum = 0.1f;
        [Range(1,12)]
        public int boundCount = 6;
        private RAPTPitchDetector pitchDetector;
        private bool recording;

        public JudgeZone judgeZone;

        public static MicrophonePitchDetector Instance { get; private set; }

        public bool Record {
            get {
                return Microphone.IsRecording(null);
            }
            set {
                if (value && !Microphone.IsRecording(null)) {
                    StartMic();
                } else if (!value && Microphone.IsRecording(null)) {
                    StopMic();
                }
            }

        }

        private RAPTPitchDetector Detector {
            get {
                if (pitchDetector == null) {
                    pitchDetector = new RAPTPitchDetector((float)micSampleRate, 50f, 800f);
                }
                return pitchDetector;
            }
        }

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            onPitchDetected.AddListener(LogPitch);
            onPitchDetected.AddListener(JudeNote);
        }
        void LogPitch(List<float> list,int a,float b)
        {
            int midiNote = Fliter(list);
            if(midiNote !=0)
                print(RAPTPitchDetectorExtensions.MidiToNote(midiNote));
        }
        /// <summary>
        /// 判断音符
        /// </summary>
        void JudeNote(List<float> list, int a, float b)
        {
            int midiNote = Fliter(list);
            if (midiNote != 0)
            {
                Pitch pitch = RAPTPitchDetectorExtensions.MidiToNote(midiNote);
                    judgeZone.Judge(pitch);
            }
        }

        /// <summary>
        /// 过滤增加准确性,满足固定重复次数才判断成功
        /// </summary>
        /// <param name="list"></param>
        /// <returns>midi音号</returns>
        int Fliter(List<float> list)
        {
            var midis = RAPTPitchDetectorExtensions.HerzToMidi(list);
            Dictionary<int,int> notes = new Dictionary<int, int>();
            foreach (var item in midis)
            {
                if(item>0)
                    if(!notes.ContainsKey(item)) notes.Add(item,1);
                    else
                    {
                        notes[item] += 1;
                    }
            }

            foreach (var note in notes)
            {
                if (note.Value > 6)
                    return note.Key;
            }

            return 0;
        }
        public void ToggleRecord() {
            Record = !Record;
        }

        private void StartMic() {
            StartCoroutine(RecordingCoroutine());
        }


        private void StopMic() {
            recording = false;
        }


        private IEnumerator RecordingCoroutine() {
            recording = true;
            AudioClip rec = Microphone.Start(Microphone.devices[0], true, 1, micSampleRate);
            float[] readBuffer = new float[rec.samples];
            int recPos = 0;
            int prevRecPos = 0;
            Func<bool> enoughSamples = () => { 
                int count = (readBuffer.Length + Microphone.GetPosition(null) - prevRecPos) % readBuffer.Length;
                return count > Detector.RequiredSampleCount((float)micSampleRate);
            };
            while (recording) {
                prevRecPos = recPos;
                yield return new WaitUntil(enoughSamples);
                recPos = Microphone.GetPosition(null);
                rec.GetData(readBuffer, 0);
                for (int i = 0; i < readBuffer.Length; i++)
                {
                    if (readBuffer[i] < minVlolum) readBuffer[i] = 0;
                }
                int sampleCount = (readBuffer.Length + recPos - prevRecPos) % readBuffer.Length;
                float db = 0f;
                List<float> pitchValues = Detector.getPitch(readBuffer, prevRecPos, ref recPos, ref db, (float)micSampleRate, true, !recording);
                sampleCount = (readBuffer.Length + recPos - prevRecPos) % readBuffer.Length;
                if (sampleCount > 0) {
                    onPitchDetected.Invoke(pitchValues, sampleCount, db);
                }
            }
            Microphone.End(null);
        }

    }
}