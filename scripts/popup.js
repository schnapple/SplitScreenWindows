function sendToPopup(message,fin_lis){
    fin_lis.sort(function(a,b){
        if (a[0] > b[0]){
            return 1;
        }
        else if (a[0] < b[0]){
            return -1;
        }
        else
            return 0;
    });
    message.innerHTML = "";
    for (var x = fin_lis.length -1; x >= 0;x--){
        if (fin_lis[x][0] > 0){
            message.innerHTML += fin_lis[x][1];
        }
    }

}

final_s = [];

function getPDFtext(theURL,theNAME,message,term,total_len){
    var record;
    var t_str = "";
    var in_obj = {name: theNAME, count: 0}
    PDFJS.getDocument(theURL).then(function(pdf) {
        var layers = {info: ""};
        var total = pdf.numPages;
        var fin_str = {info: ""};
        var checked = 0;
        for (var i = 1; i <= total; i++){
            var pnum = i;
            pdf.getPage(i).then(function(page){
            page.getTextContent().then( function(textContent){
                var t_str = "";
                for (var x = 0; x < textContent.items.length; x++){
                    t_str += textContent.items[x].str;
                }
                in_obj.count += countOccur(t_str,term);
                checked += 1;
                if (checked == total){
                    //message.innerHTML += "<tr><td><a target=\"_blank\" href=\""+theURL +"\">"+ theNAME +"</a></td><td>"+in_obj.count+"</td></tr>";
                    final_s.push([in_obj.count,"<tr><td><a target=\"_blank\" href=\""+theURL +"\">"+ theNAME +"</a></td><td>"+in_obj.count+"</td></tr>"]);
                    console.log("LENGTH",final_s.length,total_len);
                    if (final_s.length == total_len){
                        sendToPopup(message,final_s);
                    }
                }
            });
        }); 
    }
});

}


function countOccur(body,term){
    var check = body.toLowerCase().split(term.toLowerCase());
    return check.length - 1;
}



// ****************
// *   Read URL   *
// ****************
function readURL(theURL,theNAME,xmlString,document_root,message,term,total_len) {
    xmlString.responseType = "text";
    xmlString.onreadystatechange=function()
    {
        
        if (xmlString.readyState==4 && xmlString.status==200)
        {
            //message.innerHTML += "<tr><td><a target=\"_blank\" href=\""+theURL +"\">"+ theNAME +"</a></td><td>"+countOccur(xmlString.responseText,term)+"</td></tr>";
            final_s.push([countOccur(xmlString.responseText,term),"<tr><td><a target=\"_blank\" href=\""+theURL +"\">"+ theNAME +"</a></td><td>"+countOccur(xmlString.responseText,term)+"</td></tr>"]);
            console.log("LENGTH",final_s.length);
            if (final_s.length == total_len.length){
                sendToPopup(message,final_s);
            }
        }
    }
    xmlString.open("GET", theURL, true);
    xmlString.send(); 
}


// *******************
// *  Analyse Links  *
// *******************
function AnalyseLinks(link_lis,document_root,message,term) {
    console.log("THIS MANY LINKS",link_lis.length);
    var next_lis = [];
    var arr_info = [];
    for (var x = 0; x < link_lis.length; x++){
        // determine if the current link is a .txt file
        if (link_lis[x][0].indexOf(".txt") > -1){
            next_lis.push([link_lis[x][0],new XMLHttpRequest(),link_lis[x][1]]);
            //console.log(link_lis[x][1]);
            
        }
        // determine if the current link is a .pdf file
        else if (link_lis[x][0].indexOf(".pdf") > -1){
            next_lis.push([link_lis[x][0],link_lis[x][1]]);
        }
    }
    var goal_size = next_lis.length;
    for (x = 0; x < next_lis.length; x++){
        // run for .txt files
        if (next_lis[x].length == 3){
            readURL(next_lis[x][0],next_lis[x][2],next_lis[x][1],document_root,message,term,goal_size);
        }
        // run for .pdf files
        else {
            getPDFtext(next_lis[x][0],next_lis[x][1],message,term,goal_size);
        }
    }
}


var page_html = {term_array: []};

chrome.runtime.onMessage.addListener(function(request, sender) {
  if (request.action == "getSource") {
    for (var x = 0; x < request.source.length; x++) {
        page_html.term_array.push(request.source[x]);
    }
    
  }
});




// *********************
// *  Listen for Inp   *
// *********************
function listenForInp() {
    $("#search_entered").submit(function(e) {
    e.preventDefault();
    });
    
    // message: the textBox that holds the links containing the value
    var message = document.getElementById('message');
    // search: the submit button
    var search = document.getElementById('subbtn');
    // term: the input text box
    var term = document.getElementById('sbox');

    if (search){ //&& term.value != ''){
            search.onclick = function () {
                final_s = [];
                message.innerHTML = "<img align = \"middle\" src=\"loader.gif\">";
                AnalyseLinks(page_html.term_array,document,message,term.value);
            };
    }
    
    
}

function onWindowLoad() {
    console.log("Clicked");
    chrome.tabs.executeScript(null, {
        file: "scripts/scanPage.js"
        }, function() {
        if (chrome.runtime.lastError) {
          message.innerText = 'There was an error injecting script : \n' + chrome.runtime.lastError.message;
        }
    });
}

//var load_page_html;

//var x = document.getElementById("subbtn");

//console.log(x);



window.onload = onWindowLoad();
//document.getElementById('sbox').select();
// var myCustomKey = 88; // Shift + x
// window.addEventListener('keyup', keyboardNavigation, false);
// function keyboardNavigation(e) {
//     switch(e.which) {
//     case myCustomKey:
//     if (e.altKey) {
//         console.log("Clicked Shift X");
//     }
//     break;
//   }
// }

listenForInp();






