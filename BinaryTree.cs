using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private BinaryTree<T> _head;
        private BinaryTree<T> _left;
        private BinaryTree<T> _right;
        private T _value;
        private int _weight;

        private BinaryTree(T key)
        {
            _value = key;
            _weight = 1;
        }

        public BinaryTree()
        {
        }

        public void Add(T key)
        {
            if (_weight == 0)
            {
                _weight++;
                _value = key;
                return;
            }

            _weight++;

            if (key.CompareTo(_value) < 0)
            {
                if (_left == null)
                {
                    _left = new BinaryTree<T>(key)
                    {
                        _head = this
                    };
                    return;
                }

                _left.Add(key);
                return;
            }

            if (_right == null)
            {
                _right = new BinaryTree<T>(key)
                {
                    _head = this
                };
                return;
            }

            _right.Add(key);
        }

        public bool Contains(T key)
        {
            if (_weight == 0)
                return false;

            var stack = new Stack<BinaryTree<T>>();
            stack.Push(this);
            while (stack.Count != 0)
            {
                var current = stack.Pop();
                if (current._value.CompareTo(key) == 0)
                    return true;
                if (current._left != null)
                    stack.Push(current._left);
                if (current._right != null)
                    stack.Push(current._right);
            }

            return false;
        }

        private static int GetIndexByTree(BinaryTree<T> requiredTree)
        {
            // если искомое дерево это корень
            if (requiredTree._head == null)
            {
                return requiredTree._left?._weight ?? 0;
            }

            // если искомое дерево является левым для родительского
            if (requiredTree == requiredTree._head._left)
            {
                return requiredTree._left == null
                    ? GetIndexByTree(requiredTree._head) - requiredTree._weight
                    : GetIndexByTree(requiredTree._head) - requiredTree._weight + requiredTree._left._weight;
            }

            // если искомое дерево является правым для родительского
            return requiredTree._left == null
                ? GetIndexByTree(requiredTree._head) + 1
                : GetIndexByTree(requiredTree._head) + requiredTree._left._weight + 1;
        }

        private BinaryTree<T> GetTreeByIndex(int index)
        {
            var currentTree = this;
            var currentIndex = GetIndexByTree(currentTree);

            while (true)
            {
                if (index == currentIndex)
                    return currentTree;

                if (index < currentIndex)
                {
                    currentTree = currentTree._left;
                    currentIndex = GetIndexByTree(currentTree);
                }

                if (index > currentIndex)
                {
                    currentTree = currentTree._right;
                    currentIndex = GetIndexByTree(currentTree);
                }
            }
        }

        public T this[int index]
        {
            get
            {
                var result = GetTreeByIndex(index);
                return result != null ? result._value : throw new IndexOutOfRangeException();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_weight == 0)
                yield break;

            if (_left != null)
            {
                foreach (var item in _left)
                    yield return item;
            }

            yield return _value;

            if (_right != null)
            {
                foreach (var item in _right)
                    yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}