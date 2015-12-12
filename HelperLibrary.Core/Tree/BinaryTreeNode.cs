/* 
 * FileName:    BinaryTreeNode.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/30/2015 3:52:56 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Tree
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// a simply data structure of binary tree node.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryTreeNode<T>
    {
        #region Static members

        // preorder traversal helper method.
        private static void PreorderTraversalInternal(BinaryTreeNode<T> root,
            Action<BinaryTreeNode<T>> action)
        {
            Contract.Assert(action != null);

            if (root == null)
                return;

            action(root);
            PreorderTraversalInternal(root.Left, action);
            PreorderTraversalInternal(root.Right, action);
        }

        // inorder traversal helper method.
        private static void InorderTraversalInternal(BinaryTreeNode<T> root,
            Action<BinaryTreeNode<T>> action)
        {
            Contract.Assert(action != null);

            if (root == null)
                return;

            PreorderTraversalInternal(root.Left, action);
            action(root);
            PreorderTraversalInternal(root.Right, action);
        }

        // postorder traversal helper method.
        private static void PostorderTraversalInternal(BinaryTreeNode<T> root,
            Action<BinaryTreeNode<T>> action)
        {
            Contract.Assert(action != null);

            if (root == null)
                return;

            PreorderTraversalInternal(root.Left, action);
            PreorderTraversalInternal(root.Right, action);
            action(root);
        }

        #endregion

        #region Fields

        private BinaryTreeNode<T> left;
        private BinaryTreeNode<T> right;

        #endregion

        /// <summary>
        /// Initialize a BinaryTreeNode instance with the value.
        /// </summary>
        /// <param name="value">value of the node</param>
        public BinaryTreeNode(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the left node.
        /// </summary>
        public BinaryTreeNode<T> Left
        {
            get { return this.left; }
            set
            {
                if (value != null)
                {
                    if (this.left != null)
                    {
                        this.RemoveChild(true);
                    }
                    this.AddChild(value, true);
                }
                else
                {
                    if (this.left != null)
                    {
                        this.RemoveChild(true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the right node;
        /// </summary>
        public BinaryTreeNode<T> Right
        {
            get { return this.right; }
            set
            {
                if (value != null)
                {
                    if (this.right != null)
                    {
                        this.RemoveChild(false);
                    }
                    this.AddChild(value, false);
                }
                else
                {
                    if (this.right != null)
                    {
                        this.RemoveChild(false);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the parent node;
        /// </summary>
        public BinaryTreeNode<T> Parent { get; private set; }

        /// <summary>
        /// Gets or sets the value of the node
        /// </summary>
        public T Value { get; set; }

        #region Methods

        private void AddChild(BinaryTreeNode<T> node, bool isLeft)
        {
            Contract.Assert(node != null);
            Contract.Assert(isLeft ? (this.left == null) : (this.right == null));

            if (isLeft)
            {
                this.left = node;
            }
            else
            {
                this.right = node;
            }
            node.Parent = this;
        }

        private void RemoveChild(bool isLeft)
        {
            Contract.Assert(isLeft ? (this.left != null) : (this.right != null));

            if (isLeft)
            {
                this.left.Parent = null;
                this.left = null;
            }
            else
            {
                this.right.Parent = null;
                this.right = null;
            }
        }

        /// <summary>
        /// Simply call Value's ToString method, or return an empty string if Value is null.
        /// </summary>
        /// <returns>a string</returns>
        public override string ToString()
        {
            return this.Value == null ? "" : this.Value.ToString();
        }

        /// <summary>
        /// Preorder traverse the tree
        /// </summary>
        /// <param name="action">action to do with each node</param>
        public void PreorderTraversal(Action<BinaryTreeNode<T>> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            PreorderTraversalInternal(this, action);
        }

        /// <summary>
        /// Inorder traverse the tree
        /// </summary>
        /// <param name="action">action to do with each node</param>
        public void InorderTraversal(Action<BinaryTreeNode<T>> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            InorderTraversalInternal(this, action);
        }

        /// <summary>
        /// Postorder traverse the tree
        /// </summary>
        /// <param name="action">action to do with each node</param>
        public void PostorderTraversal(Action<BinaryTreeNode<T>> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            PostorderTraversalInternal(this, action);
        }

        #endregion
    }
}
