
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

class PubSub
{
    private readonly Dictionary<string, List<Handler>> Hub = new Dictionary<string, List<Handler>>();
    private static readonly Lazy<PubSub> lazy = new Lazy<PubSub>(() => new PubSub());
    public static PubSub Instance => lazy.Value;
    public delegate void Handler(object o);

    private PubSub() { }

    public void Subscribe(string topic, Handler handler)
    {
        if (!this.Hub.ContainsKey(topic))
            this.Hub.Add(topic, new List<Handler>());

        this.Hub[topic].Add(handler);
    }

    public void Unsubscribe(string topic, Handler handler)
    {
        if (!this.Hub.ContainsKey(topic))
            return;

        _ = this.Hub[topic].Remove(handler);
    }

    public void Publish(string topic, object o = null)
    {
        if (!this.Hub.ContainsKey(topic))
            return;

        this.Hub[topic].ForEach((Handler handler) => handler.Invoke(o));
    }

}
