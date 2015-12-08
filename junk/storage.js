function readURL(theURL,theNAME,xmlString,document_root) {
    //xmlString = new XMLHttpRequest();
    xmlString.responseType = "text";
    xmlString.onreadystatechange=function()
    {
        //console.log("WAITING");
        //console.log(xmlString.readyState);
        if (xmlString.readyState==4 && xmlString.status==200)
        {
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
        readURL(next_lis[x][0],next_lis[x][2],next_lis[x][1],document_root);
    }
    //return tex;
    //message.innerHtml = "hello";
}






chrome.runtime.onMessage.addListener(function(request, sender) {
  if (request.action == "getSource") {
    //var main_lis = request.source;
    //AnalyseLinks(main_lis,document,message);
    message.innerHtml = request.source;
    //message.innerText = request.source;
    //console.log(message.length);
    //temp.innerText = message;
     //var elem = document.getElementById("message");
     //elem.innerHtml = "HELLOHELLOHELLO";
  }
});

function loadMainHtml() {
  var message = document.querySelector('#message');
  //var temp = document.querySelector('#message');
  chrome.tabs.executeScript(null, {
    file: "scanPage.js"
  }, function() {
    if (chrome.runtime.lastError) {
      message.innerText = 'There was an error injecting script : \n' + chrome.runtime.lastError.message;
    }
  });
  //console.log("HERE:");
  //console.log(message.length);
  //temp.innerText = message[0][0]; 
  //message.innerText = "HELLOHELLOHELLO";
  //message += "HEHEHHEHEHEHEHHEHEHEH\n\n\n\n\n\n\n\n";
  //var elem = document.getElementById("message");
  //elem.innerHtml = "HELLOHELLOHELLO";

}


function procHtml() {
  loadMainHtml();
}
window.onload = procHtml;


function countOccur(body,term){
    var check = body.split("term");
    return check.length - 1;
}
