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
        //link = link.substr(0,link.indexOf("\""))
        //tstr += link + "\nBreak\n";
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
    return tstr;
}

function AnalyseLinks(document_root,link_lis) {
    var url_st = document_root.URL;
    var x;
    for (x = url_st.length - 1; x >= 0; x--){
        if (url_st.charAt(x) == "/"){
            break;
        }
    }
    var ts = '';
    url_st = url_st.substr(0,x+1);
    for (var y = 0; y < link_lis.length; y++){
        if (link_lis[y].indexOf("http") != 0){
            link_lis[y] = url_st + link_lis[y];
            ts += link_lis[y] + "\n";
        }
    }
    return ts;
}


chrome.runtime.sendMessage({
    action: "getSource",
    source: GetDocs(document)
});