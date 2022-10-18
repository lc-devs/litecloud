var brandname = document.getElementById("brandname");
var size= document.getElementById("size");
var stuinput=document.getElementById("stu-input");
var rtuinput = document.getElementById("rtu-input");
var numberinput = document.getElementById("number-input");
var avecostinput = document.getElementById("avecost-input");
var murateinput = document.getElementById("murate-input");
var spriceinput = document.getElementById("sprice-input");
var ErrorAlert=document.getElementById("notice");

function ValidateInputs(){
    var inputs = [brandname.text, size.text, stuinput.value, rtuinput.value, numberinput.value, avecostinput.value, murateinput.value, spriceinput.value];
    //Variable to keep track of Errors. Initialize to 0.
    var err = 0;

    //Start loop to validate.
    for (var i = 0; i < inputs.length; i++) {
        //Checks fields in the array making sure they are not empty.
        if(inputs[i] === "") {
            err++;
        }
    }
    if(brandname.options[brandname.selectedIndex].text == "" || brandname.options[brandname.selectedIndex].text=="Brand Names") {

        brandname.style.borderColor = "red";

    }else{

        brandname.style.borderColor = "#ced4da";

    }

    if(size.options[size.selectedIndex].text == ""|| size.options[size.selectedIndex].text == "Sizes") {

        size.style.borderColor = "red";

    }else{

        size.style.borderColor = "#ced4da";
        
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

    if(numberinput.value == "") {

        numberinput.style.borderColor = "red";

    }else{

        numberinput.style.borderColor = "#ced4da";
        
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
console.log(err);
    //Check that there are no errors.
    if (err === 0) {
        ErrorAlert.style.display = "none";
        $('#firstconfirm').modal('show');
    } else {
        ErrorAlert.style.display = "block";
        $('#firstconfirm').modal('hide');
    }

}

    function ValidateNumberInput(event) {
    if (event.key == "-") {
            event.preventDefault();
            document.getElementById("notice").style.display = "block";
            this.parentElement.style.borderColor = "red";
            return false;
        }
    }


    function AddNewCategory() {

        var addnewcategoryinput = document.getElementById("categoriessearchbar");
        var option = document.createElement("option");
        var brandinputtext = document.getElementById("brand-input"); 
        option.text = addnewcategoryinput.value;

        brandinputtext.add(option);
        const $select = document.querySelector('#brand-input');
        $select.value = addnewcategoryinput.value;

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

    province = document.getElementById("province-input");

    var province_id = province.value;

    var response = InvokeService("ProvinceMethods/getone/" + province_id, "GET", "");

    if (response.code == 200) {

        town.innerHTML = "";
        var data = JSON.parse(response.data);

        var town = JSON.parse(data.jsonData);

        console.log(town);

        for (var i = 0; i < town.length; i++) {
            PopulateDropdownList(town, town[i].town_id, town[i].province_name);
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


    

        


        

        
