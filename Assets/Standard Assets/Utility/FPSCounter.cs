using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof (Text))]
    public class FPSCounter : MonoBehaviour
    {
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0.0f;
        private int m_CurrentFps;
        const string display = "{0} FPS";
        private Text m_Text;

        //moving avg
        private long m_qty = 0;
        private float m_current_avg_fps = 0.0f;

        private float m_avg_fps = 0.0f;
        private int m_max_fps = int.MinValue; 

        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            m_Text = GetComponent<Text>();
        }


        private void Update()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;

                m_avg_fps = UpdateCumulativeMovingAverageFPS(m_CurrentFps);

                if (m_max_fps < m_CurrentFps)
                    m_max_fps = m_CurrentFps;

                m_Text.text = string.Format(display, m_CurrentFps);
            }
        }

        private void FixedUpdate()
        {

        }

        //https://answers.unity.com/questions/326621/how-to-calculate-an-average-fps.html
        private float UpdateCumulativeMovingAverageFPS(float new_fps)
        {
            ++m_qty;
            m_current_avg_fps += (new_fps - m_current_avg_fps) / m_qty;

            return m_current_avg_fps;
        }

        public void UpdateAverageFramerate()
        {
            GameObject fpscounter = GameObject.FindGameObjectWithTag("EventSystem");
            fpscounter.SendMessage("UpdateAverageFPSCount", m_avg_fps);
            fpscounter.SendMessage("UpdateMaximumFPSCount", m_max_fps);
        }
    }
}
