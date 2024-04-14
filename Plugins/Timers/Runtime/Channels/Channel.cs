using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ayla.Timers.Runtime.Channels
{
    public class Channel : ScriptableObject
    {
        [SerializeField, Range(0, 10.0f)]
        private double m_Scale = 1.0;

        [SerializeField]
        private List<Channel> m_Children = new();

        [SerializeField]
        private Channel m_Parent;

        private double m_InvScale = 1.0;

        private double m_Time;
        private double m_DeltaTime;

        public Channel parent => m_Parent;

        public Channel[] children => m_Children.ToArray();

        public double scaleSelf
        {
            get => m_Scale;
            set
            {
                m_Scale = value;
                m_InvScale = 1.0 / value;
            }
        }

        public double invScaleSelf => m_InvScale;

        public double scale => (m_Parent?.scale ?? 1.0) * m_Scale;

        public double invScale => (m_Parent?.invScale ?? 1.0) * m_InvScale;

        public double time => m_Time;

        public double deltaTime => m_DeltaTime;

        internal void SetParent(Channel parent)
        {
            if (parent == this)
            {
                throw new ArgumentException("Same channel is not allowed.");
            }

            if (m_Parent == parent)
            {
                return;
            }

            Channel parentCheck = parent;
            while (parentCheck != null)
            {
                if (parentCheck == this)
                {
                    throw new ArgumentException("Same channel or circular chain detected.");
                }

                parentCheck = parentCheck.m_Parent;
            }

            if (m_Parent != null)
            {
                m_Parent.m_Children.Remove(this);
            }

            if (parent != null)
            {
                m_Parent = parent;
                parent.m_Children.Add(this);
            }
        }

        internal void UpdateTimer(double deltaTime)
        {
            m_DeltaTime = deltaTime * scale;
            m_Time += m_DeltaTime;

            // propagate to children
            foreach (var child in m_Children)
            {
                child.UpdateTimer(deltaTime);
            }
        }
    }
}