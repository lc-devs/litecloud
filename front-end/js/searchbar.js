//===================================================================
//Function: Search bar
//Purpose: To search caterory and items
//Author: Mark Anthony Alegre
//Parameter:
//  Name                     Comment
//  -----------              --------------------------------
//                 
//
//Result:
// Populate result form table 
//===================================================================

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
