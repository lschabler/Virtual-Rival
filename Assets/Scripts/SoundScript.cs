using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour {

    public List<AudioSource> CarSounds;

    private float[] MinRpmTable = { 500, 750, 1120, 1669, 2224, 2783, 3335, 3882, 4355, 4833, 5384, 5943, 6436, 6928, 7419, 7900 };
    private float[] NormalRpmTable = { 720, 930, 1559, 2028, 2670, 3145, 3774, 4239, 4721, 5194, 5823, 6313, 6808, 7294, 7788, 8261 };
    private float[] MaxRpmTable = { 920, 1360, 1829, 2474, 2943, 3575, 4036, 4525, 4993, 5625, 6123, 6616, 7088, 7589, 8060, 10000 };
    private float[] PitchingTable = { 0.12f, 0.12f, 0.12f, 0.12f, 0.11f, 0.10f, 0.09f, 0.08f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f };

    private int soundRPM;

    public float RangeDivider = 4f;

    private float maxRPM = 0;

    // Use this for initialization
    void Start () {
		
        for(int i = 1; i<=16; ++i)
        {

            GameObject sound = GameObject.Find(string.Format("CarSound ({0})", i));
            if(sound)
            {
                CarSounds.Add(sound.GetComponent<AudioSource>());
                CarSounds[i - 1].Play();
            }

        }

        maxRPM = MaxRpmTable[CarSounds.Count - 1];

	}
    
    public void SetSoundRPM(float rpm)
    {
        soundRPM = Mathf.RoundToInt(Mathf.Lerp(0, maxRPM, rpm / 1000));
    }

    // Update is called once per frame
    void Update()
    {
        //Set Volume By Rpm's
        for (int i = 0; i < CarSounds.Count; i++)
        {
            if (i == 0)
            {
                //Set Audio1
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio1.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 1)
            {
                //Set Audio2
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio2.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 2)
            {
                //Set Audio3
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i - 1].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio3.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 3)
            {
                //Set Audio4
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio4.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 4)
            {
                //Set Audio5
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio5.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 5)
            {
                //Set Audio6
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio6.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 6)
            {
                //Set Audio7
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio7.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 7)
            {
                //Set Audio8
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio8.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 8)
            {
                //Set Audio9
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio9.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 9)
            {
                //Set Audio10
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio10.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 10)
            {
                //Set Audio11
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ((ReducedRPM / Range) * 2f) - 1f;
                    //Audio11.volume = 0.0f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio11.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 11)
            {
                //Set Audio12
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio12.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 12)
            {
                //Set Audio13
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio13.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 13)
            {
                //Set Audio14
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio14.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 14)
            {
                //Set Audio15
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
                else if (soundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = soundRPM - MaxRpmTable[i];
                    CarSounds[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio15.pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 15)
            {
                //Set Audio16
                if (soundRPM < MinRpmTable[i])
                {
                    CarSounds[i].volume = 0.0f;
                }
                else if (soundRPM >= MinRpmTable[i] && soundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = soundRPM - MinRpmTable[i];
                    CarSounds[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSounds[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (soundRPM >= NormalRpmTable[i] && soundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = soundRPM - NormalRpmTable[i];
                    CarSounds[i].volume = 1f;
                    float PitchMath = (ReducedRPM * (PitchingTable[i] + 0.1f)) / Range;
                    CarSounds[i].pitch = 1f + PitchMath;
                }
            }
        }
    }
}
