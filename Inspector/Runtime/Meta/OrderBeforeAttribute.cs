﻿namespace Ayla.Inspector
{
    public class OrderBeforeAttribute : OrderAttribute
    {
        public readonly string memberName;

        public OrderBeforeAttribute(string memberName)
        {
            this.memberName = memberName;
        }
    }
}
