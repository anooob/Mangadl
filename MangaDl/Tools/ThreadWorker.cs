using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace MangaDl
{
    class ThreadWorker
    {
        public event EventHandler ThreadDone;
        private Action m_task;

        public Action Task
        {
            get { return m_task; }
        }

        private Thread m_thread;

        public bool IsAlive
        {
            get
            {
                if (m_thread != null)
                {
                    return m_thread.IsAlive;
                }
                return false;
            }
        }

        public ThreadWorker(Action task)
        {
            m_task = task;
        }

        public void Run()
        {
            try
            {
                if (m_task != null)
                {
                    m_task();
                }
                if (ThreadDone != null)
                {
                    ThreadDone(this, EventArgs.Empty);
                }
            }
            catch (ThreadAbortException e)
            {
                //ThreadDone(this, EventArgs.Empty);
            }
        }

        public void Start()
        {
            if (m_thread == null)
            {
                m_thread = new Thread(Run);
                m_thread.IsBackground = true;
                m_thread.Start();
            }
        }

        public void Abort()
        {
            if (m_thread != null)
            {
                m_thread.Abort();
            }
        }
    }
}
