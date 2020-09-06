﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ArrowList : MonoBehaviour
    {
        public List<ArrowListButton> arrowListButtons = new List<ArrowListButton>();
        public int currentPage = 0;

        [Header("Button prefabs")]
        public GameObject arrowButtonPrefab;
        public GameObject pageButtonPrefab;

        public List<GameObject> elements;
        public int maxCount;
        public int pageCount;
        public void Initialize(List<GameObject> elements)
        {
            this.elements = elements;
            SetPage(0); // setting default page to 0
            
            if(elements.Count > maxCount) // continue only if the maxcount is smaller than the actual button count
            {
                AddButton(arrowButtonPrefab,-1,0); // instatiates the previous button
                pageCount = (int)Mathf.Ceil((float)elements.Count/(float)maxCount);
                
                for(int count = 0; count<pageCount; count++) // cycle through all pages
                {
                    AddButton(pageButtonPrefab,0,count); 
                }
                AddButton(arrowButtonPrefab,1,0); // instatiates the next button
                SetPageButtonsSize();
            }
        }
        public void AddButton(GameObject prefab, int adder, int index)
        {
            GameObject button = Instantiate(prefab,transform);

            // mirrors the button so the arrow faces the right way
            if(adder != 0) button.transform.localScale = new Vector3(adder,1,1);

            // initializes the component
            ArrowListButton buttonComponent = button.GetComponent<ArrowListButton>(); 
            buttonComponent.adder = adder;
            buttonComponent.index = index;
            buttonComponent.arrowList = this;
            
            arrowListButtons.Add(buttonComponent);
        }

        public void ChangePage(int adder)
        {
            int newPage = currentPage+adder;

            // sets the newPage if its too high or too low
            if(newPage >= pageCount) newPage = 0;
            else if(newPage < 0) newPage = pageCount-1;
            
            SetPage(newPage);
        }

        public void SetPage(int page)
        {
            for(int i = 0; i < elements.Count ; i++) 
            {
                // setting the pages in the wrong page as visible
                // floor(i/maxCount) = current page index of the gameobject
                int elementPage = (int)Mathf.Floor((float)i/(float)maxCount);
                if(elementPage!=page) elements[i].SetActive(false); 
                else  elements[i].SetActive(true); 
            }
            currentPage = page;
            SetPageButtonsSize();
        }

        public void SetPageButtonsSize()
        {
            foreach(ArrowListButton button in arrowListButtons)
            {
                if(button.adder != 0) continue; // skips the arrow buttons

                // sets the buttons size based on the current page
                if(button.index == currentPage)  button.transform.localScale = new Vector3(1.1f,1.1f,1.1f);
                else  button.transform.localScale = new Vector3(1,1,1);
            }
        }

    }
}
