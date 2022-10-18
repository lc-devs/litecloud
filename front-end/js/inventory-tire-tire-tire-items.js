// brandname=document.getElementById("brandname-inputlabel");
// vehiclemodel=document.getElementById("vehicle-inputlabel");
// partdescription=document.getElementById("partdescription-inputlabel");
// partnumber=document.getElementById("partnumber-inputlabel");
//

// //function for the every input type of the brand name vehicle category part description and partnumber
// function addbrandname(){
//     brandname.text=document.getElementById("category-brandname").value;
//     document.getElementById("brandname-output").innerHTML=brandname.text;
//     document.getElementById("brandedit-input").value=brandname.text;
    
// }
// function addvehicle(){
//     vehiclemodel.text=document.getElementById("category-vehiclemodel").value;
//     document.getElementById("vehicle-output").innerHTML=vehiclemodel.text;
//     document.getElementById("vehiclecategoryedit-input").value=brandname.text;
    
// }
// function addpartdescription(){
//     partdescription.text=document.getElementById("category-partdescription").value;
//     document.getElementById("partdescription-output").innerHTML=partdescription.text;
//     document.getElementById("partdescriptionedit-input").value=brandname.text;
    
// }
// function addpartnumber(){
//     partnumber.text=document.getElementById("category-partnumber").value;
//     document.getElementById("partnumber-output").innerHTML=partnumber.text;
//     document.getElementById("partnumberedit-input").value=brandname.text;
  
// }
////for the function of save and edit for the inventory vehicle parts items inventory
// function savebrandnameedit(){
//     brandedit=document.getElementById("brandedit-input").value;
//     document.getElementById("category-brandname").value=brandedit.text;
//     document.getElementById("brandname-output").value=brandedit.text;
// }
// function savevehiclecategoryedit(){
//     vehicleedit=document.getElementById("vehiclecategoryedit-input").value;
//     document.getElementById("category-vehiclecategory").value=vehicleedit.text;
//     document.getElementById("vehicle-output").value=vehicleedit.text;
// }
// function savepartdescriptionedit(){
//     partdescriptionedit=document.getElementById("partdescriptionedit-input").value;
//     document.getElementById("category-partdescription").value=partdescriptionedit.text;
//     document.getElementById("partdescription-output").value=partdescriptionedit.text;
// }
// function savepartnumberedit(){
//     partnumberedit=document.getElementById("partnumberedit-input").value;
//     document.getElementById("category-partnumber").value=partnumberedit.text;
//     document.getElementById("partnumber-output").value=partnumberedit.text;
// }

// function brandname(){
//     window.location.href=("brandname-page-managementandpage.html");
// }
// function size(){
//     window.location.href=("size-management-page.html");
// }
// function ratio(){
//     window.location.href=("ratio-management.html");
// }
// function threadpattern(){
//     window.location.href=("thread-pattern-management.html");
// }


function SavingInventoryVehicleParts(){
    alert("hello")
brandnamevehicle = document.getElementById("brandname-inputlabel");
vehiclecategoryvehicle = document.getElementById("vehicle-inputlabel");
partdescriptionvehicle = document.getElementById("partdescription-inputlabel");
partnumbervehicle = document.getElementById("partnumber-inputlabel");
stockingunit = document.getElementById("stu-input");
retailunit = document.getElementById("landline2-input");
numberofrtu = document.getElementById("number-input");
weightedaverage = document.getElementById("avecost-input");
markuprate = document.getElementById("murate-input");
alertvehicle = document.getElementById("alrt");
inputs = [brandnamevehicle.value, vehiclecategoryvehicle.value, partdescriptionvehicle.value, partnumbervehicle.value, stockingunit.value, retailunit.value, numberofrtu.value,weightedaverage.value, markuprate.value]
 
 //Variable to keep track of Errors. Initialize to 0.
 var err = 0
    
 //Start loop to validate.
 for (var i = 0; i < inputs.length; i++) {
     //Checks fields in the array making sure they are not empty.
     if(inputs[i] === "") {
         err++;
     }
 }

if (brandnamevehicle.value == 0){
    alertvehicle.style.display="block";
    brandnamevehicle.style.borderColor="red";
}else{
    brandnamevehicle.style.borderColor="#ced4da";
}
if (vehiclecategoryvehicle.value == 0){
    alertvehicle.style.display="block";
    vehiclecategoryvehicle.style.borderColor="red";
}else{
    vehiclecategoryvehicle.style.borderColor="#ced4da";
}
if (partdescriptionvehicle.value == 0){
    alertvehicle.style.display="block";
    partdescriptionvehicle.style.borderColor="red";
}else{
    partdescriptionvehicle.style.borderColor="#ced4da";
}
if (partnumbervehicle.value == 0){
    alertvehicle.style.display="block";
    partnumbervehicle.style.borderColor="red";
}else{
    partnumbervehicle.style.borderColor="#ced4da";
}
if (stockingunit.value == ""){
    alertvehicle.style.display="block";
    stockingunit.style.borderColor="red";
}else{
    stockingunit.style.borderColor="#ced4da";
}
if (retailunit.value == ""){
    alertvehicle.style.display="block";
    retailunit.style.borderColor="red";
}else{
    retailunit.style.borderColor="#ced4da";
}
if (numberofrtu.value == ""){
    alertvehicle.style.display="block";
    numberofrtu.style.borderColor="red";
}else{
    numberofrtu.style.borderColor="#ced4da";
}
if (weightedaverage.value == ""){
    alertvehicle.style.display="block";
    weightedaverage.style.borderColor="red";
}else{
    weightedaverage.style.borderColor="#ced4da";
}
if (markuprate.value == ""){
    alertvehicle.style.display="block";
    markuprate.style.borderColor="red";
}else{
    markuprate.style.borderColor="#ced4da";
}
//Check that there are no errors.
if (err === 0) {
    alertvehicle.style.display = "none";
    document.getElementById("notice").style.display = "none";
    $('#firstconfirm').modal('show');
} else {
    alertvehicle.style.display = "block";
}

}

// this is js for the inventory tire items
function brandnametire(){
    window.location.href=("brandnametire-page-managementandpage.html");
}
function vehiclecategorytire(){
    window.location.href=("vehiclecategorytire-page-managementandpage.html");
}
function partdescriptiontire(){
    window.location.href=("partdescriptiontire-page-managementandpage.html");
}
function partnumbertire(){
    window.location.href=("partnumbertire-page-managementandpage.html");
}

