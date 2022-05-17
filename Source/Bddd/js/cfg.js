//参数
var startx = 104.1291+0.1; //开始经度
var starty = 30.7279+0.1; //开始纬度
var endx = 104.0432-0.1; //结束经度
var endy = 30.6550-0.1; //结束纬度
var rowcount = 10; //行数
var colcount = 10; //列数

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return r[2]; return null;
} 
