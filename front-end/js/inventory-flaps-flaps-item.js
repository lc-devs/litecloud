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
var brandname=document.getElementById("brandname");

//add brandname
function AddBrandName(){
    var addbrandname=document.getElementById("addbrandnameinput").value;
    document.getElementById("brand-output").innerHTML=addbrandname;  
}
  

//populate Country Dropdown list
var response = InvokeService("CountryMethods/getall", "GET", "");

if (response.code == 200) {

    country.innerHTML = "";
    var data = JSON.parse(response.data);

    var countries = JSON.parse(data.jsonData);
    for (var i = 0; i < countries.length; i++) {
        PopulateDropdownList(country, countries[i].country_id, countries[i].country_name);
    }
    
}

function GetProvinces() {

    country = document.getElementById("country-input");

    var country_id = country.value;

    var response = InvokeService("CountryMethods/getone/" + country_id, "GET", "");

    if (response.code == 200) {

        province.innerHTML = "";
        var data = JSON.parse(response.data);

        var provinces = JSON.parse(data.jsonData);

        console.log(provinces);

        for (var i = 0; i < provinces.length; i++) {
            PopulateDropdownList(province, provinces[i].country_id, provinces[i].country_name);
        }

    }


}
function GetTown() {

    country = document.getElementById("province-input");

    var country_id = country.value;

    var response = InvokeService("CountryMethods/getone/" + country_id, "GET", "");

    if (response.code == 200) {

        province.innerHTML = "";
        var data = JSON.parse(response.data);

        var provinces = JSON.parse(data.jsonData);

        console.log(provinces);

        for (var i = 0; i < provinces.length; i++) {
            PopulateDropdownList(province, provinces[i].country_id, provinces[i].country_name);
        }

    }


}
function GetBarangay() {

    country = document.getElementById("country-input");

    var country_id = country.value;

    var response = InvokeService("CountryMethods/getone/" + country_id, "GET", "");

    if (response.code == 200) {

        province.innerHTML = "";
        var data = JSON.parse(response.data);

        var provinces = JSON.parse(data.jsonData);

        console.log(provinces);

        for (var i = 0; i < provinces.length; i++) {
            PopulateDropdownList(province, provinces[i].country_id, provinces[i].country_name);
        }

    }


}
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
else if(country.value==0 && country.text=="Country" || country.text==""){
    country.style.borderColor="red"; 
    failure=document.getElementById("modal_info").innerHTML="You have no input for Country.";
    
}
else if(province.text=="Province/State"  || country.text==""){
    province.style.borderColor="red";
    failure=document.getElementById("modal_info").innerHTML="YOu have no input for Province/State.";
    
}
else if(town.text=="Town/Municipality" || country.text==""){
    town.style.borderColor="red";
    failure=document.getElementById("modal_info").innerHTML="You have no input for Town/Municipality.";
    
}
else if(barangay.text=="Barangay/District" || country.text==""){
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
   window.location.href=("person-customer-management.html");
  
}
}
function AddCountry(){
    var addcountryinput=document.getElementById("country-input");
    addcountryinput.text=document.getElementById("add-country-input").value;
    document.getElementById("country-output").innerHTML=addcountryinput.text;
    
}
function AddProvince(){
    var addprovinceinput=document.getElementById("province-input");
    addprovinceinput.text=document.getElementById("add-province-input").value;
    document.getElementById("province-output").innerHTML=addprovinceinput.text;
}
function AddTown(){
    var addtowninput=document.getElementById("town-input");
    addtowninput.text=document.getElementById("add-town-input").value;
    document.getElementById("town-output").innerHTML=addtowninput.text;
}
function AddBarangay(){
    var addbarangayinput=document.getElementById("barangay-input");
    addbarangayinput.text=document.getElementById("add-barangay-input").value;
    document.getElementById("barangay-output").innerHTML=addbarangayinput.text;
}
function GoToPersonPage(){
    window.location.href=("person-customer-management.html");
}
function GoToPersonPageForm(){
    window.location.href=("add-person-customer-management.html");
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

  function DisableSelPrice() {

    var murateinput = document.getElementById("murate-input");

    if(murateinput.value != "") {

        document.getElementById("sprice-input").disabled = true;

    }else if(murateinput.value == 0){

        document.getElementById("sprice-input").disabled = false;
 
    }else{

        document.getElementById("sprice-input").disabled = false;
 
    }

}

//     function DisableMURate() {

//     var spriceinput = document.getElementById("sprice-input");
    
//     if(spriceinput.value != "" || spriceinput.value != 0) {
    
//         document.getElementById("murate-input").disabled = true;
    
//     }else if(murateinput.value == 0){

//         document.getElementById("sprice-input").disabled = false;
 
//     }else{

//         document.getElementById("sprice-input").disabled = false;
 
//     }

        
// }

// $(document).ready(function(){
 
//     $("#myBtn").click(function(){



    function ValidateInput() {

        var engineadditives = document.getElementById("engine-additives");
        var brandinput = document.getElementById("brand-input");
        // var itemdesinput = document.getElementById("itemdes-input");
        var stuinput = document.getElementById("stu-input");
        var rtuinput = document.getElementById("rtu-input");
        var ratioinput = document.getElementById("ratio-input");
        var avecostinput = document.getElementById("avecost-input");
        var murateinput = document.getElementById("murate-input");
        var spriceinput = document.getElementById("sprice-input");
        var alert = document.getElementById("alrt");
        var sizeinput = document.getElementById("size-input")
        var engineadditivesvalues = engineadditives.options[engineadditives.selectedIndex].text;   
        var brandinputvalues = brandinput.options[brandinput.selectedIndex].text;   
        var sizeinputvalues = sizeinput.options[sizeinput.selectedIndex].text;
        var inputs = [engineadditives.value,brandinput.value,sizeinput.value, itemdesinput.value, stuinput.value, rtuinput.value, ratioinput.value, avecostinput.value, murateinput.value, spriceinput.value];
    
        //Variable to keep track of Errors. Initialize to 0.
        var err = 0
    
        //Start loop to validate.
        for (var i = 0; i < inputs.length; i++) {
            //Checks fields in the array making sure they are not empty.
            if(inputs[i] === "") {
                err++;
            }
        }

        if(engineadditivesvalues == ""|| engineadditivesvalues == "Engine additives") {

            engineadditives.style.borderColor = "red";

        }else{

            engineadditives.style.borderColor = "#ced4da";

        }

        if(brandinputvalues == ""|| brandinputvalues == "Brand Name") {

            brandinput.style.borderColor = "red";

        }else{

            brandinput.style.borderColor = "#ced4da";
            
        }
        
        if( sizeinputvalues == "Size") {

            sizeinput.style.borderColor = "red";

        }else{

            sizeinput.style.borderColor = "#ced4da";
            
        }
        // if(itemdesinput.value == "") {

        //     itemdesinput.style.borderColor = "red";

        // }else{

            itemdesinput.style.borderColor = "#ced4da";
            
        }

        if(stuinput.value == "") {

            stuinput.style.borderColor = "red";
            
        }else{

            stuinput.style.borderColor = "#ced4da";
                
            }

        if(rtuinput.value == "") {

            rtuinput.style.borderColor = "red";

        }else{

            rtuinput.style.borderColor = "#ced4da";
            
        }

        if(ratioinput.value == "") {

            ratioinput.style.borderColor = "red";

        }else{

            ratioinput.style.borderColor = "#ced4da";
            
        }

        if(avecostinput.value == "") {

            avecostinput.style.borderColor = "red";

        }else{

            avecostinput.style.borderColor = "#ced4da";
            
        }

        if(murateinput.value == "") {

            murateinput.style.borderColor = "red";
            

        }else{

            murateinput.style.borderColor = "#ced4da";
            
        }

        if(spriceinput.value == "") {

            spriceinput.style.borderColor = "red";

        }else{

            spriceinput.style.borderColor = "#ced4da";
            
        }
    
        //Check that there are no errors.
        if (err === 0) {
            alert.style.display = "none";
            document.getElementById("notice").style.display = "none";
            $('#firstconfirm').modal('show');
        } else {
            alert.style.display = "block";
        }

   

        function ValidateNumberInput(event) {
        if (event.key == "-") {
                event.preventDefault();
                document.getElementById("notice").style.display = "block";
                this.parentElement.style.borderColor = "red";
                return false;
            }
        }
        function AddBrandname() {
            window.location.href=("brandname-page-managementandpage.html");
        }
        function AddSize() {
            window.location.href=("size-management-page.html");
        }