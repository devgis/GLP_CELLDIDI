var dom = document.getElementById("mapDiv");
var myChart = echarts.init(dom);

var option = {
    color: ['#3398DB'],
    tooltip: {
        trigger: 'axis',
        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
        }
    },
    grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
    },
    xAxis: [
        {
            type: 'category',
            data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            axisTick: {
                alignWithLabel: true
            }
        }
    ],
    yAxis: [
        {
            type: 'value'
        }
    ],
    series: [
        {
            name: '直接访问',
            type: 'bar',
            barWidth: '60%',
            data: [10, 52, 200, 334, 390, 330, 220]
        }
    ]
};


myChart.showLoading();

var starth = GetQueryString("shour");
var endh = GetQueryString("ehour");

//请求数据
//从后台获取gps数据
var dataurl = "./data.ashx?xcout=" + colcount + "&ycout=" + rowcount + "&startx=" + startx + "&endx=" + endx + "&starty=" + starty + "&endy=" + endy + "&t=byhour&shour=" + starth + "&ehour=" + endh;
$.ajax({
    type: "get",
    //contentType: "application/json",
    url: dataurl,
    //data: "{}",  //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到       
    dataType: 'json',   //WebService 会返回Json类型
    success: function (result) {     //回调函数，result，返回值
        if (result.length > 0) {

            option.xAxis[0].data = new Array();
            option.series[0].data = new Array();
            for (var i = 0; i < result.length; i++) {
                var title = result[i].startrectid + "到" + result[i].endrectid;
                option.xAxis[0].data.push(title);
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