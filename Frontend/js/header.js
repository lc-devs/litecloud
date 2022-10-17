if (GetAuthKey() == "" || GetAuthKey() == null) {
    alert("You are required to log-in before accesssing this page!");
    window.location.href = "admin-login.html";
}

var menu = InvokeService(GetURL(), `AccessMenuTemplateMethods/getmenu/${sessionStorage.getItem("menu_template")}?type=desktop`, "GET", "");

if (menu.code == 200) {
    var oMenu = JSON.parse(menu.data);
    if (oMenu.code == 200) {

        var parsedData = JSON.parse(oMenu.jsonData);
        document.getElementById("desktop").innerHTML = parsedData.desktop;
        document.getElementById("mobile").innerHTML = parsedData.mobile;
    } else if(oMenu.code == 404 || oMenu.code == 204 || oMenu.code == 401) {
        alert("You are not authorized to access this page!");
        window.location.href = "admin-login.html";
    }else{
        alert("Server is OFFLINE!");
    }
} else if(menu.code == 404 || menu.code == 204 || menu.code == 401) {
    alert("You are not authorized to access this page!");
    window.location.href = "admin-login.html";
}else{
    alert("Server is OFFLINE!");
}




