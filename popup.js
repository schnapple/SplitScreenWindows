final_s = {info: ""};

function getPDFtext(theURL,theNAME,message,term){
    //console.log(theURL);
    
    var record;
    PDFJS.getDocument(theURL).then(function(pdf) {
    var layers = {info: ""};
    var total = pdf.numPages;
    var fin_str = "";
    for (var i = 1; i <= total; i++){
        pdf.getPage(i).then( function(page){
            //console.log(page);
        page.getTextContent().then( function(textContent) {
            //console.log(textContent.items);
            var t_str = "";
            for (var x = 0; x < textContent.items.length; x++){
                //console.log(textContent.items[x].str);
                final_s.info += textContent.items[x].str;
            }
            console.log(final_s.info.length);
            //message.innerHTML += "<tr><td><a target=\"_blank\" href=\""+theURL +"\">"+ theNAME +"</a></td><td>"+countOccur(layers.info,term)+"</td></tr>";
            //console.log(layers.info)
            
        });
    });
    
}
//console.log(layers);
});

}


function countOccur(body,term){
    var check = body.split(term);
    return check.length - 1;
}




function readURL(theURL,theNAME,xmlString,document_root,message,term) {
    xmlString.responseType = "text";
    xmlString.onreadystatechange=function()
    {
        
        if (xmlString.readyState==4 && xmlString.status==200)
        {
            message.innerHTML += "<tr><td><a target=\"_blank\" href=\""+theURL +"\">"+ theNAME +"</a></td><td>"+countOccur(xmlString.responseText,term)+"</td></tr>";
            
        }
    }
    xmlString.open("GET", theURL, true);
    xmlString.send(); 
}

function AnalyseLinks(link_lis,document_root,message,term) {
    var next_lis = [];
    var arr_info = [];
    for (var x = 0; x < link_lis.length; x++){
        if (link_lis[x][0].indexOf(".txt") > -1){
            next_lis.push([link_lis[x][0],new XMLHttpRequest(),link_lis[x][1]]);
            
        }
        else if (link_lis[x][0].indexOf(".pdf") > -1){
            //getPDFtext(link_lis[x][0]);
            next_lis.push([link_lis[x][0],link_lis[x][1]]);
        }
    }
    //var final_s = {info: ""};
    for (x = 0; x < next_lis.length; x++){
        if (next_lis[x].length == 3){
            readURL(next_lis[x][0],next_lis[x][2],next_lis[x][1],document_root,message,term);
        }
        else {
            //final_s.info = "";
            getPDFtext(next_lis[x][0],next_lis[x][1],message,term,final_s);
            console.log(final_s);
        }
    }
    //return tex;
    //message.innerHtml = "hello";
}








var page_html = {term_array: []};

chrome.runtime.onMessage.addListener(function(request, sender) {
  if (request.action == "getSource") {
    for (var x = 0; x < request.source.length; x++) {
        page_html.term_array.push(request.source[x]);
    }
    //console.log(page_html);
    //message.innerHTML = "<table>";
    //AnalyseLinks(temp,document,message);
    //= request.source;
    //message.innerText += "HELLO";
  }
});




function listenForInp() {
    $("#search_entered").submit(function(e) {
    e.preventDefault();
    });
    var message = document.getElementById('message');
    var search = document.getElementById('subbtn');
    var term = document.getElementById('sbox');
    if (search){ //&& term.value != ''){
            search.onclick = function () {
                //console.log(message);
                message.innerHTML = "";
                AnalyseLinks(page_html.term_array,document,message,term.value);
            };
    }
    //console.log("Working");
    //console.log(page_html.term_array);
    //onWindowLoad();
    /*var search = document.getElementById('subbtn');
    var searched = false;
    if (search){
        search.onclick = function () {
            var term = document.getElementById('sbox');
            console.log(term.value);
            searched = true;

        }
    }
    if (searched){
        console.log("WHYFAIL");
        //searched = false;
        onWindowLoad();
    }*/
    //onWindowLoad;
}

function onWindowLoad() {
    chrome.tabs.executeScript(null, {
        file: "scanPage.js"
        }, function() {
        if (chrome.runtime.lastError) {
          message.innerText = 'There was an error injecting script : \n' + chrome.runtime.lastError.message;
        }
    });


    /*$("#search_entered").submit(function(e) {
    e.preventDefault();
    });
    var search = document.getElementById('subbtn');
    var message = document.querySelector('#message');
    if (search){
            search.onclick = function () {
                chrome.tabs.executeScript(null, {
                file: "scanPage.js"
                }, function() {
                if (chrome.runtime.lastError) {
                  message.innerText = 'There was an error injecting script : \n' + chrome.runtime.lastError.message;
                }
                });
        }
    }
    /*var search = document.getElementById('subbtn');
    search.onclick = function () {
        //location.reload();
        //console.log("HELLO");
        var message = document.querySelector('#message');
        chrome.tabs.executeScript(null, {
        file: "scanPage.js"
        }, function() {
        if (chrome.runtime.lastError) {
          message.innerText = 'There was an error injecting script : \n' + chrome.runtime.lastError.message;
        }
        });
    }*/
}

//var load_page_html;

//var x = document.getElementById("subbtn");

//console.log(x);

window.onload = onWindowLoad();

listenForInp();






