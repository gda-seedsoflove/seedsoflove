README for Dialogue System

Instructions for Use:
1. Drag DialoguePrefab into hierarchy for scene that needs dialogue
2. Drag prefabs for characters that are present in this scene into 
   this scene's hierarchy
   a. To create prefabs for individual characters, create an empty
      GameObject with that characters name
   b. Add Character script to this empty GameObject
   c. Populate the character poses array with the sprites for said
      character
   d. Drag GameObject into Assets window to create a prefab (a
      reusable GameObject with the same properties)
3. Make sure that the text file containing dialugue for this scene is
   located in folder Assets/DialogueText and follows naming convention
   SceneName_Dialogue.txt
