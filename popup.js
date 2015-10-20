function readURL(theURL,theNAME,xmlString,document_root,message) {
    //xmlString = new XMLHttpRequest();
    xmlString.responseType = "text";
    xmlString.onreadystatechange=function()
    {
        //console.log("WAITING");
        //console.log(xmlString.readyState);
        if (xmlString.readyState==4 && xmlString.status==200)
        {
            message.innerHTML += "<tr><td>"+theNAME+"</td></tr>"
            console.log(theNAME);
            var elem = document_root.getElementById("message");//.write(theNAME);
            elem.innerHtml = theNAME;
            //file_info.strings.push([theNAME,xmlString.responseText]);
        }
    }
    xmlString.open("GET", theURL, true);
    xmlString.send(); 
}

function AnalyseLinks(link_lis,document_root,message) {
    var next_lis = [];
    for (var x = 0; x < link_lis.length; x++){
        if (link_lis[x][0].indexOf(".txt") > -1){
            next_lis.push([link_lis[x][0],new XMLHttpRequest(),link_lis[x][1]]);
            
        }
    }
    for (x = 0; x < next_lis.length; x++){
        readURL(next_lis[x][0],next_lis[x][2],next_lis[x][1],document_root,message);
    }
    //return tex;
    //message.innerHtml = "hello";
}










chrome.runtime.onMessage.addListener(function(request, sender) {
  if (request.action == "getSource") {
    var temp = request.source;
    //message.innerHTML = "<table>";
    AnalyseLinks(temp,document,message);
    //= request.source;
    //message.innerText += "HELLO";
  }
});


function listenForInp() {

    //onWindowLoad();
    var search = document.getElementById('subbtn');
    //console.log(search);
    if (search){
        search.onclick = function () {
            console.log("HELLO");
            onWindowLoad();
        }
    }
}

function onWindowLoad() {
    var message = document.querySelector('#message');
    chrome.tabs.executeScript(null, {
    file: "scanPage.js"
    }, function() {
    if (chrome.runtime.lastError) {
      message.innerText = 'There was an error injecting script : \n' + chrome.runtime.lastError.message;
    }
    });
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

//var x = document.getElementById("subbtn");

//console.log(x);

window.onload = listenForInp();








