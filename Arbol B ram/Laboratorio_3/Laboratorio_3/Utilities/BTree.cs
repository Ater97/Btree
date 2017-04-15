using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio_3.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">key</typeparam>
    /// <typeparam name="P">puntero...</typeparam>
    class BTree<T, P> where T : IComparable<T>
    {
        public BNode<T, P> Root { get; private set; }

        public int Degree { get; private set; }

        public int Height { get; private set; }

        public Factory factory = new Factory();
        public BTree(int degree)
        {
            if (degree < 2)
            {
                throw new ArgumentException("BTree degree must be at least 2", "degree");
            }

            factory.createFile(degree);

            Root = new BNode<T, P>(degree);
            Degree = degree;
            Height = 1;
        }

        #region Insert
        public void Insert(T newKey, P newPointer)
        {
            if (!Root.HasReachedMaxEntries)
            {
                InsertNonFull(Root, newKey, newPointer);
                factory.SetHeader(Root.Entries.First().Pointer, newPointer, Root.Entries.Count, Height);
                return;
            }

            BNode<T, P> oldRoot = Root;
            Root = new BNode<T, P>(Degree);
            Root.Children.Add(oldRoot);
            SplitChild(Root, 0, oldRoot);
            InsertNonFull(Root, newKey, newPointer);

            Height++;
        }
        private void SplitChild(BNode<T, P> parentNode, int nodeToBeSplitIndex, BNode<T, P> nodeToBeSplit)
        {
            var newNode = new BNode<T, P>(Degree);

            parentNode.Entries.Insert(nodeToBeSplitIndex, nodeToBeSplit.Entries[Degree - 1]);
            parentNode.Children.Insert(nodeToBeSplitIndex + 1, newNode);

            newNode.Entries.AddRange(nodeToBeSplit.Entries.GetRange(Degree, Degree - 1));

            nodeToBeSplit.Entries.RemoveRange(Degree - 1, Degree);

            if (!nodeToBeSplit.IsLeaf)
            {
                newNode.Children.AddRange(nodeToBeSplit.Children.GetRange(Degree, Degree));
                nodeToBeSplit.Children.RemoveRange(Degree, Degree);
            }
        }

        private void InsertNonFull(BNode<T, P> node, T newKey, P newPointer)
        {
            int positionToInsert = node.Entries.TakeWhile(entry => newKey.CompareTo(entry.Key) >= 0).Count();

            if (node.IsLeaf)
            {
                node.Entries.Insert(positionToInsert, new Entry<T, P>() { Key = newKey, Pointer = newPointer });
                //factory.SetNodes(newPointer, newPointer, node.Children, node.Entries);
                return;
            }

            // non-leaf
            BNode<T, P> child = node.Children[positionToInsert];
            if (child.HasReachedMaxEntries)
            {
                SplitChild(node, positionToInsert, child);
                if (newKey.CompareTo(node.Entries[positionToInsert].Key) > 0)
                {
                    positionToInsert++;
                }
            }

            InsertNonFull(node.Children[positionToInsert], newKey, newPointer);
           
        }
        #endregion
        #region Delete
        public void Delete(T keyToDelete)
        {
            DeleteInternal(Root, keyToDelete);

            if (Root.Entries.Count == 0 && !Root.IsLeaf)
            {
                Root = Root.Children.Single();
                Height--;
            }
        }

        private void DeleteInternal(BNode<T, P> node, T keyToDelete)
        {
            int i = node.Entries.TakeWhile(entry => keyToDelete.CompareTo(entry.Key) > 0).Count();

            if (i < node.Entries.Count && node.Entries[i].Key.CompareTo(keyToDelete) == 0)
            {
                DeleteKeyFromNode(node, keyToDelete, i);
                return;
            }

            if (!node.IsLeaf)
            {
                DeleteKeyFromSubtree(node, keyToDelete, i);
            }
        }
        private void DeleteKeyFromSubtree(BNode<T, P> parentNode, T keyToDelete, int subtreeIndexInNode)
        {
            BNode<T, P> childNode = parentNode.Children[subtreeIndexInNode];

            if (childNode.HasReachedMinEntries)
            {
                int leftIndex = subtreeIndexInNode - 1;
                BNode<T, P> leftSibling = subtreeIndexInNode > 0 ? parentNode.Children[leftIndex] : null;

                int rightIndex = subtreeIndexInNode + 1;
                BNode<T, P> rightSibling = subtreeIndexInNode < parentNode.Children.Count - 1 ?
                parentNode.Children[rightIndex] : null;

                if (leftSibling != null && leftSibling.Entries.Count > Degree - 1)
                {
                    childNode.Entries.Insert(0, parentNode.Entries[subtreeIndexInNode]);
                    parentNode.Entries[subtreeIndexInNode] = leftSibling.Entries.Last();
                    leftSibling.Entries.RemoveAt(leftSibling.Entries.Count - 1);

                    if (!leftSibling.IsLeaf)
                    {
                        childNode.Children.Insert(0, leftSibling.Children.Last());
                        leftSibling.Children.RemoveAt(leftSibling.Children.Count - 1);
                    }
                }
                else if (rightSibling != null && rightSibling.Entries.Count > Degree - 1)
                {

                    childNode.Entries.Add(parentNode.Entries[subtreeIndexInNode]);
                    parentNode.Entries[subtreeIndexInNode] = rightSibling.Entries.First();
                    rightSibling.Entries.RemoveAt(0);

                    if (!rightSibling.IsLeaf)
                    {
                        childNode.Children.Add(rightSibling.Children.First());
                        rightSibling.Children.RemoveAt(0);
                    }
                }
                else
                {

                    if (leftSibling != null)
                    {
                        childNode.Entries.Insert(0, parentNode.Entries[subtreeIndexInNode]);
                        var oldEntries = childNode.Entries;
                        childNode.Entries = leftSibling.Entries;
                        childNode.Entries.AddRange(oldEntries);
                        if (!leftSibling.IsLeaf)
                        {
                            var oldChildren = childNode.Children;
                            childNode.Children = leftSibling.Children;
                            childNode.Children.AddRange(oldChildren);
                        }

                        parentNode.Children.RemoveAt(leftIndex);
                        parentNode.Entries.RemoveAt(subtreeIndexInNode);
                    }
                    else
                    {
                        Debug.Assert(rightSibling != null, "Node should have at least one sibling");
                        childNode.Entries.Add(parentNode.Entries[subtreeIndexInNode]);
                        childNode.Entries.AddRange(rightSibling.Entries);
                        if (!rightSibling.IsLeaf)
                        {
                            childNode.Children.AddRange(rightSibling.Children);
                        }

                        parentNode.Children.RemoveAt(rightIndex);
                        parentNode.Entries.RemoveAt(subtreeIndexInNode);
                    }
                }
            }

            this.DeleteInternal(childNode, keyToDelete);
        }

        private void DeleteKeyFromNode(BNode<T, P> node, T keyToDelete, int keyIndexInNode)
        {
            if (node.IsLeaf)
            {
                node.Entries.RemoveAt(keyIndexInNode);
                return;
            }

            BNode<T, P> predecessorChild = node.Children[keyIndexInNode];
            if (predecessorChild.Entries.Count >= Degree)
            {
                Entry<T, P> predecessor = DeletePredecessor(predecessorChild);
                node.Entries[keyIndexInNode] = predecessor;
            }
            else
            {
                BNode<T, P> successorChild = node.Children[keyIndexInNode + 1];
                if (successorChild.Entries.Count >= Degree)
                {
                    Entry<T, P> successor = DeleteSuccessor(predecessorChild);
                    node.Entries[keyIndexInNode] = successor;
                }
                else
                {
                    predecessorChild.Entries.Add(node.Entries[keyIndexInNode]);
                    predecessorChild.Entries.AddRange(successorChild.Entries);
                    predecessorChild.Children.AddRange(successorChild.Children);

                    node.Entries.RemoveAt(keyIndexInNode);
                    node.Children.RemoveAt(keyIndexInNode + 1);

                    DeleteInternal(predecessorChild, keyToDelete);
                }
            }
        }

        private Entry<T, P> DeletePredecessor(BNode<T, P> node)
        {
            if (node.IsLeaf)
            {
                var result = node.Entries[node.Entries.Count - 1];
                node.Entries.RemoveAt(node.Entries.Count - 1);
                return result;
            }

            return DeletePredecessor(node.Children.Last());
        }

        private Entry<T, P> DeleteSuccessor(BNode<T, P> node)
        {
            if (node.IsLeaf)
            {
                var result = node.Entries[0];
                node.Entries.RemoveAt(0);
                return result;
            }

            return DeletePredecessor(node.Children.First());
        }
        #endregion
        #region Search
        public Entry<T, P> Search(T key)
        {
            return SearchInternal(Root, key);
        }

        private Entry<T, P> SearchInternal(BNode<T, P> node, T key)
        {
            int i = node.Entries.TakeWhile(entry => key.CompareTo(entry.Key) > 0).Count();

            if (i < node.Entries.Count && node.Entries[i].Key.CompareTo(key) == 0)
            {
                return node.Entries[i];
            }

            return node.IsLeaf ? null : SearchInternal(node.Children[i], key);
        }
        #endregion
        
        //public void SaveTree(BTree<T, P> tree, string filename)
        //{
        //    using (Stream file = File.Open(filename, FileMode.Create))
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        bf.Serialize(file, tree.BNodes.Cast<BNode<T, P>>().ToList());
        //    }
        //}

    }
}
