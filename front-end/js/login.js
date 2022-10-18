//'Password is required'
var alert1 = document.getElementById("alrt");
//'Incorrect Username or Password.'
var alert2 = document.getElementById("alrt1");
//'Username is required.'
var alert3 = document.getElementById("alrt2");
//'You are required to fill all the inputs.'
var alert4 = document.getElementById("alrt3");
var us = document.getElementById("userid");
var ps = document.getElementById("Password");
var email = document.getElementById("email");
var alrt = document.getElementById("alert");
var alrt1 = document.getElementById("alert1");

// Execute a function when the user presses a key on the keyboard
us.addEventListener("keypress", function(event) {
// If the user presses the "Enter" key on the keyboard
if (event.key === "Enter") {
// Cancel the default action, if needed
event.preventDefault();
// Trigger the button element with a click
document.getElementById("myBtn").click();
}
});
ps.addEventListener("keypress", function(event) {
// If the user presses the "Enter" key on the keyboard
if (event.key === "Enter") {
// Cancel the default action, if needed
event.preventDefault();
// Trigger the button element with a click
document.getElementById("myBtn").click();
}
});

//for the linking of the 'back to login page'

function gotologin(){

    window.location.href = "login.html";

}

//for account recovery page validation
function verify() {

    if (email.value == "james@gmail.com" || 
        email.value == "j@gmail.com" ||
        email.value == "james@email.com" ||
        email.value == "jam@gmail.com" ||
        email.value == "jc@gmail.com" ||
        email.value == "jamescarl@gmail.com" ||
        email.value == "jcs@gmail.com" ||
        email.value == "alegre" ||
        email.value == "james" ||
        email.value == "j.com" ||
        email.value == "jamescarl" 
    ) {

        window.location.href = "sendcode.html";
        
    }else if(email.value == ""){

        alrt1.style.display = "block";
        alrt.style.display = "none";
        email.style.borderColor = "#f01e2c";

    }else{

        alrt.style.display = "block";
        alrt1.style.display = "none";
        email.style.borderColor = "#f01e2c";

    }

}


function check(form){

//if the username input value = 'Litecloud' and password input value = 'Litecloud123' the page will be redirected to the landing page.
if(us.value == "Litecloud" && ps.value == "Litecloud123"){

   window.location.href = "welcome.html";
   return false;

}
//if the us.value is empty but the ps.value is correct, display 'Username is required' (alert3)
else if( us.value == "" && ps.value == "Litecloud123"){

    alert3.style.display = 'block';
    alert2.style.display = 'none';
    alert1.style.display = 'none';
    alert4.style.display = 'none';
    us.style.borderColor = "red";
    ps.style.borderColor = "#e5e5e5";
   
}
//if us.value is correct but the ps.value is empty, display 'Password is required' (alert1)
else if( us.value == "Litecloud" && ps.value == ""){

    alert1.style.display = 'block';
    alert3.style.display = 'none';
    alert2.style.display = 'none';
    alert4.style.display = 'none';
    ps.style.borderColor = "red";
    us.style.borderColor = "#e5e5e5";
   
}

//if the us.value is not equal to 'Litecloud' but ps.value is correct, display 'Incorrect username or password'(alert2)
else if( us.value != "Litecloud" && ps.value == "Litecloud123"){

    alert2.style.display = 'block';
    alert3.style.display = 'none';
    alert1.style.display = 'none';
    alert4.style.display = 'none';
    us.style.borderColor = "red";
    ps.style.borderColor = "red";
   
}
//if us.value is correct but the ps.value is not, dispaly 'Incorrect username or password' (alert2)
else if( us.value == "Litecloud" && ps.value != "Litecloud123"){

    alert2.style.display = 'block';
    alert3.style.display = 'none';
    alert1.style.display = 'none';
    alert4.style.display = 'none';
    ps.style.borderColor = "red";
    us.style.borderColor = "red";
   
}
//if us.value is empty but ps.value is not empty, display 'Username is required' (alert3)
else if(us.value == "" && ps.value != ""){

    alert3.style.display = 'block';
    alert1.style.display = 'none';
    alert2.style.display = 'none';
    alert4.style.display = 'none';
    us.style.borderColor = "red";
    ps.style.borderColor = "#e5e5e5";

}
//if us.value is not empty but ps.value is, display 'Password is required' (alert1)
else if(us.value != "" && ps.value == ""){

    alert1.style.display = 'block';
    alert3.style.display = 'none';
    alert2.style.display = 'none';
    alert4.style.display = 'none';
    ps.style.borderColor = "red";
    us.style.borderColor = "#e5e5e5";
   

}
//if both us.value and ps.value is empty, display 'You are required to fill all the inputs' (alert4)
else if(us.value == "" && ps.value == ""){

    alert4.style.display = 'block';
    alert3.style.display = 'none';
    alert1.style.display = 'none';
    alert2.style.display = 'none';
    us.style.borderColor = "red";
    ps.style.borderColor = "red";
   
}
//all the conditions not mentioned above will display 'Incorrect username or password' (alert2)
else{
   
    alert2.style.display = 'block';
    alert3.style.display = 'none';
    alert4.style.display = 'none';
    alert1.style.display = 'none';
    us.style.borderColor = "red";
    ps.style.borderColor = "red";

}

}


