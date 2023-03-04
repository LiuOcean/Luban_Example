using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
    public class UniTaskCancellationTokenSource
    {
        private HashSet<Action> actions = new HashSet<Action>();

        private CancellationTokenSource cts;

        /// <summary>
        /// 只有当需要和外部绑定时, 才会 new 一个新的 cts
        /// </summary>
        public CancellationToken Token
        {
            get
            {
                if(cts != null)
                {
                    return cts.Token;
                }

                cts = new CancellationTokenSource();

                return cts.Token;
            }
        }

        public void Add(Action callback)
        {
            // 如果action是null，绝对不能添加,要抛异常，说明有协程泄漏
            actions.Add(callback);
        }

        public void Remove(Action callback) { actions?.Remove(callback); }

        public bool IsCancel() { return actions == null; }

        public void Cancel()
        {
            if(actions == null)
            {
                return;
            }

            cts?.Cancel();

            Invoke();
        }

        private void Invoke()
        {
            HashSet<Action> runActions = actions;
            actions = null;
            try
            {
                foreach(Action action in runActions)
                {
                    action.Invoke();
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}