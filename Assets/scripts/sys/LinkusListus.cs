using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.XPath;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace ADT
{
    public class LinkusListus<T> : IDynamicList<T>
    {
        private class ListNode
        {
            public T nodeItem;
            public ListNode nextNode;

            public ListNode(T item, ListNode nextNode)
            {
                nodeItem = item;
                this.nextNode = nextNode;
            }
        }

        private ListNode head;
        private ListNode tail;
        private int count;

        public LinkusListus()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public int Count => count;
        public T Tail => tail.nodeItem;

        public int IndexOf(T item)
        {
            ListNode currentNode = head;
            int idx = 0;

            while (currentNode != null)
            {
                if (currentNode.nodeItem.Equals(item))
                {
                    return idx;
                }

                idx++;
                currentNode = currentNode.nextNode;
            }

            return -1;
        }

        public bool Contains(T item)
        {
            ListNode currentNode = head;

            while (currentNode != null)
            {
                if (currentNode.nodeItem.Equals(item))
                {
                    return true;
                }

                currentNode = currentNode.nextNode;
            }

            return false;
        }

        public void Add(T item)
        {
            ListNode newNode = new ListNode(item, null);

            if (count == 0)
            {
                head = newNode;
                tail = newNode;
            } else
            {
                tail.nextNode = newNode;
                tail = newNode;
            }

            count++;
        }

        public void Insert(int index, T item)
        {
            if (index > count || index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            ListNode currentNode = head;
            ListNode previousNode = head;

            if (index == 0)
            {
                ListNode newHead = new ListNode(item, currentNode);
                head = newHead;
                count++;
                return;
            }

            if (index == count)
            {
                ListNode newTail = new ListNode(item, null);
                tail.nextNode = newTail;
                tail = newTail;
                count++;
            }

            for (int curIndex = 0; curIndex < index; curIndex++)
            {
                previousNode = currentNode;
                currentNode = currentNode.nextNode;

                curIndex++;
            }

            ListNode newNode = new ListNode(item, currentNode);
            previousNode.nextNode = newNode;
            count++;
        }

        public void RemoveAt(int index)
        {
            if (index >= count || index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            ListNode currentNode = head;
            ListNode previousNode = head;

            if (index == 0)
            {
                head = head.nextNode;
                count--;
                return;
            }

            for (int curIndex = 0; curIndex < index; curIndex++)
            {
                previousNode = currentNode;
                currentNode = currentNode.nextNode;

                curIndex++;
            }

            previousNode.nextNode = currentNode.nextNode;
            count--;
        }

        public bool Remove(T item)
        {
            ListNode currentNode = head;
            ListNode previousNode = head;

            while (currentNode != null)
            {
                if (currentNode.nodeItem.Equals(item))
                {
                    previousNode.nextNode = currentNode.nextNode;
                    count--;
                    return true;
                }

                previousNode = currentNode;
                currentNode = currentNode.nextNode;
            }

            return false;
        }

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void CopyTo(T[] target, int index)
        {
            ListNode currentNode = head;
            int curIndex = 0;

            if (target == null)
            {
                throw new ArgumentNullException();
            }

            if (count == 0)
            {
                return;
            }

            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (target.Length - index < count)
            {
                throw new ArgumentException("destination array too small");
            }

            while (curIndex < index || currentNode != null)
            {
                target[curIndex] = currentNode.nodeItem;
                curIndex++;
                currentNode = currentNode.nextNode;
            }
        }

        public T this[int index]
        {
            get
            {
                ListNode currentNode = head;
                int curIndex = 0;

                while (curIndex < index)
                {
                    currentNode = currentNode.nextNode;
                    curIndex++;
                }

                return currentNode.nodeItem;
            }
            set
            {
                ListNode currentNode = head;
                int curIndex = 0;

                while (curIndex < index)
                {
                    currentNode = currentNode.nextNode;
                    curIndex++;
                }

                currentNode.nodeItem = value;
            }
        }
    }
}