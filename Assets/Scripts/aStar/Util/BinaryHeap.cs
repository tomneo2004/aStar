using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NP.DataStructure{

	/**
	 * Binary Heap aka priority queue
	 * 
	 * Store element in a sorted array in either
	 * ascending or decending order in low overhead
	 * performance
	 * 
	 **/
	public class BinaryHeap<T> {

		/**
		 * Heap which contain all elements
		 **/
		T[] _heap;

		/**
		 * Return a copy of heap
		 **/
		public T[] Heap{get{ return (T[])_heap.Clone();}}

		/**
		 * Get first element in heap
		 **/
		public T Priority{get{ return GetElement ();}}

		/**
		 * The length of heap
		 * 
		 * Return how many elements in heap
		 **/
		public int Count{get{ return _heap == null ? 0 : _heap.Length;}}

		/**
		 * Swap delegate method
		 * 
		 * Swap delegate method give parent and child as parameters
		 * 
		 * In a Min-Heap return true in Swap method implementation if parent greater than or
		 * equal to child. As Min-Heap parent have always been smaller than child.
		 * e.g parent >= child
		 * 
		 * In a Max-Heap return false in Swap method implementation if child greater than or
		 * equal to parent. As Max-Heap parent have always been bigger than child.
		 * e.g parent <= child
		 **/
		public delegate bool Swap(T parent, T child);

		/**
		 * Swap delegate method instance
		 **/
		Swap _swap;



		/**
		 * Swap delegate method must be provided.
		 * 
		 * Swap delegate method give parent and child as parameters
		 * 
		 * In a Min-Heap return true in Swap method implementation if parent greater than or
		 * equal to child. As Min-Heap parent have always been smaller than child.
		 * e.g parent >= child
		 * 
		 * In a Max-Heap return false in Swap method implementation if child greater than or
		 * equal to parent. As Max-Heap parent have always been bigger than child.
		 * e.g parent <= child
		 **/
		public BinaryHeap(Swap swapCheck){

			_heap = new T[0];

			if (swapCheck == null)
				Debug.LogAssertion ("BinaryHeap must have comparsion method");
			_swap = swapCheck;
		}

		/**
		 * Return first element in heap
		 **/
		T GetElement(){
		
			if (_heap == null)
				return default(T);

			if (_heap.Length <= 0)
				return default(T);

			return _heap [0];
		}

		/**
		 * Clear heap
		 **/
		public void Clear(){

			if (_heap != null && _heap.Length > 0)
				_heap = new T[0];
		}

		/**
		 * Add element into heap
		 **/
		public void Add(T element){

			int totalElements = _heap.Length + 1;

			//add element to heap
			T[] tempHeap = new T[totalElements];
			_heap.CopyTo (tempHeap, 0);
			tempHeap [totalElements - 1] = element;
			_heap = tempHeap;

			//The pointer which point to the last element in a set of
			//elements of binary tree that should be checking
			int b_pointerIndex = totalElements;

			//Binary heap start index from 1 not 0
			//Sort heap from end to being in array
			//Use -1 to get actual index in array
			//Each loop we move pointer backward
			while (b_pointerIndex != 1) {

				//Each element has 2 childs both location are
				//element's location * 2
				//element's location * 2 + 1
				//We find parent index of 2 child
				int b_parentIndex = b_pointerIndex / 2;

				//should element swap
				if (_swap (_heap [b_parentIndex - 1], _heap [b_pointerIndex - 1])) {

					T tempElement = _heap [b_pointerIndex - 1];
					_heap [b_pointerIndex - 1] = _heap [b_parentIndex - 1];
					_heap [b_parentIndex - 1] = tempElement;

					//move pointer forward to upper layer of binary tree
					b_pointerIndex = b_parentIndex;

				} else {
				
					break;
				}
			}
		}

		/**
		 * Remove first element from heap
		 **/
		public T Remove(){
		
			if (_heap == null || _heap.Length <= 0)
				return default(T);

			T removedElement = _heap [0];

			int totalElements = _heap.Length - 1;

			T[] tempHeap = new T[totalElements];

			//replace first element in heap with last
			_heap [0] = _heap [_heap.Length - 1];

			//copy heap to new heap
			for (int i = 0; i < tempHeap.Length; i++)
				tempHeap [i] = _heap [i];

			_heap = tempHeap;

			//If parent index and swap index are not the same
			//we need to swap element otherwise we don't
			//Parent inde point to each binary tree it need to be check
			//Swap index point to element which need to swap with parent
			//in binary tree
			int b_swapIndex = 1;
			int b_parentIndex = 1;

			//Sort heap from begin to end in array
			//Use -1 to get actual index in array
			//Each loop we move parent index forward
			do {

				//move parent index forward to swap index
				b_parentIndex = b_swapIndex;

				if ((b_parentIndex * 2 + 1) <= totalElements) {//both childs exist

					//check second child need to swap with parent
					if (_swap (_heap [b_parentIndex - 1], _heap [b_parentIndex * 2 + 1 - 1]))
						b_swapIndex = b_parentIndex * 2 + 1;

					//check first child need to swap with parent
					if (_swap (_heap [b_swapIndex - 1], _heap [b_parentIndex * 2 - 1]))
						b_swapIndex = b_parentIndex * 2;
					
				} else if ((b_parentIndex * 2) <= totalElements) {//only one child exist

					//check first child need to swap with parent
					if (_swap (_heap [b_parentIndex - 1], _heap [b_parentIndex * 2 - 1]))
						b_swapIndex = b_parentIndex * 2;
				}

				//if parent index and swap index are not the same
				//we swap
				if(b_parentIndex != b_swapIndex){

					T tempElement = _heap[b_swapIndex - 1];
					_heap[b_swapIndex - 1] = _heap[b_parentIndex - 1];
					_heap[b_parentIndex - 1] = tempElement;
				}

			} while(b_swapIndex != b_parentIndex);

			return removedElement;
		}

		/**
		 * Define equal delegate method
		 * 
		 * First: the element which is comparing to
		 * 
		 * Second: the element which will be compared
		 **/
		public delegate bool Equal(T first, T second);

		/**
		 * Check if element is exist in heap
		 * 
		 * First: the element which is comparing to
		 * 
		 * Second: the element which will be compared
		 **/
		public bool Contain(T element, Equal equal){
		
			if (equal == null) {
				Debug.LogAssertion ("Binary Heap must have equal delegate method");
				return false;
			}

			if (element == null)
				return false;

			IEnumerator ie = _heap.GetEnumerator ();
			while (ie.MoveNext ()) {

				if (equal (element, (T)ie.Current))
					return true;
			}

			return false;
		}
	}
}

