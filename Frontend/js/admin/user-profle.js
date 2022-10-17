var authkey = sessionStorage.getItem('authkey');
var userSite = sessionStorage.getItem('userSiteDescription');
var sectionId = sessionStorage.getItem('sectionId');
var sectionDescription;

if(sectionId ==0 ){
    sectionDescription = "Admin";
}else if(sectionId == 1){
    sectionDescription = "Logistics";
}else if(sectionId == 2){
    sectionDescription = "Laundry";
}
if (authkey == "" || authkey == null) {
    location.href = "admin-login.html";
}
GetDomElement("userDetails").innerHTML =
    ` <p class='card-text'><strong>User: </strong> <span class='text-capitalize'>${sessionStorage.getItem("userName")}</span> <br>` +
    ` <strong> Site:</strong> <span class='text-capitalize'> ${userSite==""?"n/a":userSite} </span> <br>` +
    ` <strong> Type of User: </strong>${sectionDescription}  ` +
    "</p>";
GetDomElement("mobileUserDetails").innerHTML =
    ` <p class='card-text'><strong>User: </strong> <span class='text-capitalize'>${sessionStorage.getItem("userName")}</span> <br>` +
    ` <strong> Site:</strong> <span class='text-capitalize'> ${userSite==""?"n/a":userSite} </span> <br>` +
    ` <strong> Type of User: </strong>${sectionDescription}  ` +
    "</p>";