/* 
 * FileName:    ITreeNode.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  4/9/2015 3:25:00 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Tree
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface of a simply tree structure
    /// </summary>
    /// <typeparam name="T">element type</typeparam>
    public interface ITreeNode<T>
    {
        /// <summary>
        /// Gets or sets the value of this node
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// Gets the parent node of this node.
        /// </summary>
        TreeNode<T> Parent { get; }

        /// <summary>
        /// Gets children node list of this node.
        /// </summary>
        IList<TreeNode<T>> Children { get; }

        /// <summary>
        /// Check if this node has any child.
        /// </summary>
        /// <returns>true if has one or more children, otherwise false.</returns>
        bool HasChildren();

        /// <summary>
        /// Check if the specific node is a child of this node.
        /// </summary>
        /// <param name="child">the node to check</param>
        /// <returns>true if the given node is a child of this node, otherwise false.</returns>
        bool ContainsChild(TreeNode<T> child);

        /// <summary>
        /// Add a given node to this node's children list.
        /// </summary>
        /// <param name="child">the node to add</param>
        void AddChild(TreeNode<T> child);

        /// <summary>
        /// Remove a specific child from this node's children list.
        /// </summary>
        /// <param name="child">the node to remove</param>
        void RemoveChild(TreeNode<T> child);


    }
}
