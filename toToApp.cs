//PEDRO JONAS LANDEROS CORTES
//INGENIERO EN SISTEMAS COMPUTACIONALES
//GITHUB: pet3r20
//To do app with structures 

/*
insert new task
delete task
modify
order by priority
show tasks

*/
using System;
using System.Collections.Generic;

namespace ToDoList
{
    //main menu
    public class Node
    {
        public string request = "";
        public int priority;
        public Node? next = null;
        public Node? prev = null;
    }
    
    //we can't directly create a max heap, so is necesary to create a new clase that allow us to sort by max
    public class MaxPriorityComparer : IComparer<int> //we are compargin two ints
    {
        //method to return inverted values
        public int Compare(int x, int y)
        {
            return y.CompareTo(x);
        }
    }

    public class ToDoListApp
    {
        //heap that contains nodes, it is ordered by the priority variable
        public static PriorityQueue<Node, int> heap = new PriorityQueue<Node, int>(new MaxPriorityComparer());
        public Node? firstNode; //this node gives us the oportunity to know if our list is empty or not
        public Node? LastCreatedNode; //this node helps us to know the position of the last created node, so we don't need to check the entire list in order to know the new task position
        
        //constructor method 
        public ToDoListApp()
        {
            firstNode = null;
            LastCreatedNode = null;
        }
        
        public void NewTask(string task, int priority)
        {
            Node? newNode = new Node();

            //set the protiry value
            newNode.priority = priority;
            heap.Enqueue(newNode, newNode.priority);

            //this is the follow instructions for the first time we adding a pending
            if (firstNode == null)
            {
                firstNode = newNode;
                //newNode.request = task;
                newNode.prev = null;
                //newNode.next = null;
                //LastCreatedNode = newNode;   
            }
            else //in this case we have at least one element in our list
            {
                //example with only 1 value in the list
                //1 and want to insert 2
                //the lastCreatedNode is 1, and it also is the first node (which is not going to change and does not interest us cause we are not using it, the porpouse of that node is to help us to know if the list is empty or not)
                //newNode prev is 1, so newNode prev is lastCreatedNode
                //newNode next is always null
                //lastCreatedNode next is newNode/
                //and the lastCreatedNode now also is newNode

                //example with at least 2 elements in the list
                //1,2,3 and we want to insert 4
                //the lastCreated node is 3, and the newNode is 4
                //continuing with the logic of the last example, newNode next is always null and we can also see that prev is also lastCreatedNode, and at the end the newNode always is gonna be the lastCreatedNode,so the way we managed the creation of newNodes is correct

                newNode.prev = LastCreatedNode;
                //newNode.next = null;
                //newNode.request = task;
                LastCreatedNode.next = newNode;
                //LastCreatedNode = newNode;
            }

            newNode.request = task;
            newNode.next = null;
            LastCreatedNode = newNode;
        }

        public void DeleteTask(string task)
        {
            //check if the first node is not empty (the list has at least 1 task)
            if (firstNode == null)
            {
                Console.WriteLine("Escribe al menos 1 tarea!");
                return;
            }
            
            Node NodeToDelete = firstNode;

            while (NodeToDelete.request != task)
                NodeToDelete = NodeToDelete.next; //we are searching for the node with the task to delete

            //when the temporalNode is null we notice that is at the end of the list, so the task is not in the list
            if (NodeToDelete != null) 
            {
                //we create a heap copy to save the original heap and delete elements without losing information
                var heapCopy = new PriorityQueue<Node, int>(new MaxPriorityComparer());


                //example: we only have 1 element: 5 -> temporalNode is pointing to 5 (since is the unique element)
                //if we just say that NodeToDelete.next = null the code will break since NodeToDelete.next is null (the value of the next node) so we are not allowed to do that
                if (NodeToDelete == firstNode)
                {
                    firstNode = NodeToDelete.next;
                    if (firstNode != null) //the first element is not the only in the list
                        firstNode.prev = null;   
                }
                    
                else if (NodeToDelete == LastCreatedNode)//example: 1, 2, 3 ,4  and the element we wanna eliminate is 4 -> temporalNode is pointig to 4
                {
                    NodeToDelete.prev.next = null; 
                    //the task to delete is the last createdNode, but in our heap the order is different, so we need to erase all the elements before our task so we can also errase it.
                }
                else //example: 1, 2, 3, 4 and the element we wanna eliminate is 2  -> temporalNode is pointing to 2
                {
                    NodeToDelete.next.prev = NodeToDelete.prev;
                    NodeToDelete.prev.next = NodeToDelete.next;
                    NodeToDelete = null;
                }
                DeleteIntPriority(task);
                NodeToDelete = null;
                Console.WriteLine("Tarea finalizada, felicidades!");
            }
            else
            {
                Console.WriteLine("Tarea no encontrada");
            }

        }

        public void DeleteIntPriority(string task)
        {
            var heapCopy = new PriorityQueue<Node, int>(new MaxPriorityComparer()); //a copy to manipulate the heap 

            while (heap.Count > 0) //if our heap is not empty we still removing elements
            {
                var actualNode = heap.Dequeue();
                if (actualNode.request != task)
                    heapCopy.Enqueue(actualNode, actualNode.priority); //we add elements in the heap copy to replace the main heap without including the task to delete
            }
            heap = heapCopy; //the main heap is now pointing at the space memory where the heap copy is, so now the main heap doesn't has the task deleted
        }

        //we wanna show all tasks ordering by priority (which is gonna be indicated by the user and it was dropped in the variable priority)
        public void OrderByPriority()
        {
            //creating a new heap to be the main heap copy
            var heapCopy = new PriorityQueue<Node, int>(new MaxPriorityComparer());

            //showing the list sorted by priority and adding elements into the "copy", so we can still having the original list (all is lg or constant)
            while(heap.Count > 0)
            {
                var actualNode = heap.Dequeue();
                heapCopy.Enqueue(actualNode, actualNode.priority);
                Console.WriteLine(actualNode.request);
            }

            heap = heapCopy;
        }

        public void ShowList()
        {
            Node? temporalNode = new Node();
            temporalNode = firstNode;

            while(temporalNode != null)
            {
                Console.WriteLine(temporalNode.request);
                temporalNode = temporalNode.next;
            }

            temporalNode = null;
        }

        static void Main(string[] args)
        {
            //create a new ToDoList object
            ToDoListApp mainLogic = new ToDoListApp();
            mainLogic.NewTask("tarea 5", 1);
            mainLogic.NewTask("tarea 2", 1);
            mainLogic.NewTask("tarea 15", 10);
            mainLogic.NewTask("tarea 1", 2);

            mainLogic.ShowList();
            mainLogic.DeleteTask("tarea 15");
            mainLogic.ShowList();

            Console.WriteLine("Ahora ordenada con prioridad: ");
            mainLogic.OrderByPriority();

        }
    }
}