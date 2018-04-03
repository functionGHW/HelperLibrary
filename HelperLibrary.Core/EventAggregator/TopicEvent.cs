/* 
 * FileName:    TopicEvent.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/23/2017 11:15:49 PM
 * Description:
 * */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.EventAggregator
{
    public class TopicEvent<TArg> : ITopicEvent
    {
        private readonly HashSet<DelegateItem> subscribers = new HashSet<DelegateItem>();
        private readonly object syncObj = new object();

        public void Publish(TArg arg, bool multithread = false)
        {
            DelegateItem[] ary;
            lock (syncObj)
            {
                ary = subscribers.ToArray();
            }
            object[] parameters = { arg };
            foreach (var item in ary)
            {
                if (item.Instance != null && item.Instance.Target == null)
                {
                    lock (syncObj)
                    {
                        subscribers.Remove(item);
                    }
                }
                else
                {
                    if (multithread)
                    {
                        item.DelegateAction.BeginInvoke(item.Instance?.Target, parameters, null, null);
                    }
                    else
                    {
                        item.DelegateAction.Invoke(item.Instance?.Target, parameters);
                    }
                }
            }
        }

        public ISubscriptionObject Subscribe(Action<TArg> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));


            var item = new DelegateItem()
            {
                Instance = action.Target == null ? null : new WeakReference(action.Target),
                Method = action.Method,
                DelegateAction = CreateAction(action.Method)
            };
            lock (syncObj)
            {
                subscribers.Add(item);
            }
            return new SubscriptionObject(this, action);
        }

        public void Unsubscribe(Action<TArg> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (syncObj)
            {
                var target = action.Target;
                DelegateItem result = null;
                if (target == null)
                {
                    result = subscribers.FirstOrDefault(item => item.Instance == null
                                                                && item.Method == action.Method);
                }
                else
                {
                    result = subscribers.FirstOrDefault(item => item.Instance != null
                                                                && item.Instance.Target == target
                                                                && item.Method == action.Method);
                }
                subscribers.Remove(result);
            }
        }

        private static Action<object, object[]> CreateAction(MethodInfo method)
        {
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            var parameterExpressions = new List<Expression>();
            ParameterInfo[] paramInfos = method.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                var valueObj = Expression.ArrayIndex(
                    parametersParameter, Expression.Constant(i));
                var valueCast = Expression.Convert(
                    valueObj, paramInfos[i].ParameterType);

                parameterExpressions.Add(valueCast);
            }
            Expression instanceCast = method.IsStatic
                ? null
                : Expression.Convert(instanceParameter, method.ReflectedType);

            var methodCall = Expression.Call(instanceCast, method, parameterExpressions);

            var lambda = Expression.Lambda<Action<object, object[]>>(
                methodCall, instanceParameter, parametersParameter);

            Action<object, object[]> execute = lambda.Compile();
            return execute;
        }

        private class DelegateItem
        {
            public WeakReference Instance;

            public MethodInfo Method;

            public Action<object, object[]> DelegateAction;
        }

        private class SubscriptionObject : ISubscriptionObject
        {
            private TopicEvent<TArg> topicEvent;
            private Action<TArg> action;

            public SubscriptionObject(TopicEvent<TArg> topicEvent, Action<TArg> action)
            {
                this.topicEvent = topicEvent;
                this.action = action;
            }

            public void Dispose()
            {
                this.Unsubscribe();
            }

            public void Unsubscribe()
            {
                topicEvent.Unsubscribe(action);
            }
        }

    }
}
