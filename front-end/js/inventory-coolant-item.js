//WALA PA NAHUMAN.........

//ELEMENT USED IN JAVASCRIPT CODE...
var itemdes=document.getElementById("idescription-input")
var stu=document.getElementById("number-input");
var rtu=document.getElementById("avecost-input");
var number=document.getElementById("number-input");
var weightedAverage=document.getElementById("avecost-input");
var markup=document.getElementById("murate-input");
var SellingPrice=document.getElementById("sprice-input");
var brandname=document.getElementById("brandname");
var ratio=document.getElementById("ratio-input");
var thread=document.getElementById("thread-input");

//PRINTING INPUT FROM MODAL EDIT DELETE AND ADD FUNCTION
function SaveEditBrandName(){

    var brandeditinput=document.getElementById("editbrandnameinput"). value;
    document.getElementById("brand-output").innerHTML=brandeditinput;
}

function AddBrandName(){

    var addbrandname=document.getElementById("addbrandinput").value;
    document.getElementById("brand-output").innerHTML=addbrandname;  
}


//CHECKING THE DETAILS, ERROR AND MAKING THE INPUT REQUIRED
function CheckDetailEntries(){
  
    if(brandname.text==null || brandname.text=="" || brandname.text==undefined){
        brandname.style.borderColor="red"; 
        document.getElementById("modal_info").innerHTML="You have no input for Brand Name.";
  
    }
    else if (itemdes.innerText==undefined || itemdes.innerText==null || itemdes.innerText==""){
        size.style.borderColor="red";
        document.getElementById("modal_info").innerHTML="You have no input for the size.";  
    }
    else if(stockingunit.value == "" || stockingunit.value == null){ 
        stockingunit.style.borderColor="red";
        document.getElementById("modal_info").innerHTML="You have no input for stocking unit.";
       
    }
    else if(retailunit.value == "" || retailunit.value == null){
        retailunit.style.borderColor="red";
        document.getElementById("modal_info").innerHTML="You have no input for Retail Unit.";
       
    }
    else if(NoOfRTUorSTU.value == "" || NoOfRTUorSTU.value == null){
        NoOfRTUorSTU.style.borderColor="red";
        document.getElementById("modal_info").innerHTML="You have no input for Number of RTU per STU.";
       
    }
    else if(weightedAverage.value=="" || weightedAverage.value== null){
        weightedAverage.style.borderColor="red";
       document.getElementById("modal_info").innerHTML="You have no input in Weighted Average. ";
      
    }
    else if(markup.value=="" || markup.value== null){
        markup.style.borderColor="red";
        document.getElementById("modal_info").innerHTML="You have no input for Mark Up rate. ";
      
    }
    else if(count(markup.value)>=3)
    {
        document.getElementById("modal_info").innerHTML="Mark Up input must be in percentage form. ";
        alert("failure");
    }   
     else if(SellingPrice.value=="" || SellingPrice.value== null){
        weightedAverage.style.borderColor="red";
        document.getElementById("modal_info").innerHTML="You have no input for Selling Price. ";
      
    }
    else{
       document.getElementById("modal_info").innerHTML="You have successfully added the input.";
     
      
    }
}