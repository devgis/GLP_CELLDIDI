var map = new BMap.Map("mapDiv");
//map.centerAndZoom(new BMap.Point(116.404, 39.915), 11);
map.centerAndZoom('成都',12);

var top_left_control = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT }); // 左上角，添加比例尺
//var top_left_navigation = new BMap.NavigationControl();  //左上角，添加默认缩放平移控件
var top_right_navigation = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_RIGHT }); //右上角，仅包含平移和缩放按钮 //, type: BMAP_NAVIGATION_CONTROL_SMALL
/*缩放控件type有四种类型:
BMAP_NAVIGATION_CONTROL_SMALL：仅包含平移和缩放按钮；BMAP_NAVIGATION_CONTROL_PAN:仅包含平移按钮；BMAP_NAVIGATION_CONTROL_ZOOM：仅包含缩放按钮*/

//添加控件和比例尺
map.addControl(top_left_control);
//map.addControl(top_left_navigation);
map.addControl(top_right_navigation);

map.enableScrollWheelZoom(); //启动鼠标滚轮缩放地图
map.enableKeyboard(); //启动键盘操作地图

////查询所有公交站点
//var local = new BMap.LocalSearch("成都",
//    { renderOptions: { autoViewport: true }, pageCapacity: 100 //map: map,
//    , onSearchComplete: function (results) {
//        if (local.getStatus() != BMAP_STATUS_SUCCESS) {
//            //            resultStr = "查询异常！";
//            //            console.log
//        } else {

//            var totalPages = results.getNumPages();
//            var currPage = results.getPageIndex(); // 获取当前是第几页数据  
//            // alert("totalResults:" + totalResults);  
//            for (i = 0; i < results.getCurrentNumPois(); i++) {
//                //console.log(i * results.getPageIndex());
//                console.log(results.getPoi(i));
//                //resultArray[50 * currPage + i] = results.getPoi(i);//在当前页面下获取页面中的内容  
//            }

//            if (results.getPageIndex() <= results.getNumPages() - 1) {
//                //console.log(results.Br);
////                for (var i = 0; i < results.Br.length; i++) {
////                    console.log(results.getPoi(i));
////                }
//                local.gotoPage(results.getPageIndex() + 1); // 遍历到最后一页之后不再进行下一页搜索，此时，已经获取到全部的搜索结果，  
//            }
//        }
//    }
//});
//local.search("公交站");



//动态计算绘制区域
var cellwidth = (endx - startx) / colcount;
var cellheight=(endy-starty)/rowcount;

for (var i = 0; i < rowcount; i++) {
    for (var j = 0; j < colcount; j++) {
        var pt1 = new BMap.Point(startx + i * cellwidth, starty + j * cellheight);
        var pt2 = new BMap.Point(startx + (i + 1) * cellwidth, starty + j * cellheight);
        var pt3 = new BMap.Point(startx + (i + 1) * cellwidth, starty + (j + 1) * cellheight);
        var pt4 = new BMap.Point(startx + i * cellwidth, starty + (j + 1) * cellheight);
        var polygon = new BMap.Polygon([pt1, pt2, pt3, pt4], { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });  //创建多边形

        var lbtext = i + "-" + j;
        var point = new BMap.Point(startx + (2 * i + 1) * cellwidth / 2, starty + (2 * j + 1) * cellheight / 2);
        var opts = {
            position: point,    // 指定文本标注所在的地理位置
            //offset: new BMap.Size(30, -30)    //设置文本偏移量
        }
        var label = new BMap.Label(lbtext, opts);  // 创建文本标注对象
        label.setStyle({
            color: "blue",
            fontSize: "12px",
            height: "20px",
            lineHeight: "20px",
            fontFamily: "微软雅黑"
        });


        map.addOverlay(polygon);   //增加多边形
        map.addOverlay(label);  //添加标注
    }
}


//请求数据
//从后台获取gps数据
var dataurl="./data.ashx?xcout=" + colcount + "&ycout=" + rowcount + "&startx=" + startx + "&endx=" + endx + "&starty=" + starty + "&endy=" + endy;
$.ajax({
    type: "get",
    //contentType: "application/json",
    url: dataurl,
    //data: "{}",  //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到       
    dataType: 'json',   //WebService 会返回Json类型
    success: function (result) {     //回调函数，result，返回值
        if (result.length > 0) {
            console.log(result);
        }
        //alert("ok");
    },
    error: function (err) {     //  出错s
        console.log(err);
    }
});

function showOrder()
{
    window.open("data.html");    
}

function showZoneOrder()
{
    window.open("data_rs.html");    
}

function showQueyByHour()
{
    var starthour= $("#starthour").val();
    var endhour= $("#endhour").val();
    window.open("data_hour.html?shour="+starthour+"&ehour="+endhour);    
}

function showQueyByZoneid()
{
    var startzoneid= $("#startzoneid").val();
    var endzoneid= $("#endzoneid").val();
    window.open("data_zone.html?szoneid="+startzoneid+"&ezoneid="+endzoneid);      
}

function showQueyByHourChart()
{
    var starthour= $("#starthour").val();
    var endhour= $("#endhour").val();
    window.open("data_hour_chart.html?shour="+starthour+"&ehour="+endhour);    
}

function showQueyByZoneidChart()
{
    var startzoneid= $("#startzoneid").val();
    var endzoneid= $("#endzoneid").val();
    window.open("data_zone_chart.html?szoneid="+startzoneid+"&ezoneid="+endzoneid);      
}

function showQueyByOneZoneid()
{
    var onezoneid= $("#onezoneid").val();
    window.open("data_onezone.html?zoneid="+onezoneid);      
}

function showQueyByOneZoneidChart()
{
    var onezoneid= $("#onezoneid").val();
    window.open("data_onezone_chart.html?zoneid="+onezoneid);      
}

//初始化界面空间
function init()
{
    for(var i=0; i<24;i++){ //循环添加多个值
        $("#starthour").append("<option value='"+i+"'>"+i+":00</option>");
        $("#endhour").append("<option value='"+i+"'>"+i+":00</option>");
    }

    for (var i = 0; i < rowcount; i++) {
    for (var j = 0; j < colcount; j++) {
        $("#startzoneid").append("<option value='"+i+"-"+j+"'>"+i+"-"+j+"</option>");
        $("#endzoneid").append("<option value='"+i+"-"+j+"'>"+i+"-"+j+"</option>");
        $("#onezoneid").append("<option value='"+i+"-"+j+"'>"+i+"-"+j+"</option>");
    }
    }
}

init();

function showDataInMap()
{
//绘制所有线
$.ajax({
    type: "get",
    //contentType: "application/json",
    url: "./data.ashx?t=data",
    //data: "{}",  //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到       
    dataType: 'json',   //WebService 会返回Json类型
    success: function (result) {     //回调函数，result，返回值
        if (result.length > 0) {
            for(var i=0;i<result.length;i++)
            {
                if(i>10000)
                {
                return;
                }
                var polyline = new BMap.Polyline([
		        new BMap.Point(result[i].x1, result[i].y1),
		        new BMap.Point(result[i].x2, result[i].y2)
	            ], {strokeColor:"blue", strokeWeight:1, strokeOpacity:0.5});
	            map.addOverlay(polyline);          //增加折线
             }
            
        }
    },
    error: function (err) {     //  出错s
        console.log(err);
    }
});
}