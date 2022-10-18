// variables for inventory vehicle parts items
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
 



function SavingInventoryvehicleparts(){

 //Variable to keep track of Errors. Initialize to 0.
 var err = 0;
    
 //Start loop to validate.
 for (var i = 0; i < inputs.length; i++) {
     //Checks fields in the array making sure they are not empty.
     if(inputs[i] === "") {
         err++;
     }
 }
// conditions
if (brandnamevehicle.value == 0 ){
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
// function to new window to add brandnae
function brandname(){
    window.location.href=("brandname-page-managementandpage.html");
}
// function to new window to add vehicle category
function vehiclecategory(){
    window.location.href=("vehicle-category-management.html");
}
// function to new window to add part description
function partdescription(){
    window.location.href=("part-description-management.html");
}
// function to new window to add part number
function partnumber(){
    window.location.href=("part-number-management.html");
}


