<<<<<<< HEAD
=======
// function SaveNewSize(css){
  
//     var newsize=document.getElementById("new-size-input").value;
//     var size = document.createElement("td");
//     size.class="customer";
//     var newsizeinput = document.createTextNode(newsize);
//     size.appendChild(newsizeinput);
//     var size_element = document.getElementById("size-inputs");
//     size_element.appendChild(size);

//     var neweditbutton=document.createElement("td");
//     EditButton=document.createElement("button");
//     EditButton.class="btn btn-outline-primary" ;
//     EditIcon=document.createElement("i");
//     EditIcon.class="fa fa-pencil-square-o";
//     EditButton=document.createTextNode("Hello");   
//     neweditbutton.appendChild(EditButton);
//     var ButtonPosition=document.getElementById("size-inputs");
//     ButtonPosition.appendChild(neweditbutton); 
//     Create Row
//     var SizeTable=document.getElementById("SizeTable");
//     var SizeRow=SizeTable.insertRow(0);
//     var SizeCell1=SizeRow.inserCell(0);
//     var SizeCell2=SizeRow.insertCell(1);
//     SizeCell1.innerHTML="NEW CELL 1";
//     SizeCell2.innerHTML="NEW CELL 2";

//     EditIcon=document.createElement("i");
//     EditIcon.class="fa fa-pencil-square-o";
//     EditButton.appendChild(EditIcon);
//     var EditButton=document.createElement("button");
//     EditButton.class="btn btn-outline-primary";
    
// }
function SaveNewSize(css) {
    var sizeinput=document.getElementById("new-size-input").value;
    var newbutton=document.createElement("button");
    newbutton.innerHTML="Failure";

    var table = document.getElementById("size-inputs");
    var row = table.insertRow(0);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    cell1.innerHTML = sizeinput;
    cell2.appendChild(newbutton);
  }

>>>>>>> develop-ijkm
function SaveEditSize(){
    var editedsize=document.getElementById("change-size-input");
    document.getElementById("editedsizeresult").innerHTML=editedsize.value;
}
function OnSizeEdit(){
    var editedsizeresult=document.getElementById("editedsizeresult").innerText;
    document.getElementById("previous-size-input").value=editedsizeresult;
}
