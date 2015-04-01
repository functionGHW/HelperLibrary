/* 
 * FileName:    TreeNode.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/27/2015 9:44:25 AM
 * Version:     v1.1
 * Description:
 * */

namespace HelperLibrary.Core.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HelperLibrary.Core.ExtensionHelper;
    using System.Diagnostics.Contracts;
    using System.Collections.Concurrent;

    /// <summary>
    /// a simply implement of data structure Tree.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNode<T>
    {
        #region Static Members

        /// <summary>
        /// Get root of tree by any node of the tree.
        /// </summary>
        /// <param name="node">one node of the tree</param>
        /// <returns>the root of the tree; if node is a single node tree, return the node</returns>
        public static TreeNode<T> FindRootNode(TreeNode<T> node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            //var root = node;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }

        /// <summary>
        /// a helper method for checking if the tree is build right.
        /// </summary>
        /// <param name="root">root of tree</param>
        /// <returns></returns>
        public static bool CheckTree(TreeNode<T> root)
        {
            if (root == null)
                throw new ArgumentNullException("root");

            List<TreeNode<T>> nodeList = new List<TreeNode<T>>();
            nodeList.Add(root);
            for (int i = 0; i < nodeList.Count; i++)
            {
                TreeNode<T> node = nodeList[i];
                foreach (var child in node.Children)
                {
                    if (nodeList.Contains(child))
                    {
                        return false;
                    }
                    nodeList.Add(child);
                }
            }
            return true;
        }

        // depth-first traversal helper method
        private static void DepthFirstTraversalInternal(TreeNode<T> root, Action<TreeNode<T>> action)
        {
            Contract.Assert(root != null && action != null);
            
            /* In this implement, we use preorder traversal, however, 
             * your program should not depend on the order of which node be visited first.
             */
            action.Invoke(root);
            foreach (var node in root.Children)
            {
                DepthFirstTraversalInternal(node, action);
            }

        }


        #endregion

        #region Fields

        private IList<TreeNode<T>> children = new List<TreeNode<T>>();

        // sync object for children list.
        private readonly object childrenSyncObj = new object();

        private TreeNode<T> parent = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize instance of TreeNode
        /// </summary>
        public TreeNode(T value)
        {
            this.Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all children as a list, if no child, return an empty list.
        /// </summary>
        /// <remarks>modify the list you got will not change the real children.</remarks>
        public IList<TreeNode<T>> Children
        {
            get
            {
                Contract.Assert(this.children != null);
                lock (this.childrenSyncObj)
                {
                    return this.children.ToList();
                }
            }
        }

        /// <summary>
        /// Gets the parent node of this node.
        /// </summary>
        public TreeNode<T> Parent
        {
            get { return this.parent; }
        }

        /// <summary>
        /// Gets or sets value of this node
        /// </summary>
        public T Value
        {
            get;
            set;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// check if this node has children.
        /// </summary>
        /// <returns>true if has one child or more, false if has no child.</returns>
        public bool HasChildren()
        {
            Contract.Assert(this.children != null);
            return (this.children.Count > 0);
        }

        /// <summary>
        /// check if the specific node is child of the tree
        /// </summary>
        /// <param name="child">the node to check</param>
        /// <returns>true if the node is child of the tree, otherwise false.</returns>
        public bool ContainsChild(TreeNode<T> child)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            Contract.Assert(this.children != null);

            bool result = child.Parent == this;
            return result;
        }

        /// <summary>
        /// add the node to the children list.
        /// </summary>
        /// <param name="child">the node to add</param>
        public void AddChild(TreeNode<T> child)
        {
            Contract.Assert(this.children != null);
            if (child == null)
                throw new ArgumentNullException("child");

            if (!this.children.Contains(child))
            {
                lock (childrenSyncObj)
                {
                    if (!this.children.Contains(child))
                    {
                        if (child.parent != null)
                            throw new InvalidOperationException("one node cannot be added to two trees.");

                        this.children.Add(child);
                        child.parent = this;
                    }
                }
            }
        }

        /// <summary>
        /// Remove a child from the children list.
        /// </summary>
        /// <param name="child">the child to remove</param>
        public void RemoveChild(TreeNode<T> child)
        {
            Contract.Assert(this.children != null);
            if (child == null)
                throw new ArgumentNullException("child");

            if (this.children.Contains(child))
            {
                lock (this.childrenSyncObj)
                {
                    if (this.children.Contains(child))
                    {
                        child.parent = null;
                        this.children.Remove(child);

                    }
                }
            }
        }

        /// <summary>
        /// Depth-First Traversal.
        /// </summary>
        /// <param name="action">action to do with each node.</param>
        /// <remarks>Note that your program should not depend on the order of which node be visit first.</remarks>
        public void DepthFirstTraversal(Action<TreeNode<T>> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            DepthFirstTraversalInternal(this, action);
        }

        /// <summary>
        /// Breadth-First Traversal
        /// </summary>
        /// <param name="action">action to do with each node.</param>
        public void BreadthFirstTraversal(Action<TreeNode<T>> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var list = new List<TreeNode<T>>() { this };

            for (int i = 0; i < list.Count; i++)
            {
                var node = list[i];
                list.AddRange(node.Children);

                action.Invoke(node);
            }
        }

        #endregion

        /// <summary>
        /// return a string format of the Value in the node.
        /// This simply call ToString method of the Value.
        /// </summary>
        /// <returns>a string, if the Value is null, return an empty string.</returns>
        public override string ToString()
        {
            return Value == null ? "" : Value.ToString();
        }
    }
}
