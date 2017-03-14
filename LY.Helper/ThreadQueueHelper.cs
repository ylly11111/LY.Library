using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LY.Helper
{
    /// <summary>
    /// 线程队列帮助类
    /// </summary>
    public class ThreadQueueHelper<T>
    {
        public delegate void  CallHandler(T para);
        public  CallHandler Execute;
        private Queue<T> _queue;

        public ThreadQueueHelper()
        {
            _queue = new Queue<T>();

            ThreadPool.QueueUserWorkItem(new WaitCallback(ExecuteQueue));
        }

        public void AddQueue(T item)
        {
            _queue.Enqueue(item);
        }

        public void ClearQueue()
        {
            _queue.Clear();
        }

        /// <summary>
        /// 执行线程队列
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteQueue(object obj)
        {
            while (true)
            {
                if (_queue.Count > 0)
                {
                    
                   T item= _queue.Dequeue();
                   Execute(item);
                }

                System.Threading.Thread.Sleep(1);
            }
        }
    }

    class ThreadQueueTest
    {
        private void TestQueue()
        {
            ThreadQueueHelper<int> QueueOperate = new ThreadQueueHelper<int>();
            QueueOperate.Execute += new ThreadQueueHelper<int>.CallHandler(InsertData);

            for (int i = 0; i < 100; i++)
            {
                QueueOperate.AddQueue(i);
            }
        }

        private object obj = new object();
        private void  InsertData(int i)
        {
            lock (obj)
            {
                //insert into db operate
            }
        }
    }
}
