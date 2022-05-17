var dom = document.getElementById("mapDiv");
var myChart = echarts.init(dom);

var option = {
    title: {
        text: '小区订单分析'
    },
    tooltip: {
        trigger: 'axis'
    },
    legend: {
        data:['上车人数','下车人数']
    },
    grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
    },
    toolbox: {
        feature: {
            saveAsImage: {}
        }
    },
    xAxis: {
        type: 'category',
        boundaryGap: false,
        data: ['周一','周二','周三','周四','周五','周六','周日']
    },
    yAxis: {
        type: 'value'
    },
    series: [
        {
            name:'上车人数',
            type:'line',
            stack: '总量',
            data:[120, 132, 101, 134, 90, 230, 210]
        },
        {
            name:'下车人数',
            type:'line',
            stack: '总量',
            data:[220, 182, 191, 234, 290, 330, 310]
        }
    ]
};
myChart.showLoading();
var zoneid = GetQueryString("zoneid");

//请求数据
//从后台获取gps数据
var dataurl = "./data.ashx?xcout=" + colcount + "&ycout=" + rowcount + "&startx=" + startx + "&endx=" + endx + "&starty=" + starty + "&endy=" + endy + "&t=onezone&zoneid=" + zoneid;
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
            option.series[1].data = new Array();
            for (var i = 0; i < result.length; i++) {
                option.xAxis.data.push(result[i].timestring);
                option.series[0].data.push(result[i].startcount);
                option.series[1].data.push(result[i].endcount);
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