using System;
using System.Threading;


namespace MangaDl
{
    class ThreadWorker
    {
        public event EventHandler ThreadDone;

        private Action<object> m_task;
        private object m_taskParameters;
        public Action<object> Task
        {
            get { return m_task; }
            set { m_task = value; }
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

        public ThreadWorker(Action<object> task, object parameters = null)
        {
            m_task = task;
            m_taskParameters = parameters;
        }

        public ThreadWorker()
        {
        }

        public void Run()
        {
            try
            {
                if (m_task != null)
                {
                    m_task(m_taskParameters);
                }
                if (ThreadDone != null)
                {
                    ThreadDone(this, EventArgs.Empty);
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
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

        public void Join()
        {
            if (m_thread != null)
            {
                m_thread.Join();
            }
        }
    }
}
