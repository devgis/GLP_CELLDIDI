var dom = document.getElementById("mapDiv");
var myChart = echarts.init(dom);

var option = {
    xAxis: {
        type: 'category',
        data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
    },
    yAxis: {
        type: 'value'
    },
    series: [{
        data: [820, 932, 901, 934, 1290, 1330, 1320],
        type: 'line'
    }]
};
myChart.showLoading();
var szoneid = GetQueryString("szoneid");
var ezoneid = GetQueryString("ezoneid");

//请求数据
//从后台获取gps数据
var dataurl = "./data.ashx?xcout=" + colcount + "&ycout=" + rowcount + "&startx=" + startx + "&endx=" + endx + "&starty=" + starty + "&endy=" + endy + "&t=byzone&szoneid=" + szoneid + "&ezoneid=" + ezoneid;
$.ajax({
    type: "get",
    //contentType: "application/json",
    url: dataurl,
    //data: "{}",  //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到       
    dataType: 'json',   //WebService 会返回Json类型
    success: function (result) {     //回调函数，result，返回值
        if (result.length > 0) {
            //将返回的category和series对象赋值给options对象内的category和series
            option.xAxis.data = new Array();
            option.series[0].data = new Array();
            for (var i = 0; i < result.length; i++) {
                option.xAxis.data.push(result[i].hours);
                option.series[0].data.push(result[i].power);
            }
            myChart.hideLoading();
            myChart.setOption(option);

        }
        else {
            myChart.hideLoading();
        }
    },
    error: function (err) {     //  出错s
        console.log(err);
        myChart.hideLoading();
    }
});