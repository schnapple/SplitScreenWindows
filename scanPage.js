
function GetDocs(document_root) {
    var html = '',
        node = document_root.firstChild;
    while (node) {
        switch (node.nodeType) {
        case Node.ELEMENT_NODE:
            html += node.outerHTML;
            break;
        case Node.TEXT_NODE:
            html += node.nodeValue;
            break;
        case Node.CDATA_SECTION_NODE:
            //html += '<![CDATA[' + node.nodeValue + ']]>';
            break;
        case Node.COMMENT_NODE:
            //html += '<!--' + node.nodeValue + '-->';
            break;
        case Node.DOCUMENT_TYPE_NODE:
            //html += "<!DOCTYPE " + node.name + (node.publicId ? ' PUBLIC "' + node.publicId + '"' : '') + (!node.publicId && node.systemId ? ' SYSTEM' : '') + (node.systemId ? ' "' + node.systemId + '"' : '') + '>\n';
            break;
        }
        node = node.nextSibling;
    }
    var tstr = '';

    var finstring = [];
    var len = 0;
    var temp_s;
    var url_st = document_root.URL;
    var x;
    for (x = url_st.length - 1; x >= 0; x--){
        if (url_st.charAt(x) == "/"){
            break;
        }
    }
    url_st = url_st.substr(0,x+1);
    while (len < html.length){
        if (html.charAt(len) == "<" && html.charAt(len+1) == "a"){
            temp_s = '';
            while (html.charAt(len) != "a" || html.charAt(len+1) != ">"){
                temp_s += html.charAt(len);
                len++;
            }
            finstring.push(temp_s);
            //tstr += temp_s + "\n\n\n\n\n";
        }
        len++;
    }
    var final_lis = [];
    for (var x = 0; x < finstring.length; x++){
        
        var first;
        var second = '';
        var link = finstring[x];
        link = link.substr(link.indexOf("href")+6,link.length);
        first = link.substr(0,link.indexOf("\""));
        if (first.indexOf("http") != 0){
            first = url_st + first;
        }
        if (first.indexOf(".txt") > -1 || first.indexOf(".pdf") > -1){
            var check = 0;
            while (check < link.length-1){
                if (link.charAt(check) == ">" && link.charAt(check+1) != "<"){
                    check++;
                    while (link.charAt(check) != "<" && check < link.length-1){
                        second += link.charAt(check);
                        check++;
                    }
                    break;
                }
                check++;
            }
            final_lis.push([first,second]);
        }
       
    }
    for (var z = 0; z < final_lis.length; z++){
        tstr += "\n" + final_lis[z][0] + "\n" + final_lis[z][1] + "\nBREAK\n";
    }
    
    /*var lis = html.split("<a");
    var types = [".txt",".pdf"];
    for (var i = 0; i < lis.length; i++){
        /*if (lis[i].indexOf("href=") == 0 && (lis[i].indexOf(".txt") > -1 || lis[i].indexOf(".pdf") > -1)){
            var link = lis[i].substr(6)
            link = link.substr(0,link.indexOf("\""))
            tstr += link + "\n";
            finstring.push(link);
        }
        tstr += lis[i] + "\n\n\nBREAK\n\n\n";

    }*/
    //tstr += "\n\n\n\n\n" + document_root.URL;
    //console.log("HERE");
    return final_lis;
}



function readURL(theURL,theNAME,xmlString,file_info) {
    //xmlString = new XMLHttpRequest();
    xmlString.responseType = "text";
    xmlString.onreadystatechange=function()
    {
        //console.log("WAITING");
        //console.log(xmlString.readyState);
        if (xmlString.readyState==4 && xmlString.status==200)
        {
            //console.log(file_info.strings.length);
            file_info.strings.push([theNAME,xmlString.responseText]);
        }
    }
    xmlString.open("GET", theURL, true);
    xmlString.send(); 
}



function AnalyseLinks(document_root) {
    var file_info = {strings: []};
    var infoTable = "";
    var link_lis = GetDocs(document_root);
    var next_lis = [];
    var tex = "";
    var text_lis;
    for (var x = 0; x < link_lis.length; x++){
        if (link_lis[x][0].indexOf(".txt") > -1){
            next_lis.push([link_lis[x][0],new XMLHttpRequest(),link_lis[x][1]]);
            
        }
    }
    for (x = 0; x < next_lis.length; x++){
        readURL(next_lis[x][0],next_lis[x][2],next_lis[x][1],file_info);
    }
        /*next_lis[x][1].responseType = "text";
        next_lis[x][1].onreadystatechange = function(){
            if (next_lis[x][1].readyState==4 && next_lis[x][1].status==200)
            {
                console.log("GETSHERE");
                return next_lis[x][1].responseText;
            }
        }
        next_lis[x][1].open("GET",next_lis[x][0],true);
        next_lis[x][1].send();
    }*/
    //console.log("DOES THIS FIRST");
    //console.log(file_info.strings.length);
    //while (file_info.strings.length > next_lis.length){
    //    console.log(file_info.strings.length);
    //}
    return tex;
}


chrome.runtime.sendMessage({
    action: "getSource",
    source: GetDocs(document)
});