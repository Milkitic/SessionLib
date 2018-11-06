﻿using System;
using System.Linq;

namespace Milkitic.SessionLib
{
    public class NavigatableTree : Tree
    {
        private NavigatableNode _prevNode;
        private NavigatableNode _currentNode;

        public NavigatableTree(NavigatableNode rootNode) : base(rootNode)
        {
            _currentNode = rootNode;
        }

        public NavigatableTree(string rootName, Func<object, ActionParam> actionCallback)
            : this(new NavigatableNode(rootName, actionCallback))
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// 查找指定子节点。
        /// </summary>
        /// <param name="nodeName">节点名</param>
        /// <returns></returns>
        public override Node SearchNode(string nodeName)
        {
            if (Root.Name == nodeName)
                return Root;
            if (Root.Children == null)
                return null;

            var node = _currentNode.Children?.FirstOrDefault(child => child.Name == nodeName);
            if (node != null)
                return node;

            if (_currentNode.Parent != null && _currentNode.Parent.Name == nodeName)
                return _currentNode.Parent;

            return Root.SearchChild(nodeName);
        }

        /// <summary>
        /// 运行程序。
        /// </summary>
        public void Run()
        {
            ActionParam param = new ActionParam(null, null);
            do
            {
                param = CurrentNode.Action.Invoke((param.TargetParam));
                if (param.TargetName != null)
                {
                    _prevNode = CurrentNode;
                    var node = (NavigatableNode)SearchNode(param.TargetName);
                    if (node == null)
                        throw new ArgumentOutOfRangeException(nameof(param.TargetName));
                    _currentNode = node;
                }
                else
                    break;
            } while (true);
        }

        public NavigatableNode PrevNode => _prevNode;

        public NavigatableNode CurrentNode => _currentNode;
    }
}