ClosePopupModal("btnLogoutCancel","btnCloseLogout");
ClosePopupModal("btnLogoutCancelTwo","btnCloseLogoutTwo");
ClosePopupModal("btnLogoutConfirm","btnCloseLogout");

GetDomElement('btnLogoutConfirmTwo').onclick = ()=>{
    if(sessionStorage.getItem('portalType') == 9999){
        sessionStorage.clear();
        location.href = "admin-login.html";
    }else{
        sessionStorage.clear();
        location.href = "online-customer-side-login.html";
    }
 
}