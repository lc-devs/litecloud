function GetCamera(selectCamera) {

    select = document.getElementById(selectCamera);
    // This method will trigger user permissions 

    document.getElementById(selectCamera).innerHTML = "";
    Html5Qrcode.getCameras().then(devices => {
        /**
          * devices would be an array of objects of type:
          * { id: "id", label: "label" }
         */
        if (devices && devices.length) {
            for (let i = 0; i < devices.length; i++) {

                var opt = document.createElement('option');
                opt.value = devices[i].id;
                opt.innerHTML = devices[i].label;
                select.add(opt);
            }

            // .. use this to start scanning.
        }
    }).catch(err => {
        // handle err 
        alert(err);
    });

    
}

function ReadCamera(selectCamera, divReader, txtField, btnSearch, modalbtnClose) {

    select = document.getElementById(selectCamera);

    var cameraId = select.options[select.selectedIndex].value;

    var txtField = document.getElementById(txtField);

    var btnSearch = document.getElementById(btnSearch);
    var modalbtnClose = document.getElementById(modalbtnClose);

    const html5QrCode = new Html5Qrcode(/* element id */ divReader);

    html5QrCode.start(
        cameraId,
        {
            fps: 10,    // Optional, frame per seconds for qr code scanning
            qrbox: { width: 250, height: 250 }  // Optional, if you want bounded box UI
        },
        (decodedText, decodedResult) => {
            // do something when code is read

            txtField.value = decodedText;

            html5QrCode.stop().then((ignore) => {
                // QR Code scanning is stopped.
            }).catch((err) => {
                // Stop failed, handle it.
            });

            modalbtnClose.click();

            btnSearch.click();


        },
        (errorMessage) => {
            // parse error, ignore it.
        })
        .catch((err) => {
            alert(err);
        });

}
