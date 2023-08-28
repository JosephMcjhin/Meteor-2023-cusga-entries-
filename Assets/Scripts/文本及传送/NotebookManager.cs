using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookManager : MonoBehaviour
{
    public static NotebookManager instance;
    void Awake(){
        if(instance!=null){
            Destroy(this);
        }
        instance = this;
    }

    public Notebook notebook;

    public bool isopen;

    public void Add_text(Note note){
        if(notebook.is_added[note.note_id] == false){
            notebook.is_added[note.note_id] = true;
            notebook.note.Add(note);
        }
    }
}
