//container for filling up input
var lastname=document.getElementById("lastname-input");
var firstname=document.getElementById("firstname-input");
var middlename=document.getElementById("middlename-input");
var dateofbirth=document.getElementById("dateofbirth-input");
var gender=document.getElementById("gender-input");
var status=document.getElementById("status-input");
var country=document.getElementById("country-input");
var province=document.getElementById("province-input");
var town=document.getElementById("town-input");
var barangay=document.getElementById("barangay-input");
var zipcode=document.getElementById("zipcode-input");
var tin=document.getElementById("tin-input");
var bldg=document.getElementById("bldg-input");
var email=document.getElementById("email-input");
var landline1=document.getElementById("landline1-input");
var landline2=document.getElementById("landline2-input");
var mobile1=document.getElementById("mobile1-input");
var mobile2=document.getElementById("mobile2-input");


function savingpersoncustomermanagement(){
if (lastname.value == "" || lastname.value == null){
    lastname.style.borderColor="red";
    failure=document.getElementById("modal_info").innerHTML="You have no input for your Surname.";
   
}
else if(firstname.value == "" || firstname.value == null){
    firstname.style.borderColor="red";
    failure=document.getElementById("modal_info").innerHTML="You have no input for your Firstname.";
   
}
else if(middlename.value == "" || middlename.value == null){
    middlename.style.borderColor="red";
    failure=document.getElementById("modal_info").innerHTML="You have no input for your Middlename.";
    
}
else if(gender.value==0){
    gender.style.borderColor="red";
    failure=document.getElementById("modal_info").innerHTML="You have no input for your Gender.";
    
}
else if(country.value==0){
    country.style.borderColor="red"; 
    failure=document.getElementById("modal_info").innerHTML="You have no input for Country.";
    
}
else if(province.value==0){
    province.style.borderColor="red";
    failure=document.getElementById("modal_info").innerHTML="YOu have no input for Province/State.";
    
}
else if(town.value==0){
    town.style.borderColor="red";
    failure=document.getElementById("modal_info").innerHTML="You have no input for Town/Municipality.";
    
}
else if(barangay.value==0){
    barangay.style.borderColor="red";
    failure=document.getElementById("modal_info").innerHTML="You have no input for Barangay/District. Please have a input.";
    
}
else if(email.value == "" || email.value == null){
    email.style.borderColor="red";
    failure=document.getElementById("modal_info").innerHTML="You have no input for Email. Please have an input at least one.";
    
}
else if(landline1.value=="" || landline1.value==null && mobile1.value=="" || mobile1.value==null){
   landline1.style.borderColor="red";
   mobile1.style.borderColor="red";
   failure=document.getElementById("modal_info").innerHTML="You have no contact either in Landline and Mobile number. Please fill the Landline or the Mobile number. ";
   
}
else{
   success=document.getElementById("modal_info").innerHTML="You have successfully added the input.";
  
}
}

function ConfirmSuccess(){
    if(success==true)
{
    window.location.href=("person-customer-management");
}
}
function GoToPersonPage(){
    window.location.href=("person-customer-management.html")
}
function GoToPersonPageForm(){
    window.location.href=("add-person-customer-management.html")
}

function search_customer() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("searchbar");
    filter = input.value.toUpperCase();
    table = document.getElementById("searchpersons");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
      td = tr[i].getElementsByTagName("td")[0];
      if (td) {
        txtValue = td.textContent || td.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
          tr[i].style.display = "";
        } else {
          tr[i].style.display = "none";
        }
      }       
    }
  }
