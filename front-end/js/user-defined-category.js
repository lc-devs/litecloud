
function AddNewUserDefinedCategory(){
    NewUnitLocation=document.getElementById("add-usercategory-input").value;

    var response = InvokeService("Inventory_ItemCategoriesMethods?description=" + NewUnitLocation, "POST", "");
    console.log(response);
    if(response.code==200){

        var data=JSON.parse(response.data);

        if(data.code==200){
            var oData=JSON.parse(data.jsonData);
            $("#CategorySaveSuccessfully").modal("show");
        }

    }
}

function PopulateUserDefinedCategory(){

    var newUserCategory_row=document.createElement("tr");
    var response = InvokeService("Inventory_ItemCategoriesMethods/description/" + NewUnitLocation, "GET", "");
    console.log(response)

    if(response.code=200){
        var data=JSON.parse(response.data);
        userTBL=document.getElementById("UserTbl");
        if(data.code=200){
            var oData=JSON.parse(data.jsonData);
            for (var i = 0; i<oData.length; i++){
                userTBL.innerHTML +=
                "<tr> " +
                "<td class='border text-center px-0'>"+ oData[i].description + "</td>" +
                "<td class='border text-center px-0'><button type='button' class='btn btn-outline-primary' onfocus='SetItemDes()'  onclick='SetEditPersonId("+oData[i].item_category_id+")' data-toggle='modal' data-target='#editcategory'><i class='fa fa-pencil-square-o'></i></button><button type='button' class='btn btn-danger' data-toggle='modal' data-target='#deletecategory' onclick='SetDeletePersonId("+oData[i].item_category_id+")'><i class='fa fa-trash' style='color: white;'></i>&nbsp;</button> </td>" +
                "</tr>";
            }
        }
    }
}
function SetEditPersonId(item_category_id){
    itemCategory=document.getElementById("item_category_id").value;
    itemCategory=item_category_id;
    var response=InvokeService("Inventory_ItemCategoriesMethods/getone/"+item_category_id, "GET", "");
    console.log(response)
     if (response.code==200){
         var data=JSON.parse(response.data);
         var PersonId=JSON.parse(data.jsonData);

         for (var i=0; i<PersonId.length; i++){
             console.log(PersonId[i].description)
             oldCategory=document.getElementById("old-category-input").value=PersonId[i].description;
             document.getElementById("item_description").value=PersonId[i].description;
             newItemDes=document.getElementById("edit-item-category").value;
             
         }
     }
}
function SetItemDes(){
    var NewItemDes=document.getElementById("new-item-category");
    var EditedCategory=document.getElementById("edit-item-category").value;
    NewItemDes.value=EditedCategory;
}
function EditUserDefinedCategory(NewItemDes){
    
    itemCategory_id=document.getElementById("item_category_id").value;
    document.getElementById("new-item-category").value=NewItemDes;

    console.log(NewItemDes);

    var response=InvokeService("Inventory_ItemCategoriesMethods/id?item_category_id="+itemCategory_id+"&"+"description="+ NewItemDes, "PUT", "");
   console.log(response)
    if(response.code=200){
        data=JSON.parse(response.data);
        if(data.code==200){
            editUserDefinedCategory=JSON.parse(data.jsonData);
        }
        else if(data.code==404){
            var tblbody = document.getElementById('UserTbl');
            tblbody.innerHTML="";
            tblbody.innerHTML +=
                "<tr> " +
                "<td class='border text-center px-0' colspan='2'>No Data Found.</td>" +
                "</tr>";
        }
    }
}
function SetDeletePersonId(item_category_id){
    setItemCategoryId=document.getElementById("category_id");
    setItemCategoryId.value=item_category_id;

    var response=InvokeService("Inventory_ItemCategoriesMethods/getone/"+item_category_id, "GET", "");
    console.log(response)
     if (response.code==200){
         var data=JSON.parse(response.data);
         var PersonId=JSON.parse(data.jsonData);

         for (var i=0; i<PersonId.length; i++){
             console.log(PersonId[i].description)
             document.getElementById("delete-country-input").value=PersonId[i].description;
         }
     }
}
function DeleteUserDefinedCategory(){
    var category_id=document.getElementById("category_id").value;

    var response = InvokeService("Inventory_ItemCategoriesMethods/id?id=" + category_id, "DELETE", "");
    console.log(response);

    if (response.code==200){
        var data=JSON.parse(response.data);
        if(data.code==200){
            CategoryTable=document.getElementById("UserTbl");
            CategoryTable.innerHTML="";
            deleteCategory=JSON.parse(data.jsonData);
            $("#categorydeletesuccess").modal("show");
        }
        else{

        }
    }
}