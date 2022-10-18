function fnExcelReport(title, referenceTable, filename, From, To, Customer) {
    var tab_text = `<h1>${title} </h1>
                    <p>Starting Date: ${From}</p>
                    <p>End Date: ${To}</p>
                    <p>Customer Name: ${Customer}</p>
                    <table border='2px'><tr bgcolor='#87AFC6'>`;
    var i = 0;
    tab = document.getElementById(referenceTable); // id of table

    for (i = 0; i < tab.rows.length; i++) {
        tab_text = tab_text + tab.rows[i].innerHTML + "</tr>";
    }

    tab_text = tab_text + "</table>";

    var UserAgent = window.navigator.userAgent;
    var msie = UserAgent.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer
    {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, `${filename}.xls`);
        return (sa);
    } else {
        var a = document.createElement('a');
        var data_type = 'data:application/vnd.ms-excel';
        a.href = data_type + ', ' + encodeURIComponent(tab_text);
        a.download = filename + '.xls';
        a.click();
    }


}